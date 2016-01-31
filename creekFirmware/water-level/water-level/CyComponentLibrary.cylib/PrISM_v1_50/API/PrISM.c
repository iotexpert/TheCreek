/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the source code of the API for the PrISM Component.
*
* Note:
*  The PRiSM Component consists of a 8, 16, 24, 32 - bit PrISM, it
*  depends on length Polynomial value and user selected Seed Value. 
*
********************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"


/***************************************
* Local data allocation
***************************************/

uint8 `$INSTANCE_NAME`_initVar = 0u;


/*******************************************************************************
* FUNCTION NAME:   `$INSTANCE_NAME`_Start
********************************************************************************
*
* Summary:
*  The start function sets Polynomial, Seed and Pulse Density registers provided
*  by customizer. PrISM computation starts on rising edge of input clock.
*
* Parameters:  
*  None.
*
* Return:  
*  None.
*
* Global variables:
*  The `$INSTANCE_NAME`_initVar variable is used to indicate initial
*  configuration of this component.  The variable is initialized to zero and
*  set to 1 the first time `$INSTANCE_NAME`_Start() is called. This allows for
*  component initialization without re-initialization in all subsequent calls
*  to the `$INSTANCE_NAME`_Start() routine. 

* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Start(void) 
{
    /* If not Initialized then initialize all required hardware and software */
    if(`$INSTANCE_NAME`_initVar == 0u)
    {
        `$INSTANCE_NAME`_Init();
        `$INSTANCE_NAME`_initVar = 1u;
    }
    `$INSTANCE_NAME`_Enable();

}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Init
********************************************************************************
*
* Summary:
*  Initialize component's parameters to the parameters set by user in the 
*  customizer of the component placed onto schematic. Usually called in 
*  `$INSTANCE_NAME`_Start().
*
* Parameters:  
*  None.
*
* Return: 
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_Init")`
{
    uint8 enableInterrupts;
    
    /* Writes Seed value, polynom value and density provided by customizer */
    `$INSTANCE_NAME`_WriteSeed(`$INSTANCE_NAME`_SEED);
    `$INSTANCE_NAME`_WritePolynomial(`$INSTANCE_NAME`_POLYNOM);
    `$INSTANCE_NAME`_WritePulse0(`$INSTANCE_NAME`_DENSITY0);
    `$INSTANCE_NAME`_WritePulse1(`$INSTANCE_NAME`_DENSITY1);
    
    enableInterrupts = CyEnterCriticalSection();
    /* Set FIFO0_CLR bit to use FIFO0 as a simple one-byte buffer*/
    #if (`$INSTANCE_NAME`_RESOLUTION <= 8u)      /* 8bit - PrISM */
        `$INSTANCE_NAME`_AUX_CONTROL_REG |= `$INSTANCE_NAME`_FIFO0_CLR;
    #elif (`$INSTANCE_NAME`_RESOLUTION <= 16u)   /* 16bit - PrISM */
        CY_SET_REG16(`$INSTANCE_NAME`_AUX_CONTROL_PTR, CY_GET_REG16(`$INSTANCE_NAME`_AUX_CONTROL_PTR) | 
                                        `$INSTANCE_NAME`_FIFO0_CLR | `$INSTANCE_NAME`_FIFO0_CLR << 8u);
    #elif (`$INSTANCE_NAME`_RESOLUTION <= 24u)   /* 24bit - PrISM */
        CY_SET_REG24(`$INSTANCE_NAME`_AUX_CONTROL_PTR, CY_GET_REG24(`$INSTANCE_NAME`_AUX_CONTROL_PTR) |
                                        `$INSTANCE_NAME`_FIFO0_CLR | `$INSTANCE_NAME`_FIFO0_CLR << 8u );
        CY_SET_REG24(`$INSTANCE_NAME`_AUX_CONTROL2_PTR, CY_GET_REG24(`$INSTANCE_NAME`_AUX_CONTROL2_PTR) | 
                                        `$INSTANCE_NAME`_FIFO0_CLR );
    #else                                 /* 32bit - PrISM */
        CY_SET_REG32(`$INSTANCE_NAME`_AUX_CONTROL_PTR, CY_GET_REG32(`$INSTANCE_NAME`_AUX_CONTROL_PTR) |
                                        `$INSTANCE_NAME`_FIFO0_CLR | `$INSTANCE_NAME`_FIFO0_CLR << 8u );
        CY_SET_REG32(`$INSTANCE_NAME`_AUX_CONTROL2_PTR, CY_GET_REG32(`$INSTANCE_NAME`_AUX_CONTROL2_PTR) |
                                        `$INSTANCE_NAME`_FIFO0_CLR | `$INSTANCE_NAME`_FIFO0_CLR << 8u );
    #endif                                /* End `$INSTANCE_NAME`_RESOLUTION */
    CyExitCriticalSection(enableInterrupts);
    
    #if(!`$INSTANCE_NAME`_PULSE_TYPE_HARDCODED)
        /* Writes density type provided by customizer */
        if(`$INSTANCE_NAME`_GREATERTHAN_OR_EQUAL == `$CompareType0` )
        {
            `$INSTANCE_NAME`_CONTROL_REG |= `$INSTANCE_NAME`_CTRL_COMPARE_TYPE0_GREATER_THAN_OR_EQUAL;
        }
        else
        {
            `$INSTANCE_NAME`_CONTROL_REG &= ~`$INSTANCE_NAME`_CTRL_COMPARE_TYPE0_GREATER_THAN_OR_EQUAL;
        }
    
        if(`$INSTANCE_NAME`_GREATERTHAN_OR_EQUAL == `$CompareType1`)
        {
            `$INSTANCE_NAME`_CONTROL_REG |= `$INSTANCE_NAME`_CTRL_COMPARE_TYPE1_GREATER_THAN_OR_EQUAL;
        }
        else
        {
            `$INSTANCE_NAME`_CONTROL_REG &= ~`$INSTANCE_NAME`_CTRL_COMPARE_TYPE1_GREATER_THAN_OR_EQUAL;
        }
    #endif /* End `$INSTANCE_NAME`_PULSE_TYPE_HARDCODED */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Enable
********************************************************************************
*  
* Summary: 
*  Enables the PrISM block operation
*
* Parameters:  
*  None.
*
* Return: 
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`
{
    #if(!`$INSTANCE_NAME`_PULSE_TYPE_HARDCODED)
        /* Enable the PrISM computation */
        `$INSTANCE_NAME`_CONTROL_REG |= `$INSTANCE_NAME`_CTRL_ENABLE;
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
*  None.
*
* Return: 
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`
{
    #if(!`$INSTANCE_NAME`_PULSE_TYPE_HARDCODED)
        `$INSTANCE_NAME`_CONTROL_REG &= ~`$INSTANCE_NAME`_CTRL_ENABLE;
    #else
        /* PulseTypeHardoded option enabled - to stop PrISM use enable input */
    #endif /* End $INSTANCE_NAME`_PULSE_TYPE_HARDCODED */
}

#if(!`$INSTANCE_NAME`_PULSE_TYPE_HARDCODED)


    /***************************************************************************
    * FUNCTION NAME:     `$INSTANCE_NAME`_SetPulse0Mode
    ****************************************************************************
    *
    * Summary:
    *  Sets the pulse density type for Density0. Less than or Equal(<=) or 
    *  Greater than or Equal(>=).
    *
    * Parameters:
    *  pulse0Type: Selected pulse density type.
    *
    * Return:
    *  None.
    *
    ***************************************************************************/
    void `$INSTANCE_NAME`_SetPulse0Mode(uint8 pulse0Type) `=ReentrantKeil($INSTANCE_NAME . "_SetPulse0Mode")`
    {
        if(pulse0Type == `$INSTANCE_NAME`_GREATERTHAN_OR_EQUAL)
        {
            `$INSTANCE_NAME`_CONTROL_REG |= `$INSTANCE_NAME`_CTRL_COMPARE_TYPE0_GREATER_THAN_OR_EQUAL;
        }
        else
        {
            `$INSTANCE_NAME`_CONTROL_REG &= ~`$INSTANCE_NAME`_CTRL_COMPARE_TYPE0_GREATER_THAN_OR_EQUAL;
        }
    }
    
    
    /***************************************************************************
    * FUNCTION NAME:   `$INSTANCE_NAME`_SetPulse1Mode
    ****************************************************************************
    *
    * Summary:
    *  Sets the pulse density type for Density1. Less than or Equal(<=) or 
    *  Greater than or Equal(>=).
    *
    * Parameters:  
    *  pulse1Type: Selected pulse density type.
    *
    * Return:
    *  None.
    *
    ***************************************************************************/
    void `$INSTANCE_NAME`_SetPulse1Mode(uint8 pulse1Type) `=ReentrantKeil($INSTANCE_NAME . "_SetPulse1Mode")`
    {
        if(pulse1Type == `$INSTANCE_NAME`_GREATERTHAN_OR_EQUAL)
        {
            `$INSTANCE_NAME`_CONTROL_REG |= `$INSTANCE_NAME`_CTRL_COMPARE_TYPE1_GREATER_THAN_OR_EQUAL;
        }
        else
        {
            `$INSTANCE_NAME`_CONTROL_REG &= ~`$INSTANCE_NAME`_CTRL_COMPARE_TYPE1_GREATER_THAN_OR_EQUAL;
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
*  None.
*
* Return:
*  Current Period register value.
*
*******************************************************************************/
`$RegSizeReplacementString` `$INSTANCE_NAME`_ReadSeed(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadSeed")`
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
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteSeed(`$RegSizeReplacementString` seed) `=ReentrantKeil($INSTANCE_NAME . "_WriteSeed")`
{
    if(seed != 0u)
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
*  None.
*
* Return:
*  PrISM polynomial.
*
*******************************************************************************/
`$RegSizeReplacementString` `$INSTANCE_NAME`_ReadPolynomial(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadPolynomial")`
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
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_WritePolynomial(`$RegSizeReplacementString` polynomial) \
                                                                `=ReentrantKeil($INSTANCE_NAME . "_WritePolynomial")`
{
    `$CySetRegReplacementString`(`$INSTANCE_NAME`_POLYNOM_PTR, polynomial);
    
}


/*******************************************************************************
* FUNCTION NAME:   `$INSTANCE_NAME`_ReadPulse0
********************************************************************************
*
* Summary:
*  Reads the PrISM Pulse Density0 register.
*
* Parameters:
*  None.
*
* Return:
*  Pulse Density0 register value.
*
*******************************************************************************/
`$RegSizeReplacementString` `$INSTANCE_NAME`_ReadPulse0(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadPulse0")`
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
*  pulseDensity0: Pulse Density value.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_WritePulse0(`$RegSizeReplacementString` pulseDensity0) \
                                                                `=ReentrantKeil($INSTANCE_NAME . "_WritePulse0")`
{
    `$CySetRegReplacementString`(`$INSTANCE_NAME`_DENSITY0_PTR, pulseDensity0);
}


/*******************************************************************************
* FUNCTION NAME:   `$INSTANCE_NAME`_ReadPulse1
********************************************************************************
*
* Summary:
*  Reads the PrISM Pulse Density1 register.
*
* Parameters:
*  None.
*
* Return:
*  Pulse Density1 register value.
*
*******************************************************************************/
`$RegSizeReplacementString` `$INSTANCE_NAME`_ReadPulse1(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadPulse1")`
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
*  pulseDensity1: Pulse Density value.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_WritePulse1(`$RegSizeReplacementString` pulseDensity1) \
                                                                    `=ReentrantKeil($INSTANCE_NAME . "_WritePulse1")`
{
    `$CySetRegReplacementString`(`$INSTANCE_NAME`_DENSITY1_PTR, pulseDensity1);
}


/* [] END OF FILE */
