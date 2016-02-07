/*******************************************************************************
* File Name: therm.h
* Version 1.20
*
* Description:
*  This header file provides registers and constants associated with the
*  ThermistorCalc component.
*
* Note:
*  None.
*
********************************************************************************
* Copyright 2013, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(CY_THERMISTOR_CALC_therm_H)
#define CY_THERMISTOR_CALC_therm_H

#include "cyfitter.h"
#include "CyLib.h"

#define therm_IMPLEMENTATION         (1u)
#define therm_EQUATION_METHOD        (0u)
#define therm_LUT_METHOD             (1u)

#if (therm_IMPLEMENTATION == therm_EQUATION_METHOD)
    #include <math.h>
#endif /* (therm_IMPLEMENTATION == therm_EQUATION_METHOD) */


/***************************************
*   Conditional Compilation Parameters
***************************************/

#define therm_REF_RESISTOR           (10000)
#define therm_REF_RES_SHIFT          (0u)
#define therm_ACCURACY               (10u)
#define therm_MIN_TEMP               (0 * therm_SCALE)


/***************************************
*        Function Prototypes
***************************************/

uint32 therm_GetResistance(int16 vReference, int16 vThermistor)
                                      ;
int16 therm_GetTemperature(uint32 resT) ;


/***************************************
*           API Constants
***************************************/

#define therm_K2C                    (273.15)
#define therm_SCALE                  (100)
#define therm_LUT_SIZE               (501u)


#endif /* CY_THERMISTOR_CALC_therm_H */


/* [] END OF FILE */
