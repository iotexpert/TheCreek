/*******************************************************************************
* File Name: therm.c
* Version 1.20
*
* Description:
*  This file provides the source code to the API for the ThermistorCalc
*  Component.
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

#include "therm.h"


/*******************************************************************************
* Function Name: therm_GetResistance
********************************************************************************
*
* Summary:
*  The digital values of the voltages across the reference resistor and the
*  thermistor are passed to this function as parameters. The function returns
*  the resistance, based on the voltage values.
*
* Parameters:
*  vReference: the voltage across the reference resistor;
*  vThermistor: the voltage across the thermistor.
*  The ratio of these two voltages is used by this function. Therefore, the
*  units for both parameters must be the same.
*
* Return:
*  The return value is the resistance across the thermistor. The value returned
*  is the resistance in Ohms.
*
*******************************************************************************/
uint32 therm_GetResistance(int16 vReference, int16 vThermistor)
                                      
{
    int32 resT;
    int16 temp;

    /* Calculate thermistor resistance from the voltages */
    resT = therm_REF_RESISTOR * ((int32)vThermistor);
    if (vReference < 0)
    {
        temp = -vReference;
        temp = (int16)((uint16)((uint16)temp >> 1u));
        temp = -temp;
    }
    else
    {
        temp = (int16)((uint16)((uint16)vReference >> 1u));
    }
    resT += temp;
    resT /= vReference;

    /* The ordering of Reference resistor value is specifically designed to keep the result from overflowing */
    return ((uint32)((uint32)resT << therm_REF_RES_SHIFT));
}


