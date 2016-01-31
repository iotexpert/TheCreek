/*******************************************************************************
* File Name: `$INSTANCE_NAME`_CSHL.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*     Provides the function definitions for a High Level API of CapSense component.
*
*   Note:
*     None
*
*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#include "`$INSTANCE_NAME`_CSHL.h"

`$writerCHLVariables`

const uint8 `$INSTANCE_NAME`_CSHL_RotarySliderAngle[9] = {180, 106, 56, 29, 14, 7, 4, 2, 1};

const uint8 `$INSTANCE_NAME`_CSHL_RotarySliderAngleSin[46]	=	{
		   4,       13,       22,       31,       40,
		  49,       58,       66,       75,       83,
		  92,      100,      108,      116,      124, 
		 132,      139,      147,      154,      161, 
		 168,      175,      181,      187,      193, 
		 199,      204,      210,      215,      219, 
		 224,      228,      232,      236,      239, 
		 242,      245,      247,      249,      251, 
		 253,      254,      255,      255,      255, 255 
		};  /* Array of SIN Table */


 /*-----------------------------------------------------------------------------
 * FUNCTION NAME: uint16 `$INSTANCE_NAME`_CSHL_JitterFilter(uint16 x1, uint16 x2)
 *-----------------------------------------------------------------------------
 * Summary: 
 *  Jitter filter funciton.
 *
 * Parameters: 
 *  x1: uint16 – first argument of jitter filter
  *  x2: uint16 – second argument of jitter filter
 * 
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
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

 /*-----------------------------------------------------------------------------
 * FUNCTION NAME: uint16 `$INSTANCE_NAME`_CSHL_MedianFilter(uint16 x1, uint16 x2, uint16 x3)
 *-----------------------------------------------------------------------------
 * Summary: 
 *  Median filter function.
 *
 * Parameters: 
 *  x1: uint16 – first argument of median filter
 *  x2: uint16 – second argument of median filter
 *  x3: uint16 – third argument of median filter
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
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

 /*-----------------------------------------------------------------------------
 * FUNCTION NAME: uint16 `$INSTANCE_NAME`_CSHL__CSHL_RawAveraging(uin16 x1, uint16 x2, uint16 x3)
 *-----------------------------------------------------------------------------
 * Summary: 
 * Averaging filter function.
 *
 * Parameters: 
 *  x1: uint16 – first argument of median filter
 *  x2: uint16 – second argument of median filter
 *  x3: uint16 – third argument of median filter
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
uint16 `$INSTANCE_NAME`_CSHL_AveragingFilter(uint16 x1, uint16 x2, uint16 x3)
{
	uint32 temp = (x1+x2+x3)/3;
	
	return ((uint16) temp);
}

`$writerCHLFunctions`
`$writerCHLFunctionsCentroid`

 /* [] END OF FILE */