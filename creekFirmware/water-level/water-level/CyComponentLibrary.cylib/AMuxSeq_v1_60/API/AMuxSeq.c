/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*    This file contains functions for the AMuxSeq.
*
*   Note:
*
*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

#include "`$INSTANCE_NAME`.h"

uint8 `$INSTANCE_NAME`_initVar = 0u;


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
* Summary:
*  Disconnect all channels. The next time Next is called, channel 0 will be
*  connected.
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
    `$INSTANCE_NAME`_DisconnectAll();
    `$INSTANCE_NAME`_initVar = 1u;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Init
********************************************************************************
* Summary:
*  Disconnect all channels. The next time Next is called, channel 0 will be
*  connected.
*
* Parameters:
*  void
*
* Return:
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_Init(void)
{
    `$INSTANCE_NAME`_DisconnectAll();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
* Summary:
*  Disconnect all channels. The next time Next is called, channel 0 will be
*  connected.
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
    `$INSTANCE_NAME`_DisconnectAll();
}

#if(`$INSTANCE_NAME`_MUXTYPE == `$INSTANCE_NAME`_MUX_DIFF)

extern int8 `$INSTANCE_NAME`_CYAMUXSIDE_A_curChannel;


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Next
********************************************************************************
* Summary:
*  Disconnects the previous channel and connects the next one in the sequence.
*  When Next is called for the first time after Init, Start, Enable, Stop, or
*  DisconnectAll, it connects channel 0.
*
* Parameters:
*  void
*
* Return:
*  void
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
*  This function disconnects all channels. The next time Next is called, channel
*  0 will be connected.
*
* Parameters:
*  void
*
* Return:
*  void
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
*  void
*
* Return:
*  The current channel or -1.
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
*  void
*
* Return:
*  The current channel or -1.
*
*******************************************************************************/
int8 `$INSTANCE_NAME`_GetChannel(void)
{
    return `$INSTANCE_NAME`_curChannel;
}

#endif


/* [] END OF FILE */