/*******************************************************************************
* Function Name: therm_GetTemperature
********************************************************************************
*
* Summary:
*  The value of the thermistor resistance is passed to this function as a
*  parameter. The function returns the temperature, based on the resistance
*  value. The method used to calculate the temperature is dependent on whether
*  Equation or LUT was selected in the Customizer.
*
* Parameters:
*  resT: the resistance across the thermistor in Ohms.
*
* Return:
*  The return value is the temperature in 1/100ths of degrees C. For example,
*  the return value is 2345, when the actual temperature is 23.45 degrees C.
*
*******************************************************************************/
int16 therm_GetTemperature(uint32 resT) 
{
    int16 tempTR;
    static const uint32 CYCODE therm_LUT[] = { 32747u, 32580u, 32413u, 32248u, 32083u, 31920u, 31757u, 31596u, 31435u,
 31275u, 31116u, 30958u, 30801u, 30645u, 30490u, 30335u, 30182u, 30029u, 29877u, 29726u, 29576u, 29427u, 29278u, 29131u,
 28984u, 28838u, 28693u, 28549u, 28406u, 28263u, 28121u, 27980u, 27840u, 27701u, 27562u, 27424u, 27287u, 27151u, 27015u,
 26881u, 26746u, 26613u, 26481u, 26349u, 26218u, 26088u, 25958u, 25829u, 25701u, 25574u, 25447u, 25321u, 25196u, 25071u,
 24947u, 24824u, 24701u, 24580u, 24458u, 24338u, 24218u, 24099u, 23981u, 23863u, 23745u, 23629u, 23513u, 23398u, 23283u,
 23169u, 23056u, 22943u, 22831u, 22720u, 22609u, 22498u, 22389u, 22280u, 22171u, 22063u, 21956u, 21849u, 21743u, 21638u,
 21533u, 21428u, 21325u, 21221u, 21119u, 21017u, 20915u, 20814u, 20714u, 20614u, 20514u, 20416u, 20317u, 20220u, 20122u,
 20026u, 19929u, 19834u, 19739u, 19644u, 19550u, 19456u, 19363u, 19271u, 19179u, 19087u, 18996u, 18905u, 18815u, 18726u,
 18636u, 18548u, 18460u, 18372u, 18285u, 18198u, 18112u, 18026u, 17940u, 17855u, 17771u, 17687u, 17603u, 17520u, 17437u,
 17355u, 17273u, 17192u, 17111u, 17031u, 16950u, 16871u, 16792u, 16713u, 16634u, 16556u, 16479u, 16402u, 16325u, 16249u,
 16173u, 16097u, 16022u, 15947u, 15873u, 15799u, 15725u, 15652u, 15579u, 15507u, 15435u, 15363u, 15292u, 15221u, 15151u,
 15080u, 15011u, 14941u, 14872u, 14803u, 14735u, 14667u, 14599u, 14532u, 14465u, 14399u, 14332u, 14267u, 14201u, 14136u,
 14071u, 14006u, 13942u, 13878u, 13815u, 13752u, 13689u, 13626u, 13564u, 13502u, 13440u, 13379u, 13318u, 13258u, 13197u,
 13137u, 13078u, 13018u, 12959u, 12900u, 12842u, 12784u, 12726u, 12668u, 12611u, 12554u, 12497u, 12441u, 12385u, 12329u,
 12273u, 12218u, 12163u, 12108u, 12054u, 12000u, 11946u, 11892u, 11839u, 11786u, 11733u, 11681u, 11628u, 11576u, 11525u,
 11473u, 11422u, 11371u, 11320u, 11270u, 11220u, 11170u, 11120u, 11071u, 11021u, 10973u, 10924u, 10875u, 10827u, 10779u,
 10732u, 10684u, 10637u, 10590u, 10543u, 10497u, 10450u, 10404u, 10358u, 10313u, 10267u, 10222u, 10177u, 10133u, 10088u,
 10044u, 10000u, 9956u, 9913u, 9869u, 9826u, 9783u, 9740u, 9698u, 9656u, 9613u, 9572u, 9530u, 9488u, 9447u, 9406u,
 9365u, 9325u, 9284u, 9244u, 9204u, 9164u, 9124u, 9085u, 9045u, 9006u, 8967u, 8929u, 8890u, 8852u, 8814u, 8776u, 8738u,
 8700u, 8663u, 8626u, 8589u, 8552u, 8515u, 8479u, 8443u, 8406u, 8370u, 8335u, 8299u, 8264u, 8228u, 8193u, 8158u, 8124u,
 8089u, 8054u, 8020u, 7986u, 7952u, 7918u, 7885u, 7851u, 7818u, 7785u, 7752u, 7719u, 7687u, 7654u, 7622u, 7590u, 7558u,
 7526u, 7494u, 7463u, 7431u, 7400u, 7369u, 7338u, 7307u, 7276u, 7246u, 7215u, 7185u, 7155u, 7125u, 7095u, 7066u, 7036u,
 7007u, 6978u, 6949u, 6920u, 6891u, 6862u, 6834u, 6805u, 6777u, 6749u, 6721u, 6693u, 6665u, 6638u, 6610u, 6583u, 6556u,
 6528u, 6501u, 6475u, 6448u, 6421u, 6395u, 6369u, 6342u, 6316u, 6290u, 6264u, 6239u, 6213u, 6188u, 6162u, 6137u, 6112u,
 6087u, 6062u, 6037u, 6013u, 5988u, 5964u, 5939u, 5915u, 5891u, 5867u, 5843u, 5819u, 5796u, 5772u, 5749u, 5725u, 5702u,
 5679u, 5656u, 5633u, 5610u, 5588u, 5565u, 5543u, 5520u, 5498u, 5476u, 5454u, 5432u, 5410u, 5388u, 5367u, 5345u, 5324u,
 5302u, 5281u, 5260u, 5239u, 5218u, 5197u, 5176u, 5155u, 5135u, 5114u, 5094u, 5074u, 5053u, 5033u, 5013u, 4993u, 4973u,
 4954u, 4934u, 4914u, 4895u, 4875u, 4856u, 4837u, 4818u, 4799u, 4780u, 4761u, 4742u, 4723u, 4705u, 4686u, 4668u, 4649u,
 4631u, 4613u, 4595u, 4577u, 4559u, 4541u, 4523u, 4505u, 4488u, 4470u, 4452u, 4435u, 4418u, 4400u, 4383u, 4366u, 4349u,
 4332u, 4315u, 4299u, 4282u, 4265u, 4249u, 4232u, 4216u, 4199u, 4183u, 4167u, 4151u, 4135u, 4119u, 4103u, 4087u, 4071u,
 4055u, 4040u, 4024u, 4009u, 3993u, 3978u, 3962u, 3947u, 3932u, 3917u, 3902u, 3887u, 3872u, 3857u, 3842u, 3828u, 3813u,
 3799u, 3784u, 3770u, 3755u, 3741u, 3727u, 3712u, 3698u, 3684u, 3670u, 3656u, 3642u, 3628u, 3615u, 3601u };


    #if (therm_IMPLEMENTATION == therm_EQUATION_METHOD)

        float32 stEqn;
        float32 logrT;

        /* Calculate thermistor resistance from the voltages */
        #if(!CY_PSOC3)
            logrT = (float32)log((float64)resT);
        #else
            logrT = log((float32)resT);
        #endif  /* (!CY_PSOC3) */

        /* Calculate temperature from the resistance of thermistor using Steinhart-Hart Equation */
        #if(!CY_PSOC3)
            stEqn = (float32)(therm_THA + (therm_THB * logrT) + 
                             (therm_THC * pow((float64)logrT, (float32)3)));
        #else
            stEqn = (float32)(therm_THA + (therm_THB * logrT) + 
                             (therm_THC * pow(logrT, (float32)3)));
        #endif  /* (!CY_PSOC3) */

        tempTR = (int16)((float32)((((1.0 / stEqn) - therm_K2C) * (float32)therm_SCALE) + 0.5));

    #else /* therm_IMPLEMENTATION == therm_LUT_METHOD */

        uint16 mid;
        uint16 first = 0u;
        uint16 last = therm_LUT_SIZE - 1u;

        /* Calculate temperature from the resistance of thermistor by using the LUT */
        while (first < last)
        {
            mid = (first + last) >> 1u;

            if ((0u == mid) || ((therm_LUT_SIZE - 1u) == mid) || (resT == therm_LUT[mid]))
            {
                last = mid;
                break;
            }
            else if (therm_LUT[mid] > resT)
            {
                first = mid + 1u;
            }
            else /* (therm_LUT[mid] < resT) */
            {
                last = mid - 1u;
            }
        }

        /* Calculation the closest entry in the LUT */
        if ((therm_LUT[last] > resT) && (last < (therm_LUT_SIZE - 1u)) &&
           ((therm_LUT[last] - resT) > (resT - therm_LUT[last + 1u])))
        {
            last++;
        }
        else if ((therm_LUT[last] < resT) && (last > 0u) &&
                ((therm_LUT[last - 1u] - resT) < (resT - therm_LUT[last])))
        {
            last--;
        }
        else
        {
            /* Closest entry in the LUT already found */
        }

        tempTR = therm_MIN_TEMP + (int16)((uint16)(last * therm_ACCURACY));

    #endif /* (therm_IMPLEMENTATION == therm_EQUATION_METHOD) */

    return (tempTR);
}


/* [] END OF FILE */
