/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This header file provides registers and constants associated with the
*  RTDCalc component.
*
* Note:
*  None.
*
********************************************************************************
* Copyright 2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(CY_RTD_CALC_`$INSTANCE_NAME`_H)
#define CY_RTD_CALC_`$INSTANCE_NAME`_H

#include <cyfitter.h>


/***************************************
*   Conditional Compilation Parameters
***************************************/

#define `$INSTANCE_NAME`_RTD_TYPE           (`$RTDType`u)


/***************************************
*        Function Prototypes
***************************************/

int32 `$INSTANCE_NAME`_GetTemperature(uint32 res) `=ReentrantKeil($INSTANCE_NAME . "_GetTemperature")`;

#if(CY_PSOC5)
    int32 `$INSTANCE_NAME`_MultShift24(int32 op1, uint32 op2) `=ReentrantKeil($INSTANCE_NAME . "_MultShift24")`;
#endif /* End CY_PSOC3 */


/***************************************
*    Enumerated Types and Parameters
***************************************/

/* Enumerated Types RTDType, Used in parameter RTDType */

`#cy_declare_enum RTDType`


/***************************************
*           API Constants
***************************************/

/* Resistance value at 0 degrees C in milliohms */
#define `$INSTANCE_NAME`_ZERO_VAL_PT100             (100000u)
#define `$INSTANCE_NAME`_ZERO_VAL_PT500             (500000u)
#define `$INSTANCE_NAME`_ZERO_VAL_PT1000            (1000000u)
#define `$INSTANCE_NAME`_INIT                       (0)
#define `$INSTANCE_NAME`_FIRST_EL_MAS               (0u)
#define `$INSTANCE_NAME`_24BIT_SHIFTING             (24u)
#define `$INSTANCE_NAME`_16BIT_SHIFTING             (16u)
#define `$INSTANCE_NAME`_8BIT_SHIFTING              (8u)
#define `$INSTANCE_NAME`_24BIT_CUTTING              (0xFFFFFFu)
#define `$INSTANCE_NAME`_16BIT_CUTTING              (0xFFFFu)
#define `$INSTANCE_NAME`_8BIT_CUTTING               (0xFFu)
#define `$INSTANCE_NAME`_IN_NORMALIZATION           (24u - 10u)
#define `$INSTANCE_NAME`_IN_FLOAT_NORMALIZATION     (1000u)
#define `$INSTANCE_NAME`_OUT_FLOAT_NORMALIZATION    (100u)

#endif /* CY_RTD_CALC_`$INSTANCE_NAME`_H */

/* [] END OF FILE */
