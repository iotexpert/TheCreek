/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides constants and parameter values for the PRS Component.
*
* Note:
*  None
*
*********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/


#if !defined(CY_PRS_`$INSTANCE_NAME`_H)
#define CY_PRS_`$INSTANCE_NAME`_H

#include "cytypes.h"
#include "cyfitter.h"


/***************************************
*   Conditional Compilation Parameters
****************************************/

#define `$INSTANCE_NAME`_PRS_SIZE        `$Resolution`u
#define `$INSTANCE_NAME`_RUN_MODE        `$RunMode`u

/***************************************
*        Function Prototypes
****************************************/

void `$INSTANCE_NAME`_Start(void);
void `$INSTANCE_NAME`_Stop(void);
#if (`$INSTANCE_NAME`_RUN_MODE)
    void `$INSTANCE_NAME`_Step(void);
#endif

#if (`$INSTANCE_NAME`_PRS_SIZE <= 32)    /* 8-32bit - PRS */
    `$RegSize` `$INSTANCE_NAME`_Read(void);
    void `$INSTANCE_NAME`_WriteSeed(`$RegSize` seed);
    `$RegSize` `$INSTANCE_NAME`_ReadPolynomial(void);
    void `$INSTANCE_NAME`_WritePolynomial(`$RegSize` polynomial);
#else    /* 64bits - PRS */
    `$RegSize` `$INSTANCE_NAME`_ReadUpper(void);
    `$RegSize` `$INSTANCE_NAME`_ReadLower(void);
    void `$INSTANCE_NAME`_WriteSeedUpper(uint32 seed);
    void `$INSTANCE_NAME`_WriteSeedLower(uint32 seed);
    uint32 `$INSTANCE_NAME`_ReadPolynomialUpper(void);
    uint32 `$INSTANCE_NAME`_ReadPolynomialLower(void);
    void `$INSTANCE_NAME`_WritePolynomialUpper(uint32 polynomial);
    void `$INSTANCE_NAME`_WritePolynomialLower(uint32 polynomial);
#endif


/***************************************
*    Initial Parameter Constants
***************************************/

#if (`$INSTANCE_NAME`_PRS_SIZE <= 32)
    #define `$INSTANCE_NAME`_DEFAULT_POLYNOM           `$PolyValueLower`
    #define `$INSTANCE_NAME`_DEFAULT_SEED              `$SeedValueLower`
#else
    #define `$INSTANCE_NAME`_DEFAULT_POLYNOM_LOWER      `$PolyValueLower`
    #define `$INSTANCE_NAME`_DEFAULT_POLYNOM_UPPER      `$PolyValueUpper`
    #define `$INSTANCE_NAME`_DEFAULT_SEED_LOWER         `$SeedValueLower`
    #define `$INSTANCE_NAME`_DEFAULT_SEED_UPPER         `$SeedValueUpper`
#endif /* `$INSTANCE_NAME`_PRS_SIZE */


/***************************************
*             Registers
***************************************/

#if (`$INSTANCE_NAME`_PRS_SIZE <= 8)    /* 8bits - PRS */
    #define `$INSTANCE_NAME`_POLYNOM_A__D0_REG          (*(reg8 *) `$INSTANCE_NAME`_PRSdp_a__D0_REG )
    #define `$INSTANCE_NAME`_POLYNOM_A__D0_REG_PTR      ((reg8 *) `$INSTANCE_NAME`_PRSdp_a__D0_REG )
    #define `$INSTANCE_NAME`_SEED_A__A0_REG             (*(reg8 *) `$INSTANCE_NAME`_PRSdp_a__A0_REG )
    #define `$INSTANCE_NAME`_SEED_A__A0_REG_PTR         ((reg8 *) `$INSTANCE_NAME`_PRSdp_a__A0_REG )
    
