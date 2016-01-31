/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the source code to the API for the RTDCalc Component.
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

#include "`$INSTANCE_NAME`.h"


/***************************************
*  Customizer Generated Coeffitients
***************************************/

`$Orders`

#if(CY_PSOC5)


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_MultShift24
    ********************************************************************************
    *
    * Summary:
    *  Performs the math function (op1 * op2) >> 24 using 64 bit arithmetic without
    *  any loss of precision and without overflow.
    *
    * Parameters:
    *  op1: Signed 32-bit operand
    *  op2: Unsigned 24-bit operand
    *
    * Return:
    *  Signed 32-bit result of the math calculation
    *
    *******************************************************************************/
    int32 `$INSTANCE_NAME`_MultShift24(int32 op1, uint32 op2) `=ReentrantKeil($INSTANCE_NAME . "_MultShift24")`
    {
        long long result=0;

        result = (long long) op1 * op2;
        return ((int32) (result >> `$INSTANCE_NAME`_24BIT_SHIFTING));
    }

#endif /* End CY_PSOC5 */


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetTemperature
********************************************************************************
*
* Summary:
*  Calculates the temperature from RTD resistance.
*
* Parameters:
*  res: Resistance in milliohms.
*
* Return:
*  Temperature in 1/100ths degrees C.
*
*******************************************************************************/
int32 `$INSTANCE_NAME`_GetTemperature(uint32 res) `=ReentrantKeil($INSTANCE_NAME . "_GetTemperature")`

#if (CY_PSOC3)
{    
    uint8 i;
    float temp=`$INSTANCE_NAME`_INIT;

    if (((`$INSTANCE_NAME`_RTD_TYPE == `$INSTANCE_NAME`__PT100) && (res > `$INSTANCE_NAME`_ZERO_VAL_PT100)) ||
        ((`$INSTANCE_NAME`_RTD_TYPE == `$INSTANCE_NAME`__PT500) && (res > `$INSTANCE_NAME`_ZERO_VAL_PT500)) ||
        ((`$INSTANCE_NAME`_RTD_TYPE == `$INSTANCE_NAME`__PT1000) && (res > `$INSTANCE_NAME`_ZERO_VAL_PT1000)))
    {
         /* Resistance value at 0 degrees C */
        res = (float)res;
        for (i = `$INSTANCE_NAME`_ORDER_POS - (1u); i > `$INSTANCE_NAME`_INIT; i--)
        {
            temp = (`$INSTANCE_NAME`_coeffPos[i] + temp) * res;
        }
        temp = temp + `$INSTANCE_NAME`_coeffPos[`$INSTANCE_NAME`_FIRST_EL_MAS];
    }
    else
    {
        /* Temperature below 0 degrees C */
        res = (float)res;

        for (i = `$INSTANCE_NAME`_ORDER_NEG - (1u); i > `$INSTANCE_NAME`_INIT; i--)
        {
            temp = (`$INSTANCE_NAME`_coeffNeg[i] + temp) * res;
        }
        temp = temp + `$INSTANCE_NAME`_coeffNeg[`$INSTANCE_NAME`_FIRST_EL_MAS];
    }
    return ((int32)(temp));
}
#else
{
    uint8 i;
    int32 temp=`$INSTANCE_NAME`_INIT;

    if (((`$INSTANCE_NAME`_RTD_TYPE == `$INSTANCE_NAME`__PT100) && (res > `$INSTANCE_NAME`_ZERO_VAL_PT100)) ||
        ((`$INSTANCE_NAME`_RTD_TYPE == `$INSTANCE_NAME`__PT500) && (res > `$INSTANCE_NAME`_ZERO_VAL_PT500)) ||
        ((`$INSTANCE_NAME`_RTD_TYPE == `$INSTANCE_NAME`__PT1000) && (res > `$INSTANCE_NAME`_ZERO_VAL_PT1000)))
    {
         /* Resistance value at 0 degrees C */
        res = res << (`$INSTANCE_NAME`_IN_NORMALIZATION - `$INSTANCE_NAME`_POS_INPUT_SCALE);

        for (i = `$INSTANCE_NAME`_ORDER_POS - (1u); i > `$INSTANCE_NAME`_INIT; i--)
        {
            temp = `$INSTANCE_NAME`_MultShift24((`$INSTANCE_NAME`_coeffPos[i] + temp), res);
        }
        temp = temp + `$INSTANCE_NAME`_coeffPos[`$INSTANCE_NAME`_FIRST_EL_MAS];

        if (temp < `$INSTANCE_NAME`_INIT)
        {
            temp = -temp;
            temp = temp >> `$INSTANCE_NAME`_POS_COEFF_SCALE;
            temp = -temp;
        }
        
        else
        {
            temp = temp >> `$INSTANCE_NAME`_POS_COEFF_SCALE;
        }
    }
    
    else
    {
        /* Temperature below 0 degrees C */
        res = res << (`$INSTANCE_NAME`_IN_NORMALIZATION - `$INSTANCE_NAME`_NEG_INPUT_SCALE);

        for (i = `$INSTANCE_NAME`_ORDER_NEG - (1u); i > `$INSTANCE_NAME`_INIT; i--)
        {
            temp = `$INSTANCE_NAME`_MultShift24((`$INSTANCE_NAME`_coeffNeg[i] + temp), res);
        }
        temp = temp + `$INSTANCE_NAME`_coeffNeg[`$INSTANCE_NAME`_FIRST_EL_MAS];

        if (temp < `$INSTANCE_NAME`_INIT)
        {
            temp = -temp;
            temp = temp >> `$INSTANCE_NAME`_NEG_COEFF_SCALE;
            temp = -temp;
        }
        
        else
        {
           temp = temp >> `$INSTANCE_NAME`_NEG_COEFF_SCALE;
        }
    }
    return (temp);
}   
#endif /* End PSoC3 */

/* [] END OF FILE */
