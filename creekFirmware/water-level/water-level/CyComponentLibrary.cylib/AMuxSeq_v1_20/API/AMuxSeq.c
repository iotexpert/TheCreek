/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*    This file contains all functions required for the analog multiplexer
*    AMuxSeq User Module.
*
*   Note:
*
*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#include "cytypes.h"
#include "`$INSTANCE_NAME`.h"
#include "cyfitter_cfg.h"

#if(`$INSTANCE_NAME`_MUXTYPE == `$INSTANCE_NAME`_MUX_DIFF)

extern int8 `$INSTANCE_NAME`_CYAMUXSIDE_A_curChannel;

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Next
********************************************************************************
* Summary:
*  This function selects the next mux input. If the last input is selected, it
*  selects the first input.
*
* Parameters:  
*  channel:  (uint8) Selects the channel in which to disconnect.
*
* Return: 
*  (void)
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_Next(void) 
{
   `$INSTANCE_NAME`_CYAMUXSIDE_A_Next();
   `$INSTANCE_NAME`_CYAMUXSIDE_B_Next();
}

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_DisconnectAll
********************************************************************************
* Summary:
*  This function disconnects all channels.
*
* Parameters:  
*  (void)
*
* Return: 
*  (void)
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_DisconnectAll(void)
{
   `$INSTANCE_NAME`_CYAMUXSIDE_A_DisconnectAll();
   `$INSTANCE_NAME`_CYAMUXSIDE_B_DisconnectAll();
}

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetChannel
********************************************************************************
* Summary:
*  The currently connected channel is retuned. If no channel is connected
*  returns -1.
*
* Parameters:  
*  (void)
*
* Return: 
*  The current channel or -1.
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
int8 `$INSTANCE_NAME`_GetChannel(void)
{
	return `$INSTANCE_NAME`_CYAMUXSIDE_A_curChannel;
}

#else

extern int8 `$INSTANCE_NAME`_curChannel;

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetChannel
********************************************************************************
* Summary:
*  The currently connected channel is retuned. If no channel is connected
*  returns -1.
*
* Parameters:  
*  (void)
*
* Return: 
*  The current channel or -1.
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
int8 `$INSTANCE_NAME`_GetChannel(void)
{
	return `$INSTANCE_NAME`_curChannel;
}

#endif

/* [] END OF FILE */
