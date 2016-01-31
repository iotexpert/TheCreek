/*******************************************************************************
* File Name: `$INSTANCE_NAME`_CRC.h  
* Version `$VERSION_MAJOR`.`$VERSION_MINOR`
*
*  Description:
*     Contains the function prototypes and constants available to the CRC
*     Component.
*
*   Note:
*     None
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

#if !defined(CY_CRC_`$INSTANCE_NAME`_H)
#define CY_CRC_`$INSTANCE_NAME`_H

#include "cytypes.h"
#include "cyfitter.h"


/***************************************
* Conditional Compilation Parameters
***************************************/

#define `$INSTANCE_NAME`_CRCSize        `$Resolution`


/***************************************
*        Function Prototypes
***************************************/

void `$INSTANCE_NAME`_Start(void);
void `$INSTANCE_NAME`_Stop(void);
void `$INSTANCE_NAME`_Reset(void);
#if (`$INSTANCE_NAME`_CRCSize <= 32)    /* 8-32bit - CRC */
    void `$INSTANCE_NAME`_WriteSeed(`$RegSize` seed);
    `$RegSize` `$INSTANCE_NAME`_ReadPolynomial(void);
    void `$INSTANCE_NAME`_WritePolynomial(`$RegSize` polynomial);
    `$RegSize` `$INSTANCE_NAME`_ReadCRC(void);
#else    /* 64bit - CRC */
    void `$INSTANCE_NAME`_WriteSeedUpper(uint32 seed);
    void `$INSTANCE_NAME`_WriteSeedLower(uint32 seed);
    uint32 `$INSTANCE_NAME`_ReadPolynomialUpper(void);
    uint32 `$INSTANCE_NAME`_ReadPolynomialLower(void);
    void `$INSTANCE_NAME`_WritePolynomialUpper(uint32 polynomial);
    void `$INSTANCE_NAME`_WritePolynomialLower(uint32 polynomial);
    uint32 `$INSTANCE_NAME`_ReadCRCUpper(void);
    uint32 `$INSTANCE_NAME`_ReadCRCLower(void);
#endif


/***************************************
*     Initial Parameter Constants
***************************************/

#define `$INSTANCE_NAME`_MASK 		                    `$CRCMaskHex`

#if (`$INSTANCE_NAME`_CRCSize <= 32)
    #define `$INSTANCE_NAME`_DEFAULT_POLYNOM 		    `$PolyValueLowerHex`
    #define `$INSTANCE_NAME`_DEFAULT_SEED		  		`$SeedValueLowerHex`
#else
    #define `$INSTANCE_NAME`_DEFAULT_POLYNOM_LOWER     	`$PolyValueLowerHex`
    #define `$INSTANCE_NAME`_DEFAULT_POLYNOM_UPPER     	`$PolyValueUpperHex`
    #define `$INSTANCE_NAME`_DEFAULT_SEED_LOWER			`$SeedValueLowerHex`
    #define `$INSTANCE_NAME`_DEFAULT_SEED_UPPER		  	`$SeedValueUpperHex`
#endif


/***************************************
*             Registers
***************************************/

#if (`$INSTANCE_NAME`_CRCSize <= 8)    /* 8bit - CRC */
    #define `$INSTANCE_NAME`_POLYNOM_A__D0_REG          (*(reg8 *) `$INSTANCE_NAME`_c1DP_CRCdp_a__D0_REG )
    #define `$INSTANCE_NAME`_POLYNOM_A__D0_REG_PTR      ((reg8 *) `$INSTANCE_NAME`_c1DP_CRCdp_a__D0_REG )
    #define `$INSTANCE_NAME`_SEED_A__A0_REG             (*(reg8 *) `$INSTANCE_NAME`_c1DP_CRCdp_a__A0_REG )
    #define `$INSTANCE_NAME`_SEED_A__A0_REG_PTR         ((reg8 *) `$INSTANCE_NAME`_c1DP_CRCdp_a__A0_REG )
    