#elif (`$INSTANCE_NAME`_PRS_SIZE <= 16)    /* 16bits - PRS */
    #define `$INSTANCE_NAME`_POLYNOM_A__D1_REG          (*(reg8 *) `$INSTANCE_NAME`_PRSdp_a__D1_REG )
    #define `$INSTANCE_NAME`_POLYNOM_A__D1_REG_PTR      ((reg8 *) `$INSTANCE_NAME`_PRSdp_a__D1_REG )
    #define `$INSTANCE_NAME`_POLYNOM_A__D0_REG          (*(reg8 *) `$INSTANCE_NAME`_PRSdp_a__D0_REG )
    #define `$INSTANCE_NAME`_POLYNOM_A__D0_REG_PTR      ((reg8 *) `$INSTANCE_NAME`_PRSdp_a__D0_REG )
    
    #define `$INSTANCE_NAME`_SEED_A__A1_REG             (*(reg8 *) `$INSTANCE_NAME`_PRSdp_a__A1_REG )
    #define `$INSTANCE_NAME`_SEED_A__A1_REG_PTR         ((reg8 *) `$INSTANCE_NAME`_PRSdp_a__A1_REG )
    #define `$INSTANCE_NAME`_SEED_A__A0_REG             (*(reg8 *) `$INSTANCE_NAME`_PRSdp_a__A0_REG )
    #define `$INSTANCE_NAME`_SEED_A__A0_REG_PTR         ((reg8 *) `$INSTANCE_NAME`_PRSdp_a__A0_REG )
    
#elif (`$INSTANCE_NAME`_PRS_SIZE <= 24)    /* 24bits - PRS */
    #define `$INSTANCE_NAME`_POLYNOM_B__D1_REG          (*(reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__D1_REG )
    #define `$INSTANCE_NAME`_POLYNOM_B__D1_REG_PTR      ((reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__D1_REG )
    #define `$INSTANCE_NAME`_POLYNOM_B__D0_REG          (*(reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__D0_REG )
    #define `$INSTANCE_NAME`_POLYNOM_B__D0_REG_PTR      ((reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__D0_REG )
    #define `$INSTANCE_NAME`_POLYNOM_A__D0_REG          (*(reg8 *) `$INSTANCE_NAME`_PRSdp_a__D0_REG )
    #define `$INSTANCE_NAME`_POLYNOM_A__D0_REG_PTR      ((reg8 *) `$INSTANCE_NAME`_PRSdp_a__D0_REG )
    
    #define `$INSTANCE_NAME`_SEED_B__A1_REG             (*(reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__A1_REG )
    #define `$INSTANCE_NAME`_SEED_B__A1_REG_PTR         ((reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__A1_REG )
    #define `$INSTANCE_NAME`_SEED_B__A0_REG             (*(reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__A0_REG )
    #define `$INSTANCE_NAME`_SEED_B__A0_REG_PTR         ((reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__A0_REG )
    #define `$INSTANCE_NAME`_SEED_A__A0_REG             (*(reg8 *) `$INSTANCE_NAME`_PRSdp_a__A0_REG )
    #define `$INSTANCE_NAME`_SEED_A__A0_REG_PTR         ((reg8 *) `$INSTANCE_NAME`_PRSdp_a__A0_REG )
    
#elif (`$INSTANCE_NAME`_PRS_SIZE <= 32)    /* 32bits - PRS */
    #define `$INSTANCE_NAME`_POLYNOM_B__D1_REG          (*(reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__D1_REG )
    #define `$INSTANCE_NAME`_POLYNOM_B__D1_REG_PTR      ((reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__D1_REG )
    #define `$INSTANCE_NAME`_POLYNOM_A__D1_REG          (*(reg8 *) `$INSTANCE_NAME`_PRSdp_a__D1_REG )
    #define `$INSTANCE_NAME`_POLYNOM_A__D1_REG_PTR      ((reg8 *) `$INSTANCE_NAME`_PRSdp_a__D1_REG )
    #define `$INSTANCE_NAME`_POLYNOM_B__D0_REG          (*(reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__D0_REG )
    #define `$INSTANCE_NAME`_POLYNOM_B__D0_REG_PTR      ((reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__D0_REG )
    #define `$INSTANCE_NAME`_POLYNOM_A__D0_REG          (*(reg8 *) `$INSTANCE_NAME`_PRSdp_a__D0_REG )
    #define `$INSTANCE_NAME`_POLYNOM_A__D0_REG_PTR      ((reg8 *) `$INSTANCE_NAME`_PRSdp_a__D0_REG )
    
    #define `$INSTANCE_NAME`_SEED_B__A1_REG             (*(reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__A1_REG )
    #define `$INSTANCE_NAME`_SEED_B__A1_REG_PTR         ((reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__A1_REG )
    #define `$INSTANCE_NAME`_SEED_A__A1_REG             (*(reg8 *) `$INSTANCE_NAME`_PRSdp_a__A1_REG )
    #define `$INSTANCE_NAME`_SEED_A__A1_REG_PTR         ((reg8 *) `$INSTANCE_NAME`_PRSdp_a__A1_REG )
    #define `$INSTANCE_NAME`_SEED_B__A0_REG             (*(reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__A0_REG )
    #define `$INSTANCE_NAME`_SEED_B__A0_REG_PTR         ((reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__A0_REG )
    #define `$INSTANCE_NAME`_SEED_A__A0_REG             (*(reg8 *) `$INSTANCE_NAME`_PRSdp_a__A0_REG )
    #define `$INSTANCE_NAME`_SEED_A__A0_REG_PTR         ((reg8 *) `$INSTANCE_NAME`_PRSdp_a__A0_REG )
    
