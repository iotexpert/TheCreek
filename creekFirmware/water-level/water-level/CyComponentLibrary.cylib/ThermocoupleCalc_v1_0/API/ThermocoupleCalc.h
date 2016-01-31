/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides constants and parameter values for the `$INSTANCE_NAME`
*  component.
*
* Note:
*  None
*
********************************************************************************
* Copyright 2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(CY_Thermocouple_`$INSTANCE_NAME`_H)
#define CY_Thermocouple_`$INSTANCE_NAME`_H

#include "cytypes.h"
#include "cyfitter.h"
#include "cyLib.h"


/***************************************
*   Conditional Compilation Parameters
****************************************/


/***************************************
*       Enum Types
***************************************/

/* ThermocoupleTypes constants  */
`#cy_declare_enum ThermocoupleTypes`             `$ThermocoupleTypes`

/* CalcErrorType constants  */
`#cy_declare_enum CalcErrorType`                 `$CalcErrorType`

/* PolynomialOrderType constants  */
`#cy_declare_enum PolynomialOrderType`             `$PolynomialOrderType`


/***************************************
*        Constants
***************************************/

#define     `$INSTANCE_NAME`_INIT                       (0)
#define     `$INSTANCE_NAME`_FIRST_EL_MAS               (0u)
#define     `$INSTANCE_NAME`_RANGE_MAS_0                (0u)
#define     `$INSTANCE_NAME`_RANGE_MAS_1                (1u)
#define     `$INSTANCE_NAME`_RANGE_MAS_2                (2u)
#define     `$INSTANCE_NAME`_RANGE_MAS_3                (3u)
#define     `$INSTANCE_NAME`_THREE                      (3u)
#define     `$INSTANCE_NAME`_IN_NORMALIZATION           (24u)
#define     `$INSTANCE_NAME`_24BIT_SHIFTING             (24u)
#define     `$INSTANCE_NAME`_16BIT_SHIFTING             (16u)
#define     `$INSTANCE_NAME`_8BIT_SHIFTING              (8u)
#define     `$INSTANCE_NAME`_24BIT_CUTTING              (0xFFFFFFu)
#define     `$INSTANCE_NAME`_16BIT_CUTTING              (0xFFFFu)
#define     `$INSTANCE_NAME`_8BIT_CUTTING               (0xFFu)
#define     `$INSTANCE_NAME`_V_IN_FLOAT_NORMALIZATION   (1000u)
#define     `$INSTANCE_NAME`_V_OUT_FLOAT_NORMALIZATION  (100u)
#define     `$INSTANCE_NAME`_T_IN_FLOAT_NORMALIZATION   (100u)
#define     `$INSTANCE_NAME`_T_OUT_FLOAT_NORMALIZATION  (1000u)


/***************************************
*        Function Prototypes
***************************************/

int32 `$INSTANCE_NAME`_GetTemperature(int32 voltage) `=ReentrantKeil($INSTANCE_NAME . "_GetTemperature")`;
int32 `$INSTANCE_NAME`_GetVoltage(int32 temperature) `=ReentrantKeil($INSTANCE_NAME . "_GetVoltage")`;

#if (CY_PSOC5)
    int32 `$INSTANCE_NAME`_MultShift24(int32 op1, int32 op2) `=ReentrantKeil($INSTANCE_NAME . "_MultShift24")`;
#endif /* End CY_PSOC3 */

#endif /* End CY_Thermocouple_`$INSTANCE_NAME`_H */

/* [] END OF FILE */
