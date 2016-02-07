/*******************************************************************************
* File Name: idac.c
* Version 1.0
*
* Description:
*  This file provides the source code of APIs for the IDAC_P4 component.
*
*******************************************************************************
* Copyright 2013, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "idac.h"

uint32 idac_initVar = 0u;


/*******************************************************************************
* Function Name: idac_Init
********************************************************************************
*
* Summary:
*  Initializes IDAC registers with initial values provided from customizer.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global variables:
*  None
*
*******************************************************************************/
void idac_Init(void)
{
    uint8 enableInterrupts;

    /* Set initial configuration */
    enableInterrupts = CyEnterCriticalSection();

    /* clear previous values */
    idac_IDAC_CONTROL_REG &= ((uint32)~((uint32)idac_IDAC_VALUE_MASK <<
        idac_IDAC_VALUE_POSITION)) | ((uint32)~((uint32)idac_IDAC_MODE_MASK <<
        idac_IDAC_MODE_POSITION))  | ((uint32)~((uint32)idac_IDAC_RANGE_MASK  <<
        idac_IDAC_RANGE_POSITION));

    idac_IDAC_POLARITY_CONTROL_REG &= (~(uint32)((uint32)idac_IDAC_POLARITY_MASK <<
        idac_IDAC_POLARITY_POSITION));

    /* set new configuration */
    idac_IDAC_CONTROL_REG |= (((uint32)idac_IDAC_INIT_VALUE <<
        idac_IDAC_VALUE_POSITION) | ((uint32)idac_IDAC_RANGE <<
        idac_IDAC_RANGE_POSITION));

    idac_IDAC_POLARITY_CONTROL_REG |= ((uint32)idac_IDAC_POLARITY <<
                                                           idac_IDAC_POLARITY_POSITION);

    CyExitCriticalSection(enableInterrupts);

}


/*******************************************************************************
* Function Name: idac_Enable
********************************************************************************
*
* Summary:
*  Enables IDAC operations.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global variables:
*  None
*
*******************************************************************************/
void idac_Enable(void)
{
    uint8 enableInterrupts;

    enableInterrupts = CyEnterCriticalSection();

    /* Enable the IDAC */
    idac_IDAC_CONTROL_REG |= ((uint32)idac_IDAC_EN_MODE <<
                                                  idac_IDAC_MODE_POSITION);
    idac_IDAC_POLARITY_CONTROL_REG |= ((uint32)idac_IDAC_CSD_EN <<
                                                           idac_IDAC_CSD_EN_POSITION);
    CyExitCriticalSection(enableInterrupts);

}


/*******************************************************************************
* Function Name: idac_Start
********************************************************************************
*
* Summary:
*  Starts the IDAC hardware.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global variables:
*  idac_initVar
*
*******************************************************************************/
void idac_Start(void)
{
    if(0u == idac_initVar)
    {
        idac_Init();
        idac_initVar = 1u;
    }

    idac_Enable();

}


/*******************************************************************************
* Function Name: idac_Stop
********************************************************************************
*
* Summary:
*  Stops the IDAC hardware.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global variables:
*  None
*
*******************************************************************************/
void idac_Stop(void)
{
    uint8 enableInterrupts;

    enableInterrupts = CyEnterCriticalSection();

    /* Disable the IDAC */
    idac_IDAC_CONTROL_REG &= ((uint32)~((uint32)idac_IDAC_MODE_MASK <<
        idac_IDAC_MODE_POSITION));
    CyExitCriticalSection(enableInterrupts);
}


/*******************************************************************************
* Function Name: idac_SetValue
********************************************************************************
*
* Summary:
*  Sets the IDAC value.
*
* Parameters:
*  uint32 value
*
* Return:
*  None
*
* Global variables:
*  None
*
*******************************************************************************/
void idac_SetValue(uint32 value)
{
    uint8 enableInterrupts;
    uint32 newRegisterValue;

    enableInterrupts = CyEnterCriticalSection();

    #if(idac_IDAC_VALUE_POSITION != 0u)
        newRegisterValue = ((idac_IDAC_CONTROL_REG & (~(uint32)((uint32)idac_IDAC_VALUE_MASK <<
        idac_IDAC_VALUE_POSITION))) | (value << idac_IDAC_VALUE_POSITION));
    #else
        newRegisterValue = ((idac_IDAC_CONTROL_REG & (~(uint32)idac_IDAC_VALUE_MASK)) | value);
    #endif /* idac_IDAC_VALUE_POSITION != 0u */

    idac_IDAC_CONTROL_REG = newRegisterValue;

    CyExitCriticalSection(enableInterrupts);
}

/* [] END OF FILE */