#elif (`$INSTANCE_NAME`_PRS_SIZE <= 40)    /* 40bits - PRS */
    #define `$INSTANCE_NAME`_POLYNOM_UPPER_C__D1_REG            (*(reg8 *) `$INSTANCE_NAME`_b2_PRSdp_c__D1_REG )
    #define `$INSTANCE_NAME`_POLYNOM_UPPER_C__D1_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_b2_PRSdp_c__D1_REG )
    
    #define `$INSTANCE_NAME`_POLYNOM_LOWER_B__D1_REG            (*(reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__D1_REG )
    #define `$INSTANCE_NAME`_POLYNOM_LOWER_B__D1_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__D1_REG )
    #define `$INSTANCE_NAME`_POLYNOM_LOWER_C__D0_REG            (*(reg8 *) `$INSTANCE_NAME`_b2_PRSdp_c__D0_REG )
    #define `$INSTANCE_NAME`_POLYNOM_LOWER_C__D0_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_b2_PRSdp_c__D0_REG )
    #define `$INSTANCE_NAME`_POLYNOM_LOWER_B__D0_REG            (*(reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__D0_REG )
    #define `$INSTANCE_NAME`_POLYNOM_LOWER_B__D0_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__D0_REG )
    #define `$INSTANCE_NAME`_POLYNOM_LOWER_A__D0_REG            (*(reg8 *) `$INSTANCE_NAME`_PRSdp_a__D0_REG )
    #define `$INSTANCE_NAME`_POLYNOM_LOWER_A__D0_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_PRSdp_a__D0_REG )
    
    #define `$INSTANCE_NAME`_SEED_UPPER_C__A1_REG               (*(reg8 *) `$INSTANCE_NAME`_b2_PRSdp_c__A1_REG )
    #define `$INSTANCE_NAME`_SEED_UPPER_C__A1_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_b2_PRSdp_c__A1_REG )
    
    #define `$INSTANCE_NAME`_SEED_LOWER_B__A1_REG               (*(reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__A1_REG )
    #define `$INSTANCE_NAME`_SEED_LOWER_B__A1_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__A1_REG )
    #define `$INSTANCE_NAME`_SEED_LOWER_C__A0_REG               (*(reg8 *) `$INSTANCE_NAME`_b2_PRSdp_c__A0_REG )
    #define `$INSTANCE_NAME`_SEED_LOWER_C__A0_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_b2_PRSdp_c__A0_REG )
    #define `$INSTANCE_NAME`_SEED_LOWER_B__A0_REG               (*(reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__A0_REG )
    #define `$INSTANCE_NAME`_SEED_LOWER_B__A0_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__A0_REG )
    #define `$INSTANCE_NAME`_SEED_LOWER_A__A0_REG               (*(reg8 *) `$INSTANCE_NAME`_PRSdp_a__A0_REG )
    #define `$INSTANCE_NAME`_SEED_LOWER_A__A0_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_PRSdp_a__A0_REG )
    
