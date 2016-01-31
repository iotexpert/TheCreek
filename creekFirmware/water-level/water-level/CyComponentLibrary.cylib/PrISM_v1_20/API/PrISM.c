/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the source code to the API for the PrISM Component.
*
* Note:
*  The PRiSM Component consists of a 8, 16, 24, 32 - bit PrISM with
*  depended on length Polynomial value and user selected Seed Value. 
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"

uint8 `$INSTANCE_NAME`_initVar = 0;


/*******************************************************************************
* FUNCTION NAME:   `$INSTANCE_NAME`_Start
********************************************************************************
*
* Summary:
*  The start function sets Polynomial, Seed and Pulse Density registers provided
*  by customizer. PrISM computation starts on rising edge of input clock.
*
* Parameters:  
*  void
*
* Return:  
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_Start(void) 
{
    if (`$INSTANCE_NAME`_initVar == 0)
    {
       `$INSTANCE_NAME`_initVar = 0x01u; 
       /* Writes Seed value, polynom value and density provided by customizer */
       `$INSTANCE_NAME`_WriteSeed(`$INSTANCE_NAME`_SEED);
       `$INSTANCE_NAME`_WritePolynomial(`$INSTANCE_NAME`_POLYNOM);
       `$INSTANCE_NAME`_WritePulse0(`$INSTANCE_NAME`_DENSITY0);
       `$INSTANCE_NAME`_WritePulse1(`$INSTANCE_NAME`_DENSITY1);

        /* Set FIFO0_CLR bit to use FIFO0 as a simple one-byte buffer*/
        #if (`$INSTANCE_NAME`_RESOLUTION <= 8)      /* 8bit - PrISM */
            CY_SET_REG8(`$INSTANCE_NAME`_AUX_CONTROL_PTR, `$INSTANCE_NAME`_FIFO0_CLR);
        #elif (`$INSTANCE_NAME`_RESOLUTION <= 16)   /* 16bit - PrISM */
            CY_SET_REG16(`$INSTANCE_NAME`_AUX_CONTROL_PTR, `$INSTANCE_NAME`_FIFO0_CLR | 
                                                           `$INSTANCE_NAME`_FIFO0_CLR << 8);
        #elif (`$INSTANCE_NAME`_RESOLUTION <= 24)   /* 24bit - PrISM */
            CY_SET_REG24(`$INSTANCE_NAME`_AUX_CONTROL_PTR, `$INSTANCE_NAME`_FIFO0_CLR | 
                                                           `$INSTANCE_NAME`_FIFO0_CLR << 8 );
            CY_SET_REG24(`$INSTANCE_NAME`_AUX_CONTROL2_PTR, `$INSTANCE_NAME`_FIFO0_CLR );
        #else                                 /* 32bit - PrISM */
            CY_SET_REG32(`$INSTANCE_NAME`_AUX_CONTROL_PTR, `$INSTANCE_NAME`_FIFO0_CLR | 
                                                           `$INSTANCE_NAME`_FIFO0_CLR << 8 );
            CY_SET_REG32(`$INSTANCE_NAME`_AUX_CONTROL2_PTR, `$INSTANCE_NAME`_FIFO0_CLR | 
                                                            `$INSTANCE_NAME`_FIFO0_CLR << 8 );
        #endif                                /* End `$INSTANCE_NAME`_RESOLUTION */
        
        #if(`$INSTANCE_NAME`_PULSE_TYPE_HARDCODED == 0)
            /* Writes density type provided by customizer */
            if(`$INSTANCE_NAME`_GREATERTHAN_OR_EQUAL == `$CompareType0` )
            {
                `$INSTANCE_NAME`_CONTROL |= `$INSTANCE_NAME`_CTRL_COMPARE_TYPE0_GREATER_THAN_OR_EQUAL;
            }
            else
            {
                `$INSTANCE_NAME`_CONTROL &= ~`$INSTANCE_NAME`_CTRL_COMPARE_TYPE0_GREATER_THAN_OR_EQUAL;
            }
    
            if(`$INSTANCE_NAME`_GREATERTHAN_OR_EQUAL == `$CompareType1`)
            {
                `$INSTANCE_NAME`_CONTROL |= `$INSTANCE_NAME`_CTRL_COMPARE_TYPE1_GREATER_THAN_OR_EQUAL;
            }
            else
            {
                `$INSTANCE_NAME`_CONTROL &= ~`$INSTANCE_NAME`_CTRL_COMPARE_TYPE1_GREATER_THAN_OR_EQUAL;
            }
        #endif /* End `$INSTANCE_NAME`_PULSE_TYPE_HARDCODED */
    }    /* End `$INSTANCE_NAME`_initVar */
    #if(`$INSTANCE_NAME`_PULSE_TYPE_HARDCODED == 0)
        /* Enable the PrISM computation */
        `$INSTANCE_NAME`_CONTROL |= `$INSTANCE_NAME`_CTRL_ENABLE;
    #endif /* End `$INSTANCE_NAME`_PULSE_TYPE_HARDCODED */
}


