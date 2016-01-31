/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the source code to the API for the `$INSTANCE_NAME`
*  component
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

#include "`$INSTANCE_NAME`.h"


/***************************************
*  Customizer Generated Coeffitients
***************************************/

`$Coefficients`

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
    int32 `$INSTANCE_NAME`_MultShift24(int32 op1, int32 op2) `=ReentrantKeil($INSTANCE_NAME . "_MultShift24")`
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
*  This function takes thermocouple voltage in microvolt as i/p and calculates
*  temperature corresponding to the thermocouple voltage. The order of the
*  polynomial and the polynomial coefficients used to convert voltage to
*  temperature are dependent on the type of thermocouple selected.
*
* Parameters:
*  int32 voltage : Thermocouple voltage measured in microvolts.
*
* Return:
*  int32 : Temperature in 1/100 degree C
*
*******************************************************************************/
int32 `$INSTANCE_NAME`_GetTemperature(int32 voltage) `=ReentrantKeil($INSTANCE_NAME . "_GetTemperature")`

#if(CY_PSOC3)
{
    /* Variable to store temperature */
    float temp = `$INSTANCE_NAME`_INIT;
    float voltage_norm =  `$INSTANCE_NAME`_INIT;
    uint8 i = `$INSTANCE_NAME`_INIT;

    #if (`$INSTANCE_NAME`_VT_RANGE_LEN == `$INSTANCE_NAME`_THREE)
    {
        if(voltage < `$INSTANCE_NAME`_voltRange[`$INSTANCE_NAME`_RANGE_MAS_0] )
        {
            voltage_norm = (float)voltage;

            for (i=`$INSTANCE_NAME`_ORDER_VT - (1u); i > (0u); i--)
            {
                temp = (temp + `$INSTANCE_NAME`_coeffVT[i][`$INSTANCE_NAME`_RANGE_MAS_0]) * voltage_norm;
            }
            temp = (temp + `$INSTANCE_NAME`_coeffVT[`$INSTANCE_NAME`_FIRST_EL_MAS][`$INSTANCE_NAME`_RANGE_MAS_0]);
        }

        else if(voltage <= `$INSTANCE_NAME`_voltRange[`$INSTANCE_NAME`_RANGE_MAS_1] )
        {
            voltage_norm = (float)voltage;

            for (i=`$INSTANCE_NAME`_ORDER_VT - (1u); i > (0u); i--)
            {
                temp = (temp + `$INSTANCE_NAME`_coeffVT[i][`$INSTANCE_NAME`_RANGE_MAS_1]) * voltage_norm;
            }
            temp = (temp + `$INSTANCE_NAME`_coeffVT[`$INSTANCE_NAME`_FIRST_EL_MAS][`$INSTANCE_NAME`_RANGE_MAS_1]);
        }

        else if(voltage <= `$INSTANCE_NAME`_voltRange[`$INSTANCE_NAME`_RANGE_MAS_2] )
        {
            voltage_norm = (float)voltage;

            for (i=`$INSTANCE_NAME`_ORDER_VT - (1u); i > (0u); i--)
            {
                temp = (temp + `$INSTANCE_NAME`_coeffVT[i][`$INSTANCE_NAME`_RANGE_MAS_2]) * voltage_norm;
            }
            temp = (temp + `$INSTANCE_NAME`_coeffVT[`$INSTANCE_NAME`_FIRST_EL_MAS][`$INSTANCE_NAME`_RANGE_MAS_2]);
        }
        
        else
        {
            voltage_norm = (float)voltage;

            for (i=`$INSTANCE_NAME`_ORDER_VT - (1u); i > (0u); i--)
            {
                temp = (temp + `$INSTANCE_NAME`_coeffVT[i][`$INSTANCE_NAME`_RANGE_MAS_3]) * voltage_norm;
            }
            temp = (temp + `$INSTANCE_NAME`_coeffVT[`$INSTANCE_NAME`_FIRST_EL_MAS][`$INSTANCE_NAME`_RANGE_MAS_3]);
        }
    }
  
    #else
    {
        if(voltage < `$INSTANCE_NAME`_voltRange[`$INSTANCE_NAME`_RANGE_MAS_0] )
        {
            voltage_norm = (float)voltage;

            for (i=`$INSTANCE_NAME`_ORDER_VT - (1u); i > (0u); i--)
            {
                temp = (temp + `$INSTANCE_NAME`_coeffVT[i][`$INSTANCE_NAME`_RANGE_MAS_0]) * voltage_norm;
            }
            temp = (temp + `$INSTANCE_NAME`_coeffVT[`$INSTANCE_NAME`_FIRST_EL_MAS][`$INSTANCE_NAME`_RANGE_MAS_0]);
        }

        else if(voltage <= `$INSTANCE_NAME`_voltRange[`$INSTANCE_NAME`_RANGE_MAS_1] )
        {
            voltage_norm = (float)voltage;

            for (i=`$INSTANCE_NAME`_ORDER_VT - (1u); i > (0u); i--)
            {
                temp = (temp + `$INSTANCE_NAME`_coeffVT[i][`$INSTANCE_NAME`_RANGE_MAS_1]) * voltage_norm;
            }
            temp = (temp + `$INSTANCE_NAME`_coeffVT[`$INSTANCE_NAME`_FIRST_EL_MAS][`$INSTANCE_NAME`_RANGE_MAS_1]);
        }

        else
        {
            voltage_norm = (float)voltage;

            for (i=`$INSTANCE_NAME`_ORDER_VT - (1u); i > (0u); i--)
            {
                temp = (temp + `$INSTANCE_NAME`_coeffVT[i][`$INSTANCE_NAME`_RANGE_MAS_2]) * voltage_norm;
            }
            temp = (temp + `$INSTANCE_NAME`_coeffVT[`$INSTANCE_NAME`_FIRST_EL_MAS][`$INSTANCE_NAME`_RANGE_MAS_2]);
        }
    }
    
    #endif /* End  `$INSTANCE_NAME`_VT_RANGE_LEN == `$INSTANCE_NAME`_THREE */
    
    return ((int32)(temp));
}
#else
{
    /* Variable to store temperature */
    int32 temp = `$INSTANCE_NAME`_INIT;
    uint8 i = `$INSTANCE_NAME`_INIT;

    #if (`$INSTANCE_NAME`_VT_RANGE_LEN == `$INSTANCE_NAME`_THREE)
    {
        if(voltage < `$INSTANCE_NAME`_voltRange[`$INSTANCE_NAME`_RANGE_MAS_0] )
        {
            voltage = voltage << (`$INSTANCE_NAME`_IN_NORMALIZATION -
                      `$INSTANCE_NAME`_XScaleVT[`$INSTANCE_NAME`_RANGE_MAS_0]);

            for (i=`$INSTANCE_NAME`_ORDER_VT - (1u); i > (0u); i--)
            {
                temp = `$INSTANCE_NAME`_MultShift24((`$INSTANCE_NAME`_coeffVT[i][`$INSTANCE_NAME`_RANGE_MAS_0] +
                       temp), voltage);
            }

            if(`$INSTANCE_NAME`_CoefScaleVT[`$INSTANCE_NAME`_RANGE_MAS_0] < `$INSTANCE_NAME`_INIT)
            {
                temp = (temp + `$INSTANCE_NAME`_coeffVT[`$INSTANCE_NAME`_FIRST_EL_MAS][`$INSTANCE_NAME`_RANGE_MAS_0]) <<
                       `$INSTANCE_NAME`_CoefScaleVT[`$INSTANCE_NAME`_RANGE_MAS_0];
            }
            
            else
            {
                temp = (temp + `$INSTANCE_NAME`_coeffVT[`$INSTANCE_NAME`_FIRST_EL_MAS][`$INSTANCE_NAME`_RANGE_MAS_0]) >>
                       `$INSTANCE_NAME`_CoefScaleVT[`$INSTANCE_NAME`_RANGE_MAS_0];
            }
        }

        else if(voltage <= `$INSTANCE_NAME`_voltRange[`$INSTANCE_NAME`_RANGE_MAS_1] )
        {
            voltage = voltage << (`$INSTANCE_NAME`_IN_NORMALIZATION -
                      `$INSTANCE_NAME`_XScaleVT[`$INSTANCE_NAME`_RANGE_MAS_1]);

            for (i=`$INSTANCE_NAME`_ORDER_VT - (1u); i > (0u); i--)
            {
                temp = `$INSTANCE_NAME`_MultShift24((`$INSTANCE_NAME`_coeffVT[i][`$INSTANCE_NAME`_RANGE_MAS_1] +
                       temp), voltage);
            }
            temp = (temp + `$INSTANCE_NAME`_coeffVT[`$INSTANCE_NAME`_FIRST_EL_MAS][`$INSTANCE_NAME`_RANGE_MAS_1]) >>
                   `$INSTANCE_NAME`_CoefScaleVT[`$INSTANCE_NAME`_RANGE_MAS_1];
        }

        else if(voltage <= `$INSTANCE_NAME`_voltRange[`$INSTANCE_NAME`_RANGE_MAS_2] )
        {
            voltage = voltage << (`$INSTANCE_NAME`_IN_NORMALIZATION -
                      `$INSTANCE_NAME`_XScaleVT[`$INSTANCE_NAME`_RANGE_MAS_2]);

            for (i=`$INSTANCE_NAME`_ORDER_VT - (1u); i > (0u); i--)
            {
                temp = `$INSTANCE_NAME`_MultShift24((`$INSTANCE_NAME`_coeffVT[i][`$INSTANCE_NAME`_RANGE_MAS_2] +
                       temp), voltage);
            }
            temp = (temp + `$INSTANCE_NAME`_coeffVT[`$INSTANCE_NAME`_FIRST_EL_MAS][`$INSTANCE_NAME`_RANGE_MAS_2]) >>
                   `$INSTANCE_NAME`_CoefScaleVT[`$INSTANCE_NAME`_RANGE_MAS_2];
        }
        
        else
        {
            voltage = voltage << (`$INSTANCE_NAME`_IN_NORMALIZATION -
                      `$INSTANCE_NAME`_XScaleVT[`$INSTANCE_NAME`_RANGE_MAS_3]);

            for (i=`$INSTANCE_NAME`_ORDER_VT - (1u); i > (0u); i--)
            {
                temp = `$INSTANCE_NAME`_MultShift24((`$INSTANCE_NAME`_coeffVT[i][`$INSTANCE_NAME`_RANGE_MAS_3] +
                       temp), voltage);
            }
            temp = (temp + `$INSTANCE_NAME`_coeffVT[`$INSTANCE_NAME`_FIRST_EL_MAS][`$INSTANCE_NAME`_RANGE_MAS_3]) >>
                   `$INSTANCE_NAME`_CoefScaleVT[`$INSTANCE_NAME`_RANGE_MAS_3];
        }
    }
    
    #else
    {
        if(voltage < `$INSTANCE_NAME`_voltRange[`$INSTANCE_NAME`_RANGE_MAS_0] )
        {
            voltage = voltage << (`$INSTANCE_NAME`_IN_NORMALIZATION -
                      `$INSTANCE_NAME`_XScaleVT[`$INSTANCE_NAME`_RANGE_MAS_0]);

            for (i=`$INSTANCE_NAME`_ORDER_VT - (1u); i > (0u); i--)
            {
                temp = `$INSTANCE_NAME`_MultShift24((`$INSTANCE_NAME`_coeffVT[i][`$INSTANCE_NAME`_RANGE_MAS_0] +
                       temp), voltage);
            }

            if(`$INSTANCE_NAME`_CoefScaleVT[`$INSTANCE_NAME`_RANGE_MAS_0] < `$INSTANCE_NAME`_INIT)
            {
                temp = (temp + `$INSTANCE_NAME`_coeffVT[`$INSTANCE_NAME`_FIRST_EL_MAS][`$INSTANCE_NAME`_RANGE_MAS_0]) <<
                       `$INSTANCE_NAME`_CoefScaleVT[`$INSTANCE_NAME`_RANGE_MAS_0];
            }
            
            else
            {
                temp = (temp + `$INSTANCE_NAME`_coeffVT[`$INSTANCE_NAME`_FIRST_EL_MAS][`$INSTANCE_NAME`_RANGE_MAS_0]) >>
                       `$INSTANCE_NAME`_CoefScaleVT[`$INSTANCE_NAME`_RANGE_MAS_0];
            }
        }

        else if(voltage <= `$INSTANCE_NAME`_voltRange[`$INSTANCE_NAME`_RANGE_MAS_1] )
        {
            voltage = voltage << (`$INSTANCE_NAME`_IN_NORMALIZATION -
                      `$INSTANCE_NAME`_XScaleVT[`$INSTANCE_NAME`_RANGE_MAS_1]);

            for (i=`$INSTANCE_NAME`_ORDER_VT - (1u); i > (0u); i--)
            {
                temp = `$INSTANCE_NAME`_MultShift24((`$INSTANCE_NAME`_coeffVT[i][`$INSTANCE_NAME`_RANGE_MAS_1] +
                       temp), voltage);
            }

            temp = (temp + `$INSTANCE_NAME`_coeffVT[`$INSTANCE_NAME`_FIRST_EL_MAS][`$INSTANCE_NAME`_RANGE_MAS_1]) >>
                   `$INSTANCE_NAME`_CoefScaleVT[`$INSTANCE_NAME`_RANGE_MAS_1];
        }

        else
        {
            voltage = voltage << (`$INSTANCE_NAME`_IN_NORMALIZATION -
                      `$INSTANCE_NAME`_XScaleVT[`$INSTANCE_NAME`_RANGE_MAS_2]);

            for (i=`$INSTANCE_NAME`_ORDER_VT - (1u); i > (0u); i--)
            {
                temp = `$INSTANCE_NAME`_MultShift24((`$INSTANCE_NAME`_coeffVT[i][`$INSTANCE_NAME`_RANGE_MAS_2] +
                       temp), voltage);
            }

            temp = (temp + `$INSTANCE_NAME`_coeffVT[`$INSTANCE_NAME`_FIRST_EL_MAS][`$INSTANCE_NAME`_RANGE_MAS_2]) >>
                   `$INSTANCE_NAME`_CoefScaleVT[`$INSTANCE_NAME`_RANGE_MAS_2];
        }
    }

    #endif /* End `$INSTANCE_NAME`_VT_RANGE_LEN == `$INSTANCE_NAME`_THREE */

    return (temp);
}
#endif /* End CY_PSOC3 */


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetVoltage
********************************************************************************
*
* Summary:
*  This function takes the temperature as input and provides the expected
*  voltage for that temperature.
*
* Parameters:
*  int32 temperature : Temperature of the cold junction in 1/100 degree C
*
* Return:
*  int32 : Expected voltage output of the thermocouple in microvolts
*
*******************************************************************************/
int32 `$INSTANCE_NAME`_GetVoltage(int32 temperature) `=ReentrantKeil($INSTANCE_NAME . "_GetVoltage")`

