/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the source code to the High Level API for the CapSesne 
*  Component.
*
* Note:
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#include "`$INSTANCE_NAME`_CSHL.h"

`$writerCHLVariables`
/*******************************************************************************
* Function Name:   uint16 `$INSTANCE_NAME`_CSHL_JitterFilter(uint16 x1, uint16 x2)
********************************************************************************
*
* Summary:
*  Jitter filter funciton.
*
* Parameters:
*  x1:  Current value
*  x2:  Previous value
* 
* Return:
*  Returns filtered value.
*
* Theory: 
*  See summary
*
* Side Effects:
*  None
*
**********************************************************************************/
uint16 `$INSTANCE_NAME`_CSHL_JitterFilter(uint16 x1, uint16 x2)
{
    if (x1 > x2)
    {    
        x1--;
    }
    else
    {
        if (x1 < x2)
        {
            x1++;
        }
    }

    return x1;
}


/*******************************************************************************
* Function Name:   uint16 `$INSTANCE_NAME`_CSHL_MedianFilter(uint16 x1, uint16 x2, uint16 x3)
********************************************************************************
*
* Summary:
*  Median filter funciton.
*
* Parameters:
*  x1:  Current value
*  x2:  Previous value
*  x3:  Before previous value
*
* Return:
*  Returns filtered value.
*
* Theory: 
*  See summary
*
* Side Effects:
*  None
*
**********************************************************************************/
uint16 `$INSTANCE_NAME`_CSHL_MedianFilter(uint16 x1, uint16 x2, uint16 x3)
{
    uint16 tmp;

    if (x1 > x2) 
    {
        tmp = x2; 
        x2 = x1;
        x1 = tmp;
    }
    
    if(x2 > x3)
    {
        x2 = x3;
    }

    return ((x1 > x2) ? x1 : x2);
}


/*******************************************************************************
* Function Name:   uint16 `$INSTANCE_NAME`_CSHL_MedianFilter(uint16 x1, uint16 x2, uint16 x3)
********************************************************************************
*
* Summary:
*  Averaging filter function.
*
* Parameters:
*  x1:  Current value
*  x2:  Previous value
*  x3:  Before previous value
*
* Return:
*  Returns filtered value.
*
* Theory: 
*  See summary
*
* Side Effects:
*  None
*
**********************************************************************************/
uint16 `$INSTANCE_NAME`_CSHL_AveragingFilter(uint16 x1, uint16 x2, uint16 x3)
{
    uint32 tmp = (x1+x2+x3)/3;
    
    return ((uint16) tmp);
}


/*******************************************************************************
* Function Name:   uint16 `$INSTANCE_NAME`_CSHL_IIRFilter(uint16 x1, uint16 x2, uint8 type)
********************************************************************************
*
* Summary:
*  IIR filter function.
*
* Parameters:
*  x1:  Current value
*  x2:  Previous value
*  type:  Type formula of IIR filter.
*
* Return:
*  Returns filtered value.
*
* Theory: 
*  See summary
*
* Side Effects:
*  None
*
**********************************************************************************/
uint16 `$INSTANCE_NAME`_CSHL_IIRFilter(uint16 x1, uint16 x2, uint8 type)
{
    uint32 tmp;
    
    if (type == `$INSTANCE_NAME`_CSHL_IIR_FILTER_0)
    {
        /* IIR = 1/2 Current Value+ 1/2 Previous Value */
        tmp = x1 + x2;
        tmp >>= 1;
    }
    else
    {
        /* IIR = 1/4 Current Value + 3/4 Previous Value */
        tmp = x1 + x2;
        tmp += (x2 << 1);
        tmp >>= 2;
    }
       
    return ((uint16) tmp);
}

`$writerCHLFunctions`
`$writerCHLFunctionsCentroid`

/* [] END OF FILE */