#elif (`$INSTANCE_NAME`_CRCSize <= 16)    /* 16bit - CRC */
    #define `$INSTANCE_NAME`_POLYNOM_A__D1_REG          (*(reg8 *) `$INSTANCE_NAME`_c1DP_CRCdp_a__D1_REG )
    #define `$INSTANCE_NAME`_POLYNOM_A__D1_REG_PTR      ((reg8 *) `$INSTANCE_NAME`_c1DP_CRCdp_a__D1_REG )
    #define `$INSTANCE_NAME`_POLYNOM_A__D0_REG          (*(reg8 *) `$INSTANCE_NAME`_c1DP_CRCdp_a__D0_REG )
    #define `$INSTANCE_NAME`_POLYNOM_A__D0_REG_PTR      ((reg8 *) `$INSTANCE_NAME`_c1DP_CRCdp_a__D0_REG )

    #define `$INSTANCE_NAME`_SEED_A__A1_REG             (*(reg8 *) `$INSTANCE_NAME`_c1DP_CRCdp_a__A1_REG )
    #define `$INSTANCE_NAME`_SEED_A__A1_REG_PTR         ((reg8 *) `$INSTANCE_NAME`_c1DP_CRCdp_a__A1_REG )
    #define `$INSTANCE_NAME`_SEED_A__A0_REG             (*(reg8 *) `$INSTANCE_NAME`_c1DP_CRCdp_a__A0_REG )
    #define `$INSTANCE_NAME`_SEED_A__A0_REG_PTR         ((reg8 *) `$INSTANCE_NAME`_c1DP_CRCdp_a__A0_REG )
    
#elif (`$INSTANCE_NAME`_CRCSize <= 24)    /* 24bit - CRC */
    #define `$INSTANCE_NAME`_POLYNOM_B__D1_REG          (*(reg8 *) `$INSTANCE_NAME`_c2DP_CRCdp_b__D1_REG )
    #define `$INSTANCE_NAME`_POLYNOM_B__D1_REG_PTR      ((reg8 *) `$INSTANCE_NAME`_c2DP_CRCdp_b__D1_REG )
    #define `$INSTANCE_NAME`_POLYNOM_B__D0_REG          (*(reg8 *) `$INSTANCE_NAME`_c2DP_CRCdp_b__D0_REG )
    #define `$INSTANCE_NAME`_POLYNOM_B__D0_REG_PTR      ((reg8 *) `$INSTANCE_NAME`_c2DP_CRCdp_b__D0_REG )
    #define `$INSTANCE_NAME`_POLYNOM_A__D0_REG          (*(reg8 *) `$INSTANCE_NAME`_c2DP_CRCdp_a__D0_REG )
    #define `$INSTANCE_NAME`_POLYNOM_A__D0_REG_PTR      ((reg8 *) `$INSTANCE_NAME`_c2DP_CRCdp_a__D0_REG )

    #define `$INSTANCE_NAME`_SEED_B__A1_REG             (*(reg8 *) `$INSTANCE_NAME`_c2DP_CRCdp_b__A1_REG )
    #define `$INSTANCE_NAME`_SEED_B__A1_REG_PTR         ((reg8 *) `$INSTANCE_NAME`_c2DP_CRCdp_b__A1_REG )
    #define `$INSTANCE_NAME`_SEED_B__A0_REG             (*(reg8 *) `$INSTANCE_NAME`_c2DP_CRCdp_b__A0_REG )
    #define `$INSTANCE_NAME`_SEED_B__A0_REG_PTR         ((reg8 *) `$INSTANCE_NAME`_c2DP_CRCdp_b__A0_REG )
    #define `$INSTANCE_NAME`_SEED_A__A0_REG             (*(reg8 *) `$INSTANCE_NAME`_c2DP_CRCdp_a__A0_REG )
    #define `$INSTANCE_NAME`_SEED_A__A0_REG_PTR         ((reg8 *) `$INSTANCE_NAME`_c2DP_CRCdp_a__A0_REG )
    