#elif (`$INSTANCE_NAME`_PRS_SIZE <= 48)    /* 48bits - PRS */
    #define `$INSTANCE_NAME`_POLYNOM_UPPER_C__D1_REG            (*(reg8 *) `$INSTANCE_NAME`_b2_PRSdp_c__D1_REG )
    #define `$INSTANCE_NAME`_POLYNOM_UPPER_C__D1_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_b2_PRSdp_c__D1_REG )
    #define `$INSTANCE_NAME`_POLYNOM_UPPER_B__D1_REG            (*(reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__D1_REG )
    #define `$INSTANCE_NAME`_POLYNOM_UPPER_B__D1_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__D1_REG )
    
    #define `$INSTANCE_NAME`_POLYNOM_LOWER_A__D1_REG            (*(reg8 *) `$INSTANCE_NAME`_PRSdp_a__D1_REG )
    #define `$INSTANCE_NAME`_POLYNOM_LOWER_A__D1_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_PRSdp_a__D1_REG )
    #define `$INSTANCE_NAME`_POLYNOM_LOWER_C__D0_REG            (*(reg8 *) `$INSTANCE_NAME`_b2_PRSdp_c__D0_REG )
    #define `$INSTANCE_NAME`_POLYNOM_LOWER_C__D0_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_b2_PRSdp_c__D0_REG )
    #define `$INSTANCE_NAME`_POLYNOM_LOWER_B__D0_REG            (*(reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__D0_REG )
    #define `$INSTANCE_NAME`_POLYNOM_LOWER_B__D0_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__D0_REG )
    #define `$INSTANCE_NAME`_POLYNOM_LOWER_A__D0_REG            (*(reg8 *) `$INSTANCE_NAME`_PRSdp_a__D0_REG )
    #define `$INSTANCE_NAME`_POLYNOM_LOWER_A__D0_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_PRSdp_a__D0_REG )
    
    #define `$INSTANCE_NAME`_SEED_UPPER_C__A1_REG               (*(reg8 *) `$INSTANCE_NAME`_b2_PRSdp_c__A1_REG )
    #define `$INSTANCE_NAME`_SEED_UPPER_C__A1_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_b2_PRSdp_c__A1_REG )
    #define `$INSTANCE_NAME`_SEED_UPPER_B__A1_REG               (*(reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__A1_REG )
    #define `$INSTANCE_NAME`_SEED_UPPER_B__A1_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__A1_REG )
    
    #define `$INSTANCE_NAME`_SEED_LOWER_A__A1_REG               (*(reg8 *) `$INSTANCE_NAME`_PRSdp_a__A1_REG )
    #define `$INSTANCE_NAME`_SEED_LOWER_A__A1_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_PRSdp_a__A1_REG )
    #define `$INSTANCE_NAME`_SEED_LOWER_C__A0_REG               (*(reg8 *) `$INSTANCE_NAME`_b2_PRSdp_c__A0_REG )
    #define `$INSTANCE_NAME`_SEED_LOWER_C__A0_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_b2_PRSdp_c__A0_REG )
    #define `$INSTANCE_NAME`_SEED_LOWER_B__A0_REG               (*(reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__A0_REG )
    #define `$INSTANCE_NAME`_SEED_LOWER_B__A0_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__A0_REG )
    #define `$INSTANCE_NAME`_SEED_LOWER_A__A0_REG               (*(reg8 *) `$INSTANCE_NAME`_PRSdp_a__A0_REG )
    #define `$INSTANCE_NAME`_SEED_LOWER_A__A0_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_PRSdp_a__A0_REG )
    
