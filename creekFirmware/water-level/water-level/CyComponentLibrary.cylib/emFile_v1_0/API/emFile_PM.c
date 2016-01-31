/*******************************************************************************
* File Name: `$INSTANCE_NAME`_PM.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the API source code for Power Management of the emFile
*  component.
*
* Note:
*
*******************************************************************************
* Copyright 2011-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include <device.h>
#include "`$INSTANCE_NAME`.h"


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SaveConfig
********************************************************************************
*
* Summary:
*  Saves SPI Masters configuration.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_SaveConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_SaveConfig")`
{
    #if(`$INSTANCE_NAME`_NUMBER_SD_CARDS == 4u)
        `$INSTANCE_NAME`_SPI0_SaveConfig();
        `$INSTANCE_NAME`_SPI1_SaveConfig();
        `$INSTANCE_NAME`_SPI2_SaveConfig();
        `$INSTANCE_NAME`_SPI3_SaveConfig();
    #elif(`$INSTANCE_NAME`_NUMBER_SD_CARDS == 3u)
        `$INSTANCE_NAME`_SPI0_SaveConfig();
        `$INSTANCE_NAME`_SPI1_SaveConfig();
        `$INSTANCE_NAME`_SPI2_SaveConfig();
    #elif(`$INSTANCE_NAME`_NUMBER_SD_CARDS == 2u)
        `$INSTANCE_NAME`_SPI0_SaveConfig();
        `$INSTANCE_NAME`_SPI1_SaveConfig();
    #else
        `$INSTANCE_NAME`_SPI0_SaveConfig();
    #endif /* (`$INSTANCE_NAME`_NUMBER_SD_CARDS == 4u) */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_RestoreConfig
********************************************************************************
*
* Summary:
*  Restores SPI Masters configuration.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Side Effects:
*  If this API is called without first calling SaveConfig then in the following
*  registers will be default values from Customizer:
*
*******************************************************************************/
void `$INSTANCE_NAME`_RestoreConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_RestoreConfig")`
{
    #if(`$INSTANCE_NAME`_NUMBER_SD_CARDS == 4u)
        `$INSTANCE_NAME`_SPI0_RestoreConfig();
        `$INSTANCE_NAME`_SPI1_RestoreConfig();
        `$INSTANCE_NAME`_SPI2_RestoreConfig();
        `$INSTANCE_NAME`_SPI3_RestoreConfig();
    #elif(`$INSTANCE_NAME`_NUMBER_SD_CARDS == 3u)
        `$INSTANCE_NAME`_SPI0_SaveConfig();
        `$INSTANCE_NAME`_SPI1_SaveConfig();
        `$INSTANCE_NAME`_SPI2_SaveConfig();
    #elif(`$INSTANCE_NAME`_NUMBER_SD_CARDS == 2u)
        `$INSTANCE_NAME`_SPI0_SaveConfig();
        `$INSTANCE_NAME`_SPI1_SaveConfig();
    #else
        `$INSTANCE_NAME`_SPI0_SaveConfig();
    #endif /* (`$INSTANCE_NAME`_NUMBER_SD_CARDS == 4u) */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Sleep
********************************************************************************
*
* Summary:
*  Prepare emFile to go to sleep.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_Sleep(void) `=ReentrantKeil($INSTANCE_NAME . "_Sleep")`
{
    #if(`$INSTANCE_NAME`_NUMBER_SD_CARDS == 4u)
        `$INSTANCE_NAME`_SPI0_Sleep();
        `$INSTANCE_NAME`_SPI1_Sleep();
        `$INSTANCE_NAME`_SPI2_Sleep();
        `$INSTANCE_NAME`_SPI3_Sleep();
    #elif(`$INSTANCE_NAME`_NUMBER_SD_CARDS == 3u)
        `$INSTANCE_NAME`_SPI0_Sleep();
        `$INSTANCE_NAME`_SPI1_Sleep();
        `$INSTANCE_NAME`_SPI2_Sleep();
    #elif(`$INSTANCE_NAME`_NUMBER_SD_CARDS == 2u)
        `$INSTANCE_NAME`_SPI0_Sleep();
        `$INSTANCE_NAME`_SPI1_Sleep();
    #else
        `$INSTANCE_NAME`_SPI0_Sleep();
    #endif /* (`$INSTANCE_NAME`_NUMBER_SD_CARDS == 4u) */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Wakeup
********************************************************************************
*
* Summary:
*  Prepare SPIM Components to wake up.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_Wakeup(void) `=ReentrantKeil($INSTANCE_NAME . "_Wakeup")`
{
    #if(`$INSTANCE_NAME`_NUMBER_SD_CARDS == 4u)
        `$INSTANCE_NAME`_SPI0_Wakeup();
        `$INSTANCE_NAME`_SPI1_Wakeup();
        `$INSTANCE_NAME`_SPI2_Wakeup();
        `$INSTANCE_NAME`_SPI3_Wakeup();
    #elif(`$INSTANCE_NAME`_NUMBER_SD_CARDS == 3u)
        `$INSTANCE_NAME`_SPI0_Wakeup();
        `$INSTANCE_NAME`_SPI1_Wakeup();
        `$INSTANCE_NAME`_SPI2_Wakeup();
    #elif(`$INSTANCE_NAME`_NUMBER_SD_CARDS == 2u)
        `$INSTANCE_NAME`_SPI0_Wakeup();
        `$INSTANCE_NAME`_SPI1_Wakeup();
    #else
        `$INSTANCE_NAME`_SPI0_Wakeup();
    #endif /* (`$INSTANCE_NAME`_NUMBER_SD_CARDS == 4u) */
}


/* [] END OF FILE */