#elif (`$INSTANCE_NAME`_CRCSize <= 32)    /* 32bit - CRC */
    #define `$INSTANCE_NAME`_POLYNOM_B__D1_REG          (*(reg8 *) `$INSTANCE_NAME`_c2DP_CRCdp_b__D1_REG )
    #define `$INSTANCE_NAME`_POLYNOM_B__D1_REG_PTR      ((reg8 *) `$INSTANCE_NAME`_c2DP_CRCdp_b__D1_REG )
    #define `$INSTANCE_NAME`_POLYNOM_A__D1_REG          (*(reg8 *) `$INSTANCE_NAME`_c2DP_CRCdp_a__D1_REG )
    #define `$INSTANCE_NAME`_POLYNOM_A__D1_REG_PTR      ((reg8 *) `$INSTANCE_NAME`_c2DP_CRCdp_a__D1_REG )
    #define `$INSTANCE_NAME`_POLYNOM_B__D0_REG          (*(reg8 *) `$INSTANCE_NAME`_c2DP_CRCdp_b__D0_REG )
    #define `$INSTANCE_NAME`_POLYNOM_B__D0_REG_PTR      ((reg8 *) `$INSTANCE_NAME`_c2DP_CRCdp_b__D0_REG )
    #define `$INSTANCE_NAME`_POLYNOM_A__D0_REG          (*(reg8 *) `$INSTANCE_NAME`_c2DP_CRCdp_a__D0_REG )
    #define `$INSTANCE_NAME`_POLYNOM_A__D0_REG_PTR      ((reg8 *) `$INSTANCE_NAME`_c2DP_CRCdp_a__D0_REG )

    #define `$INSTANCE_NAME`_SEED_B__A1_REG             (*(reg8 *) `$INSTANCE_NAME`_c2DP_CRCdp_b__A1_REG )
    #define `$INSTANCE_NAME`_SEED_B__A1_REG_PTR         ((reg8 *) `$INSTANCE_NAME`_c2DP_CRCdp_b__A1_REG )
    #define `$INSTANCE_NAME`_SEED_A__A1_REG             (*(reg8 *) `$INSTANCE_NAME`_c2DP_CRCdp_a__A1_REG )
    #define `$INSTANCE_NAME`_SEED_A__A1_REG_PTR         ((reg8 *) `$INSTANCE_NAME`_c2DP_CRCdp_a__A1_REG )
    #define `$INSTANCE_NAME`_SEED_B__A0_REG             (*(reg8 *) `$INSTANCE_NAME`_c2DP_CRCdp_b__A0_REG )
    #define `$INSTANCE_NAME`_SEED_B__A0_REG_PTR         ((reg8 *) `$INSTANCE_NAME`_c2DP_CRCdp_b__A0_REG )
    #define `$INSTANCE_NAME`_SEED_A__A0_REG             (*(reg8 *) `$INSTANCE_NAME`_c2DP_CRCdp_a__A0_REG )
    #define `$INSTANCE_NAME`_SEED_A__A0_REG_PTR         ((reg8 *) `$INSTANCE_NAME`_c2DP_CRCdp_a__A0_REG )
    
