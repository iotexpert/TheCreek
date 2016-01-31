/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the source code to the API for the ThermistorCalc 
*  Component.
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


#if (`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`__EQUATION)
    #include <math.h>
#endif /* (`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`__EQUATION) */


`$lookUpTableString`


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetResistance
********************************************************************************
*
* Summary:
*  The digital values of the voltages across the reference resistor and the 
*  thermistor are passed to this function as parameters. The function returns 
*  the resistance, based on the voltage values.
*
* Parameters:
*  Vreference: the voltage across the reference resistor;
*  Vthermistor: the voltage across the thermistor. 
*  The ratio of these two voltages is used by this function. Therefore, the 
*  units for both parameters must be the same.
*
* Return:
*  The return value is the resistance across the thermistor. The value returned
*  is the resistance in Ohms.
*
*******************************************************************************/
uint32 `$INSTANCE_NAME`_GetResistance(int16 Vreference, int16 VThermistor) 
                                      `=ReentrantKeil($INSTANCE_NAME . "_GetResistance")`
{
    int32 resT;
    int16 temp;
    
	/* Calculate thermistor resistance from the voltages */	
	resT = `$INSTANCE_NAME`_REF_RESISTOR * ((int32)VThermistor);
    if (Vreference < 0u)
    {
        temp = -Vreference;
        temp >>= 1u;
        temp = -temp;
    }
    else
    {
        temp = Vreference >> 1u;
    }
    resT += temp;
	resT /= Vreference;
    
    if (resT < 0u)
    {
        /* Halt CPU in debug mode if resT is out of valid range */
        CYASSERT(0u);
    }
    
    /* The ordering of Reference resistor value is specifically designed to keep the result from overflowing */
    resT <<= `$INSTANCE_NAME`_REF_RES_SHIFT;

    return ((uint32)resT);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetTemperature
********************************************************************************
*
* Summary:
*  The value of the thermistor resistance is passed to this function as a 
*  parameter. The function returns the temperature, based on the resistance 
*  value. The method used to calculate the temperature is dependent on whether 
*  Equation or LUT was selected in the Customizer.
*
* Parameters:
*  ResT: the resistance across the thermistor in Ohms.
*
* Return:
*  The return value is the temperature in 1/100ths of degrees C. For example,
*  the return value is 2345, when the actual temperature is 23.45 degrees C.
*
*******************************************************************************/
int16 `$INSTANCE_NAME`_GetTemperature(uint32 ResT) `=ReentrantKeil($INSTANCE_NAME . "_GetTemperature")`
{
    int16 temp_TR;
    
    #if (`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`__EQUATION)
        
        float st_Eqn;
        float logrT;
        
        /* Calculate thermistor resistance from the voltages */
		logrT = log(ResT);

		/* Calculate temperature from the resistance of thermistor using Steinhart-Hart Equation */
		st_Eqn = `$INSTANCE_NAME`_THA + `$INSTANCE_NAME`_THB * logrT + `$INSTANCE_NAME`_THC * pow(logrT, 3u);
		temp_TR = (int16)((((1u / st_Eqn) - `$INSTANCE_NAME`_K2C) * `$INSTANCE_NAME`_SCALE) + 0.5);
    
    #else /* `$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`__LUT */
            
        uint16 mid;
        uint16 first = 0u;        
        uint16 last = `$INSTANCE_NAME`_LUT_SIZE - 1u;
        
        /* Calculate temperature from the resistance of thermistor by using the LUT */        
        while (first < last)
        {
            mid = (first + last) >> 1u;
            
            if ((mid == 0u) || (mid == (`$INSTANCE_NAME`_LUT_SIZE - 1u)) || (`$INSTANCE_NAME`_LUT[mid] == ResT))
            {
                last = mid;
                break;
            }
            else if (`$INSTANCE_NAME`_LUT[mid] > ResT)
            {
                first = mid + 1u;
            }
            else /* (`$INSTANCE_NAME`_LUT[mid] < ResT) */
            {
                last = mid - 1u;
            }            
        }
        
        /* Calculation the closest entry in the LUT */       
        if ((`$INSTANCE_NAME`_LUT[last] > ResT) && (last < (`$INSTANCE_NAME`_LUT_SIZE - 1u)) &&
           ((`$INSTANCE_NAME`_LUT[last] - ResT) > (ResT - `$INSTANCE_NAME`_LUT[last + 1u])))            
        {
            last++;
        }
        else if ((`$INSTANCE_NAME`_LUT[last] < ResT) && (last > 0u) &&
                ((`$INSTANCE_NAME`_LUT[last - 1u] - ResT) < (ResT - `$INSTANCE_NAME`_LUT[last])))
        {
            last--;
        }
        
        temp_TR = `$INSTANCE_NAME`_MIN_TEMP + (int16)(last * `$INSTANCE_NAME`_ACCURACY);     

    #endif /* (`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`__EQUATION) */    

    return (temp_TR);
}


/* [] END OF FILE */