/*******************************************************************************
* FUNCTION NAME:   `$INSTANCE_NAME`_Stop
********************************************************************************
*
* Summary:
*  Stops PrISM computation. Outputs remain constant.
*
* Parameters:  
*  void
*
* Return: 
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void)
{
    #if(`$INSTANCE_NAME`_PULSE_TYPE_HARDCODED == 0)
        `$INSTANCE_NAME`_CONTROL &= ~`$INSTANCE_NAME`_CTRL_ENABLE;
    #else
        /* PulseTypeHardoded option enabled - to stop PrISM use enable input */
    #endif /* End $INSTANCE_NAME`_PULSE_TYPE_HARDCODED */
}

#if(`$INSTANCE_NAME`_PULSE_TYPE_HARDCODED == 0)


    /***************************************************************************
    * FUNCTION NAME:     `$INSTANCE_NAME`_SetPulse0Mode
    ****************************************************************************
    *
    * Summary:
    *  Sets the pulse density type for Density0. Less than or Equal(<=) or 
    *  Greater that or Equal(>=).
    *
    * Parameters:
    *  pulse0Type: Selected pulse density type.
    *
    * Return:
    *  void
    * 
    ***************************************************************************/
    void `$INSTANCE_NAME`_SetPulse0Mode(uint8 pulse0Type)
    {
        if(pulse0Type == `$INSTANCE_NAME`_GREATERTHAN_OR_EQUAL)
        {
            `$INSTANCE_NAME`_CONTROL |= `$INSTANCE_NAME`_CTRL_COMPARE_TYPE0_GREATER_THAN_OR_EQUAL;
        }
        else
        {
            `$INSTANCE_NAME`_CONTROL &= ~`$INSTANCE_NAME`_CTRL_COMPARE_TYPE0_GREATER_THAN_OR_EQUAL;
        }
    }
    
    
    /***************************************************************************
    * FUNCTION NAME:   `$INSTANCE_NAME`_SetPulse1Mode
    ****************************************************************************
    *
    * Summary:
    *  Sets the pulse density type for Density1. Less than or Equal(<=) or 
    *  Greater that or Equal(>=).
    *
    * Parameters:  
    *  pulse1Type: Selected pulse density type.
    *
    * Return:
    *  void
    * 
    ***************************************************************************/
    void `$INSTANCE_NAME`_SetPulse1Mode(uint8 pulse1Type)
    {
        if(pulse1Type == `$INSTANCE_NAME`_GREATERTHAN_OR_EQUAL)
        {
            `$INSTANCE_NAME`_CONTROL |= `$INSTANCE_NAME`_CTRL_COMPARE_TYPE1_GREATER_THAN_OR_EQUAL;
        }
        else
        {
            `$INSTANCE_NAME`_CONTROL &= ~`$INSTANCE_NAME`_CTRL_COMPARE_TYPE1_GREATER_THAN_OR_EQUAL;
        }
    }

#endif /* End `$INSTANCE_NAME`_PULSE_TYPE_HARDCODED == 0 */