#elif (`$INSTANCE_NAME`_PRS_SIZE <= 56)    /* 56bits - PRS */
    #define `$INSTANCE_NAME`_POLYNOM_UPPER_D__D1_REG            (*(reg8 *) `$INSTANCE_NAME`_b3_PRSdp_d__D1_REG )
    #define `$INSTANCE_NAME`_POLYNOM_UPPER_D__D1_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_b3_PRSdp_d__D1_REG )
    #define `$INSTANCE_NAME`_POLYNOM_UPPER_C__D1_REG            (*(reg8 *) `$INSTANCE_NAME`_b2_PRSdp_c__D1_REG )
    #define `$INSTANCE_NAME`_POLYNOM_UPPER_C__D1_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_b2_PRSdp_c__D1_REG )
    #define `$INSTANCE_NAME`_POLYNOM_UPPER_B__D1_REG            (*(reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__D1_REG )
    #define `$INSTANCE_NAME`_POLYNOM_UPPER_B__D1_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__D1_REG )
    
    #define `$INSTANCE_NAME`_POLYNOM_LOWER_D__D0_REG            (*(reg8 *) `$INSTANCE_NAME`_b3_PRSdp_d__D0_REG )
    #define `$INSTANCE_NAME`_POLYNOM_LOWER_D__D0_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_b3_PRSdp_d__D0_REG )
    #define `$INSTANCE_NAME`_POLYNOM_LOWER_C__D0_REG            (*(reg8 *) `$INSTANCE_NAME`_b2_PRSdp_c__D0_REG )
    #define `$INSTANCE_NAME`_POLYNOM_LOWER_C__D0_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_b2_PRSdp_c__D0_REG )
    #define `$INSTANCE_NAME`_POLYNOM_LOWER_B__D0_REG            (*(reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__D0_REG )
    #define `$INSTANCE_NAME`_POLYNOM_LOWER_B__D0_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__D0_REG )
    #define `$INSTANCE_NAME`_POLYNOM_LOWER_A__D0_REG            (*(reg8 *) `$INSTANCE_NAME`_PRSdp_a__D0_REG )
    #define `$INSTANCE_NAME`_POLYNOM_LOWER_A__D0_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_PRSdp_a__D0_REG )
    
    #define `$INSTANCE_NAME`_SEED_UPPER_D__A1_REG               (*(reg8 *) `$INSTANCE_NAME`_b3_PRSdp_d__A1_REG )
    #define `$INSTANCE_NAME`_SEED_UPPER_D__A1_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_b3_PRSdp_d__A1_REG )
    #define `$INSTANCE_NAME`_SEED_UPPER_C__A1_REG               (*(reg8 *) `$INSTANCE_NAME`_b2_PRSdp_c__A1_REG )
    #define `$INSTANCE_NAME`_SEED_UPPER_C__A1_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_b2_PRSdp_c__A1_REG )
    #define `$INSTANCE_NAME`_SEED_UPPER_B__A1_REG               (*(reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__A1_REG )
    #define `$INSTANCE_NAME`_SEED_UPPER_B__A1_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__A1_REG )
    
    #define `$INSTANCE_NAME`_SEED_LOWER_D__A0_REG               (*(reg8 *) `$INSTANCE_NAME`_b3_PRSdp_d__A0_REG )
    #define `$INSTANCE_NAME`_SEED_LOWER_D__A0_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_b3_PRSdp_d__A0_REG )
    #define `$INSTANCE_NAME`_SEED_LOWER_C__A0_REG               (*(reg8 *) `$INSTANCE_NAME`_b2_PRSdp_c__A0_REG )
    #define `$INSTANCE_NAME`_SEED_LOWER_C__A0_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_b2_PRSdp_c__A0_REG )
    #define `$INSTANCE_NAME`_SEED_LOWER_B__A0_REG               (*(reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__A0_REG )
    #define `$INSTANCE_NAME`_SEED_LOWER_B__A0_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__A0_REG )
    #define `$INSTANCE_NAME`_SEED_LOWER_A__A0_REG               (*(reg8 *) `$INSTANCE_NAME`_PRSdp_a__A0_REG )
    #define `$INSTANCE_NAME`_SEED_LOWER_A__A0_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_PRSdp_a__A0_REG )
    