#if (CY_PSOC3)
{

    /* Variable to store calculated voltage */
    uint8 i;
    float voltage = `$INSTANCE_NAME`_INIT;
    float temperature_norm = `$INSTANCE_NAME`_INIT;

    temperature_norm  = (float)temperature;

    for (i = `$INSTANCE_NAME`_ORDER_TV - (1u); i > `$INSTANCE_NAME`_INIT; i--)
    {
        voltage = (`$INSTANCE_NAME`_coeffTV[i] + voltage) * temperature_norm;
    }
    voltage = (voltage + `$INSTANCE_NAME`_coeffTV[`$INSTANCE_NAME`_FIRST_EL_MAS]);

    return ((int32) (voltage));
}
#else
{
    /* Variable to store calculated voltage */
    uint8 i;
    int32 voltage = `$INSTANCE_NAME`_INIT;
    int32 scaletemp;

    scaletemp = temperature << (`$INSTANCE_NAME`_IN_NORMALIZATION - `$INSTANCE_NAME`_X_SCALE_TV);

    for (i = `$INSTANCE_NAME`_ORDER_TV - (1u); i > `$INSTANCE_NAME`_INIT; i--)
    {
        voltage = `$INSTANCE_NAME`_MultShift24((`$INSTANCE_NAME`_coeffTV[i] + voltage), scaletemp);
    }

    voltage = (voltage + `$INSTANCE_NAME`_coeffTV[`$INSTANCE_NAME`_FIRST_EL_MAS]) >> `$INSTANCE_NAME`_COEF_SCALE_TV ;

    return (voltage);
}
#endif /* End CY_PSOC3 */

/* [] END OF FILE */
