/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This header file provides registers and constants associated with the
*  ThermistorCalc component.
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

#if !defined(CY_THERMISTOR_CALC_`$INSTANCE_NAME`_H)
#define CY_THERMISTOR_CALC_`$INSTANCE_NAME`_H

#include <cyfitter.h>
#include "CyLib.h"

/***************************************
*   Conditional Compilation Parameters
***************************************/

#define `$INSTANCE_NAME`_IMPLEMENTATION         (`$Implementation`u)
#define `$INSTANCE_NAME`_REF_RESISTOR           (`$RefResistor`u)
#define `$INSTANCE_NAME`_REF_RES_SHIFT          (`$RefResShift`u)
#define `$INSTANCE_NAME`_ACCURACY               (`$Accuracy`u)
#define `$INSTANCE_NAME`_MIN_TEMP               (`$MinTemp` * `$INSTANCE_NAME`_SCALE)


/***************************************
*        Function Prototypes
***************************************/

uint32 `$INSTANCE_NAME`_GetResistance(int16 Vreference, int16 VThermistor) 
                                      `=ReentrantKeil($INSTANCE_NAME . "_GetResistance")`;
int16 `$INSTANCE_NAME`_GetTemperature(uint32 ResT) `=ReentrantKeil($INSTANCE_NAME . "_GetTemperature")`;


/***************************************
*    Enumerated Types and Parameters
***************************************/

/* Enumerated Types ImplementationType, Used in parameter Implementation */

`#cy_declare_enum ImplementationType`


/***************************************
*           API Constants
***************************************/

#define `$INSTANCE_NAME`_K2C                    (273.15)
#define `$INSTANCE_NAME`_SCALE                  (100)

`$definesString`


#endif /* CY_THERMISTOR_CALC_`$INSTANCE_NAME`_H */

/* [] END OF FILE */