/*******************************************************************************
* FUNCTION NAME:   `$INSTANCE_NAME`_ReadSeed
********************************************************************************
*
* Summary:
*  Reads the PrISM Seed register.
*
* Parameters:
*  void
*
* Return:
*  Current Period register value.
*
*******************************************************************************/
`$RegSizeReplacementString` `$INSTANCE_NAME`_ReadSeed(void)
{
    return( `$CyGetRegReplacementString`(`$INSTANCE_NAME`_SEED_PTR) );
}


/*******************************************************************************
* FUNCTION NAME:   `$INSTANCE_NAME`_WriteSeed
********************************************************************************
*
* Summary:
*  Writes the PrISM Seed register with the start value.
*
* Parameters:
*  seed: Seed register value.
*
* Return:
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteSeed(`$RegSizeReplacementString` seed)
{
    if(seed != 0)
    {
        `$CySetRegReplacementString`(`$INSTANCE_NAME`_SEED_COPY_PTR, seed);
        `$CySetRegReplacementString`(`$INSTANCE_NAME`_SEED_PTR, seed);
    }
}


/*******************************************************************************
* FUNCTION NAME:   `$INSTANCE_NAME`_ReadPolynomial
********************************************************************************
*
* Summary:
*  Reads the PrISM polynomial.
*
* Parameters:
*  void
*
* Return:
*  PrISM polynomial.
*
*******************************************************************************/
`$RegSizeReplacementString` `$INSTANCE_NAME`_ReadPolynomial(void)
{
    return( `$CyGetRegReplacementString`(`$INSTANCE_NAME`_POLYNOM_PTR) );
}


/*******************************************************************************
* FUNCTION NAME:   `$INSTANCE_NAME`_WritePolynomial
********************************************************************************
*
* Summary:
*  Writes the PrISM polynomial.
*
* Parameters:
*  polynomial: PrISM polynomial.
*
* Return:
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_WritePolynomial(`$RegSizeReplacementString` polynomial)
{
    `$CySetRegReplacementString`(`$INSTANCE_NAME`_POLYNOM_PTR, polynomial);
    
}


/*******************************************************************************
* FUNCTION NAME:   `$INSTANCE_NAME`_ReadPusle0
********************************************************************************
*
* Summary:
*  Reads the PrISM Pulse Density0 register.
*
* Parameters:
*  void
*
* Return:
*  Pulse Density0 register value.
*
*******************************************************************************/
`$RegSizeReplacementString` `$INSTANCE_NAME`_ReadPusle0(void)
{
    return( `$CyGetRegReplacementString`(`$INSTANCE_NAME`_DENSITY0_PTR) );
}


/*******************************************************************************
* FUNCTION NAME:   `$INSTANCE_NAME`_WritePulse0
********************************************************************************
*
* Summary:
*  Writes the PrISM Pulse Density0 register with the Pulse Density value.
*
* Parameters:
*  pulseDesity0: Pulse Density value.
*
* Return:
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_WritePulse0(`$RegSizeReplacementString` pulseDesity0)
{
    `$CySetRegReplacementString`(`$INSTANCE_NAME`_DENSITY0_PTR, pulseDesity0);
}


/*******************************************************************************
* FUNCTION NAME:   `$INSTANCE_NAME`_ReadPusle1
********************************************************************************
*
* Summary:
*  Reads the PrISM Pulse Density1 register.
*
* Parameters:
*  void
*
* Return:
*  Pulse Density1 register value.
*
*******************************************************************************/
`$RegSizeReplacementString` `$INSTANCE_NAME`_ReadPusle1(void)
{
    return( `$CyGetRegReplacementString`(`$INSTANCE_NAME`_DENSITY1_PTR) );
}


/*******************************************************************************
* FUNCTION NAME:   `$INSTANCE_NAME`_WritePulse1
********************************************************************************
*
* Summary:
*  Writes the PrISM Pulse Density1 register with the Pulse Density value.
*
* Parameters:
*  pulseDesity1: Pulse Density value.
*
* Return:
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_WritePulse1(`$RegSizeReplacementString` pulseDesity1)
{
    `$CySetRegReplacementString`(`$INSTANCE_NAME`_DENSITY1_PTR, pulseDesity1);
}


/* [] END OF FILE */