#else/* 64bit - CRC */
    #if (`$INSTANCE_NAME`_CRCSize <= 40)
        #define `$INSTANCE_NAME`_POLYNOM_UPPER_C__D1_REG            (*(reg8 *) `$INSTANCE_NAME`_c3DP_CRCdp_c__D1_REG )
        #define `$INSTANCE_NAME`_POLYNOM_UPPER_C__D1_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_c3DP_CRCdp_c__D1_REG )

        #define `$INSTANCE_NAME`_POLYNOM_LOWER_B__D1_REG            (*(reg8 *) `$INSTANCE_NAME`_c3DP_CRCdp_b__D1_REG )
        #define `$INSTANCE_NAME`_POLYNOM_LOWER_B__D1_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_c3DP_CRCdp_b__D1_REG )
        #define `$INSTANCE_NAME`_POLYNOM_LOWER_C__D0_REG            (*(reg8 *) `$INSTANCE_NAME`_c3DP_CRCdp_c__D0_REG )
        #define `$INSTANCE_NAME`_POLYNOM_LOWER_C__D0_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_c3DP_CRCdp_c__D0_REG )
        #define `$INSTANCE_NAME`_POLYNOM_LOWER_B__D0_REG            (*(reg8 *) `$INSTANCE_NAME`_c3DP_CRCdp_b__D0_REG )
        #define `$INSTANCE_NAME`_POLYNOM_LOWER_B__D0_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_c3DP_CRCdp_b__D0_REG )
        #define `$INSTANCE_NAME`_POLYNOM_LOWER_A__D0_REG            (*(reg8 *) `$INSTANCE_NAME`_c3DP_CRCdp_a__D0_REG )
        #define `$INSTANCE_NAME`_POLYNOM_LOWER_A__D0_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_c3DP_CRCdp_a__D0_REG )

        #define `$INSTANCE_NAME`_SEED_UPPER_C__A1_REG               (*(reg8 *) `$INSTANCE_NAME`_c3DP_CRCdp_c__A1_REG )
        #define `$INSTANCE_NAME`_SEED_UPPER_C__A1_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_c3DP_CRCdp_c__A1_REG )

        #define `$INSTANCE_NAME`_SEED_LOWER_B__A1_REG               (*(reg8 *) `$INSTANCE_NAME`_c3DP_CRCdp_b__A1_REG )
        #define `$INSTANCE_NAME`_SEED_LOWER_B__A1_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_c3DP_CRCdp_b__A1_REG )
        #define `$INSTANCE_NAME`_SEED_LOWER_C__A0_REG               (*(reg8 *) `$INSTANCE_NAME`_c3DP_CRCdp_c__A0_REG )
        #define `$INSTANCE_NAME`_SEED_LOWER_C__A0_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_c3DP_CRCdp_c__A0_REG )
        #define `$INSTANCE_NAME`_SEED_LOWER_B__A0_REG               (*(reg8 *) `$INSTANCE_NAME`_c3DP_CRCdp_b__A0_REG )
        #define `$INSTANCE_NAME`_SEED_LOWER_B__A0_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_c3DP_CRCdp_b__A0_REG )
        #define `$INSTANCE_NAME`_SEED_LOWER_A__A0_REG               (*(reg8 *) `$INSTANCE_NAME`_c3DP_CRCdp_a__A0_REG )
        #define `$INSTANCE_NAME`_SEED_LOWER_A__A0_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_c3DP_CRCdp_a__A0_REG )
    
    #elif (`$INSTANCE_NAME`_CRCSize <= 48)
        #define `$INSTANCE_NAME`_POLYNOM_UPPER_C__D1_REG            (*(reg8 *) `$INSTANCE_NAME`_c3DP_CRCdp_c__D1_REG )
        #define `$INSTANCE_NAME`_POLYNOM_UPPER_C__D1_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_c3DP_CRCdp_c__D1_REG )
        #define `$INSTANCE_NAME`_POLYNOM_UPPER_B__D1_REG            (*(reg8 *) `$INSTANCE_NAME`_c3DP_CRCdp_b__D1_REG )
        #define `$INSTANCE_NAME`_POLYNOM_UPPER_B__D1_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_c3DP_CRCdp_b__D1_REG )

        #define `$INSTANCE_NAME`_POLYNOM_LOWER_A__D1_REG            (*(reg8 *) `$INSTANCE_NAME`_c3DP_CRCdp_a__D1_REG )
        #define `$INSTANCE_NAME`_POLYNOM_LOWER_A__D1_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_c3DP_CRCdp_a__D1_REG )
        #define `$INSTANCE_NAME`_POLYNOM_LOWER_C__D0_REG            (*(reg8 *) `$INSTANCE_NAME`_c3DP_CRCdp_c__D0_REG )
        #define `$INSTANCE_NAME`_POLYNOM_LOWER_C__D0_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_c3DP_CRCdp_c__D0_REG )
        #define `$INSTANCE_NAME`_POLYNOM_LOWER_B__D0_REG            (*(reg8 *) `$INSTANCE_NAME`_c3DP_CRCdp_b__D0_REG )
        #define `$INSTANCE_NAME`_POLYNOM_LOWER_B__D0_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_c3DP_CRCdp_b__D0_REG )
        #define `$INSTANCE_NAME`_POLYNOM_LOWER_A__D0_REG            (*(reg8 *) `$INSTANCE_NAME`_c3DP_CRCdp_a__D0_REG )
        #define `$INSTANCE_NAME`_POLYNOM_LOWER_A__D0_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_c3DP_CRCdp_a__D0_REG )

        #define `$INSTANCE_NAME`_SEED_UPPER_C__A1_REG               (*(reg8 *) `$INSTANCE_NAME`_c3DP_CRCdp_c__A1_REG )
        #define `$INSTANCE_NAME`_SEED_UPPER_C__A1_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_c3DP_CRCdp_c__A1_REG )
        #define `$INSTANCE_NAME`_SEED_UPPER_B__A1_REG               (*(reg8 *) `$INSTANCE_NAME`_c3DP_CRCdp_b__A1_REG )
        #define `$INSTANCE_NAME`_SEED_UPPER_B__A1_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_c3DP_CRCdp_b__A1_REG )

        #define `$INSTANCE_NAME`_SEED_LOWER_A__A1_REG               (*(reg8 *) `$INSTANCE_NAME`_c3DP_CRCdp_a__A1_REG )
        #define `$INSTANCE_NAME`_SEED_LOWER_A__A1_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_c3DP_CRCdp_a__A1_REG )
        #define `$INSTANCE_NAME`_SEED_LOWER_C__A0_REG               (*(reg8 *) `$INSTANCE_NAME`_c3DP_CRCdp_c__A0_REG )
        #define `$INSTANCE_NAME`_SEED_LOWER_C__A0_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_c3DP_CRCdp_c__A0_REG )
        #define `$INSTANCE_NAME`_SEED_LOWER_B__A0_REG               (*(reg8 *) `$INSTANCE_NAME`_c3DP_CRCdp_b__A0_REG )
        #define `$INSTANCE_NAME`_SEED_LOWER_B__A0_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_c3DP_CRCdp_b__A0_REG )
        #define `$INSTANCE_NAME`_SEED_LOWER_A__A0_REG               (*(reg8 *) `$INSTANCE_NAME`_c3DP_CRCdp_a__A0_REG )
        #define `$INSTANCE_NAME`_SEED_LOWER_A__A0_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_c3DP_CRCdp_a__A0_REG )
        
    #elif (`$INSTANCE_NAME`_CRCSize <= 56)
        #define `$INSTANCE_NAME`_POLYNOM_UPPER_D__D1_REG            (*(reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_d__D1_REG )
        #define `$INSTANCE_NAME`_POLYNOM_UPPER_D__D1_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_d__D1_REG )
        #define `$INSTANCE_NAME`_POLYNOM_UPPER_C__D1_REG            (*(reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_c__D1_REG )
        #define `$INSTANCE_NAME`_POLYNOM_UPPER_C__D1_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_c__D1_REG )
        #define `$INSTANCE_NAME`_POLYNOM_UPPER_B__D1_REG            (*(reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_b__D1_REG )
        #define `$INSTANCE_NAME`_POLYNOM_UPPER_B__D1_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_b__D1_REG )

        #define `$INSTANCE_NAME`_POLYNOM_LOWER_D__D0_REG            (*(reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_d__D0_REG )
        #define `$INSTANCE_NAME`_POLYNOM_LOWER_D__D0_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_d__D0_REG )
        #define `$INSTANCE_NAME`_POLYNOM_LOWER_C__D0_REG            (*(reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_c__D0_REG )
        #define `$INSTANCE_NAME`_POLYNOM_LOWER_C__D0_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_c__D0_REG )
        #define `$INSTANCE_NAME`_POLYNOM_LOWER_B__D0_REG            (*(reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_b__D0_REG )
        #define `$INSTANCE_NAME`_POLYNOM_LOWER_B__D0_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_b__D0_REG )
        #define `$INSTANCE_NAME`_POLYNOM_LOWER_A__D0_REG            (*(reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_a__D0_REG )
        #define `$INSTANCE_NAME`_POLYNOM_LOWER_A__D0_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_a__D0_REG )

        #define `$INSTANCE_NAME`_SEED_UPPER_D__A1_REG               (*(reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_d__A1_REG )
        #define `$INSTANCE_NAME`_SEED_UPPER_D__A1_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_d__A1_REG )
        #define `$INSTANCE_NAME`_SEED_UPPER_C__A1_REG               (*(reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_c__A1_REG )
        #define `$INSTANCE_NAME`_SEED_UPPER_C__A1_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_c__A1_REG )
        #define `$INSTANCE_NAME`_SEED_UPPER_B__A1_REG               (*(reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_b__A1_REG )
        #define `$INSTANCE_NAME`_SEED_UPPER_B__A1_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_b__A1_REG )

        #define `$INSTANCE_NAME`_SEED_LOWER_D__A0_REG               (*(reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_d__A0_REG )
        #define `$INSTANCE_NAME`_SEED_LOWER_D__A0_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_d__A0_REG )
        #define `$INSTANCE_NAME`_SEED_LOWER_C__A0_REG               (*(reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_c__A0_REG )
        #define `$INSTANCE_NAME`_SEED_LOWER_C__A0_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_c__A0_REG )
        #define `$INSTANCE_NAME`_SEED_LOWER_B__A0_REG               (*(reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_b__A0_REG )
        #define `$INSTANCE_NAME`_SEED_LOWER_B__A0_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_b__A0_REG )
        #define `$INSTANCE_NAME`_SEED_LOWER_A__A0_REG               (*(reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_a__A0_REG )
        #define `$INSTANCE_NAME`_SEED_LOWER_A__A0_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_a__A0_REG )
    
    #else
        #define `$INSTANCE_NAME`_POLYNOM_UPPER_D__D1_REG            (*(reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_d__D1_REG )
        #define `$INSTANCE_NAME`_POLYNOM_UPPER_D__D1_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_d__D1_REG )
        #define `$INSTANCE_NAME`_POLYNOM_UPPER_C__D1_REG            (*(reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_c__D1_REG )
        #define `$INSTANCE_NAME`_POLYNOM_UPPER_C__D1_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_c__D1_REG )
        #define `$INSTANCE_NAME`_POLYNOM_UPPER_B__D1_REG            (*(reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_b__D1_REG )
        #define `$INSTANCE_NAME`_POLYNOM_UPPER_B__D1_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_b__D1_REG )
        #define `$INSTANCE_NAME`_POLYNOM_UPPER_A__D1_REG            (*(reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_a__D1_REG )
        #define `$INSTANCE_NAME`_POLYNOM_UPPER_A__D1_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_a__D1_REG )

        #define `$INSTANCE_NAME`_POLYNOM_LOWER_D__D0_REG            (*(reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_d__D0_REG )
        #define `$INSTANCE_NAME`_POLYNOM_LOWER_D__D0_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_d__D0_REG )
        #define `$INSTANCE_NAME`_POLYNOM_LOWER_C__D0_REG            (*(reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_c__D0_REG )
        #define `$INSTANCE_NAME`_POLYNOM_LOWER_C__D0_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_c__D0_REG )
        #define `$INSTANCE_NAME`_POLYNOM_LOWER_B__D0_REG            (*(reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_b__D0_REG )
        #define `$INSTANCE_NAME`_POLYNOM_LOWER_B__D0_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_b__D0_REG )
        #define `$INSTANCE_NAME`_POLYNOM_LOWER_A__D0_REG            (*(reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_a__D0_REG )
        #define `$INSTANCE_NAME`_POLYNOM_LOWER_A__D0_REG_PTR        ((reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_a__D0_REG )

        #define `$INSTANCE_NAME`_SEED_UPPER_D__A1_REG               (*(reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_d__A1_REG )
        #define `$INSTANCE_NAME`_SEED_UPPER_D__A1_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_d__A1_REG )
        #define `$INSTANCE_NAME`_SEED_UPPER_C__A1_REG               (*(reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_c__A1_REG )
        #define `$INSTANCE_NAME`_SEED_UPPER_C__A1_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_c__A1_REG )
        #define `$INSTANCE_NAME`_SEED_UPPER_B__A1_REG               (*(reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_b__A1_REG )
        #define `$INSTANCE_NAME`_SEED_UPPER_B__A1_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_b__A1_REG )
        #define `$INSTANCE_NAME`_SEED_UPPER_A__A1_REG               (*(reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_a__A1_REG )
        #define `$INSTANCE_NAME`_SEED_UPPER_A__A1_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_a__A1_REG )

        #define `$INSTANCE_NAME`_SEED_LOWER_D__A0_REG               (*(reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_d__A0_REG )
        #define `$INSTANCE_NAME`_SEED_LOWER_D__A0_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_d__A0_REG )
        #define `$INSTANCE_NAME`_SEED_LOWER_C__A0_REG               (*(reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_c__A0_REG )
        #define `$INSTANCE_NAME`_SEED_LOWER_C__A0_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_c__A0_REG )
        #define `$INSTANCE_NAME`_SEED_LOWER_B__A0_REG               (*(reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_b__A0_REG )
        #define `$INSTANCE_NAME`_SEED_LOWER_B__A0_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_b__A0_REG )
        #define `$INSTANCE_NAME`_SEED_LOWER_A__A0_REG               (*(reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_a__A0_REG )
        #define `$INSTANCE_NAME`_SEED_LOWER_A__A0_REG_PTR           ((reg8 *) `$INSTANCE_NAME`_c4DP_CRCdp_a__A0_REG )
    #endif
#endif

#define `$INSTANCE_NAME`_CONTROL            		    (*(reg8 *) `$INSTANCE_NAME`_ctrlreg__CONTROL_REG)
#define `$INSTANCE_NAME`_CONTROL_PTR        			((reg8 *) `$INSTANCE_NAME`_ctrlreg__CONTROL_REG)


/***************************************
*        Register Constants
***************************************/

#define `$INSTANCE_NAME`_CTRL_ENABLE				    0x01u
#define `$INSTANCE_NAME`_CTRL_RISING_EDGE				0x02u
#define `$INSTANCE_NAME`_CTRL_RESET_DFF					0x04u
	
#endif /* CY_CRC_`$INSTANCE_NAME`_H */


/* [] END OF FILE */
