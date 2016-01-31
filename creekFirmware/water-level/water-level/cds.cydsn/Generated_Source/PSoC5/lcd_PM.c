/*******************************************************************************
* File Name: lcd_PM.c
* Version 1.70
*
* Description:
*  This file provides the API source code for the Static Segment LCD component.
*
* Note:
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "lcd.h"


void lcd_SaveConfig(void) ;
void lcd_RestoreConfig(void) ;
extern void lcd_Enable(void) ;

static lcd_BACKUP_STRUCT lcd_backup;

extern uint8 lcd_enableState;


/*******************************************************************************
* Function Name: lcd_SaveConfig
********************************************************************************
*
* Summary:
*  Does nothing, provided for consistency.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
void lcd_SaveConfig(void) 
{
}


/*******************************************************************************
* Function Name: lcd_RestoreConfig
********************************************************************************
*
* Summary:
*  Does nothing, provided for consistency.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
void lcd_RestoreConfig(void) 
{
}


/*******************************************************************************
* Function Name: lcd_Sleep
********************************************************************************
*
* Summary:
*  Prepares component for entering the sleep mode.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void lcd_Sleep(void) 
{
    lcd_backup.enableState = lcd_enableState;
    lcd_SaveConfig();
    lcd_Stop();
}


/*******************************************************************************
* Function Name: lcd_Wakeup
********************************************************************************
*
* Summary:
*  Wakes component from sleep mode. Configures DMA and enables the component for
*  normal operation.
*
* Parameters:
*  lcd_enableState - Global variable.
*
* Return:
*  Status one of standard status for PSoC3 Component
*       CYRET_SUCCESS - Function completed successfully.
*       CYRET_LOCKED - The object was locked, already in use. Some of TDs or
*                      a channel already in use.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void lcd_Wakeup(void) 
{
    lcd_RestoreConfig();

    if(lcd_backup.enableState == 1u)
    {
        lcd_Enable();
    }
}


/* [] END OF FILE */