#else    /* 64bits - PRS */
    #define `$INSTANCE_NAME`_POLYNOM_UPPER_D__D1_REG            (*(reg8 *) `$INSTANCE_NAME`_b3_PRSdp_d__D1_REG )
    #define `$INSTANCE_NAME`_POLYNOM_UPPER_D__D1_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_b3_PRSdp_d__D1_REG )
    #define `$INSTANCE_NAME`_POLYNOM_UPPER_C__D1_REG            (*(reg8 *) `$INSTANCE_NAME`_b2_PRSdp_c__D1_REG )
    #define `$INSTANCE_NAME`_POLYNOM_UPPER_C__D1_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_b2_PRSdp_c__D1_REG )
    #define `$INSTANCE_NAME`_POLYNOM_UPPER_B__D1_REG            (*(reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__D1_REG )
    #define `$INSTANCE_NAME`_POLYNOM_UPPER_B__D1_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__D1_REG )
    #define `$INSTANCE_NAME`_POLYNOM_UPPER_A__D1_REG            (*(reg8 *) `$INSTANCE_NAME`_PRSdp_a__D1_REG )
    #define `$INSTANCE_NAME`_POLYNOM_UPPER_A__D1_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_PRSdp_a__D1_REG )
    
    #define `$INSTANCE_NAME`_POLYNOM_LOWER_D__D0_REG            (*(reg8 *) `$INSTANCE_NAME`_b3_PRSdp_d__D0_REG )
    #define `$INSTANCE_NAME`_POLYNOM_LOWER_D__D0_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_b3_PRSdp_d__D0_REG )
    #define `$INSTANCE_NAME`_POLYNOM_LOWER_C__D0_REG            (*(reg8 *) `$INSTANCE_NAME`_b2_PRSdp_c__D0_REG )
    #define `$INSTANCE_NAME`_POLYNOM_LOWER_C__D0_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_b2_PRSdp_c__D0_REG )
    #define `$INSTANCE_NAME`_POLYNOM_LOWER_B__D0_REG            (*(reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__D0_REG )
    #define `$INSTANCE_NAME`_POLYNOM_LOWER_B__D0_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__D0_REG )
    #define `$INSTANCE_NAME`_POLYNOM_LOWER_A__D0_REG            (*(reg8 *) `$INSTANCE_NAME`_PRSdp_a__D0_REG )
    #define `$INSTANCE_NAME`_POLYNOM_LOWER_A__D0_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_PRSdp_a__D0_REG )
    
    #define `$INSTANCE_NAME`_SEED_UPPER_D__A1_REG               (*(reg8 *) `$INSTANCE_NAME`_b3_PRSdp_d__A1_REG )
    #define `$INSTANCE_NAME`_SEED_UPPER_D__A1_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_b3_PRSdp_d__A1_REG )
    #define `$INSTANCE_NAME`_SEED_UPPER_C__A1_REG               (*(reg8 *) `$INSTANCE_NAME`_b2_PRSdp_c__A1_REG )
    #define `$INSTANCE_NAME`_SEED_UPPER_C__A1_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_b2_PRSdp_c__A1_REG )
    #define `$INSTANCE_NAME`_SEED_UPPER_B__A1_REG               (*(reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__A1_REG )
    #define `$INSTANCE_NAME`_SEED_UPPER_B__A1_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__A1_REG )
    #define `$INSTANCE_NAME`_SEED_UPPER_A__A1_REG               (*(reg8 *) `$INSTANCE_NAME`_PRSdp_a__A1_REG )
    #define `$INSTANCE_NAME`_SEED_UPPER_A__A1_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_PRSdp_a__A1_REG )
    
    #define `$INSTANCE_NAME`_SEED_LOWER_D__A0_REG               (*(reg8 *) `$INSTANCE_NAME`_b3_PRSdp_d__A0_REG )
    #define `$INSTANCE_NAME`_SEED_LOWER_D__A0_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_b3_PRSdp_d__A0_REG )
    #define `$INSTANCE_NAME`_SEED_LOWER_C__A0_REG               (*(reg8 *) `$INSTANCE_NAME`_b2_PRSdp_c__A0_REG )
    #define `$INSTANCE_NAME`_SEED_LOWER_C__A0_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_b2_PRSdp_c__A0_REG )
    #define `$INSTANCE_NAME`_SEED_LOWER_B__A0_REG               (*(reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__A0_REG )
    #define `$INSTANCE_NAME`_SEED_LOWER_B__A0_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_b1_PRSdp_b__A0_REG )
    #define `$INSTANCE_NAME`_SEED_LOWER_A__A0_REG               (*(reg8 *) `$INSTANCE_NAME`_PRSdp_a__A0_REG )
    #define `$INSTANCE_NAME`_SEED_LOWER_A__A0_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_PRSdp_a__A0_REG )
#endif /*`$INSTANCE_NAME`_PRS_SIZE */

#define `$INSTANCE_NAME`_CONTROL                        (*(reg8 *) `$INSTANCE_NAME`_CtrlReg__CONTROL_REG)
#define `$INSTANCE_NAME`_CONTROL_PTR                    ((reg8 *) `$INSTANCE_NAME`_CtrlReg__CONTROL_REG)

/***************************************
*       Register Constants
***************************************/

#define `$INSTANCE_NAME`_CTRL_ENABLE                    0x01u
#define `$INSTANCE_NAME`_CTRL_RISING_EDGE               0x02u
#define `$INSTANCE_NAME`_CTRL_RESET_DFF                 0x04u

#endif  /* End CY_PRS_`$INSTANCE_NAME`_H */


/* [] END OF FILE */
