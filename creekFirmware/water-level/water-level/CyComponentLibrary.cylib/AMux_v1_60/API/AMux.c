/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*    This file contains all functions required for the analog multiplexer
*    AMux User Module.
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

uint8 `$INSTANCE_NAME`_lastChannel = `$INSTANCE_NAME`_NULL_CHANNEL;


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
* Summary:
*  Disconnect all channels.
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
    uint8 chan;

    for(chan = 0; chan < `$INSTANCE_NAME`_CHANNELS ; chan++)
    {
#if(`$INSTANCE_NAME`_MUXTYPE == `$INSTANCE_NAME`_MUX_SINGLE)
        `$INSTANCE_NAME`_Unset(chan);
#else
        `$INSTANCE_NAME`_CYAMUXSIDE_A_Unset(chan);
        `$INSTANCE_NAME`_CYAMUXSIDE_B_Unset(chan);
#endif
    }

	`$INSTANCE_NAME`_lastChannel = `$INSTANCE_NAME`_NULL_CHANNEL;
}


#if(!`$INSTANCE_NAME`_ATMOSTONE)
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Select
********************************************************************************
* Summary:
*  This functions first disconnects all channels then connects the given
*  channel.
*
* Parameters:
*  channel:  The channel to connect to the common terminal.
*
* Return:
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_Select(uint8 channel) `=ReentrantKeil($INSTANCE_NAME ."_Select")`
{
    `$INSTANCE_NAME`_DisconnectAll();        /* Disconnect all previous connections */
    `$INSTANCE_NAME`_Connect(channel);       /* Make the given selection */
    `$INSTANCE_NAME`_lastChannel = channel;  /* Update last channel */
}
#endif


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_FastSelect
********************************************************************************
* Summary:
*  This function first disconnects the last connection made with FastSelect or
*  Select, then connects the given channel. The FastSelect function is similar
*  to the Select function, except it is faster since it only disconnects the
*  last channel selected rather than all channels.
*
* Parameters:
*  channel:  The channel to connect to the common terminal.
*
* Return:
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_FastSelect(uint8 channel) `=ReentrantKeil($INSTANCE_NAME ."_FastSelect")`
{
    /* Disconnect the last valid channel */
    if( `$INSTANCE_NAME`_lastChannel != `$INSTANCE_NAME`_NULL_CHANNEL)
    {
        `$INSTANCE_NAME`_Disconnect(`$INSTANCE_NAME`_lastChannel);
    }

    /* Make the new channel connection */
#if(`$INSTANCE_NAME`_MUXTYPE == `$INSTANCE_NAME`_MUX_SINGLE)
    `$INSTANCE_NAME`_Set(channel);
#else
    `$INSTANCE_NAME`_CYAMUXSIDE_A_Set(channel);
    `$INSTANCE_NAME`_CYAMUXSIDE_B_Set(channel);
#endif


	`$INSTANCE_NAME`_lastChannel = channel;   /* Update last channel */
}


#if(`$INSTANCE_NAME`_MUXTYPE == `$INSTANCE_NAME`_MUX_DIFF)
#if(!`$INSTANCE_NAME`_ATMOSTONE)
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Connect
********************************************************************************
* Summary:
*  This function connects the given channel without affecting other connections.
*
* Parameters:
*  channel:  The channel to connect to the common terminal.
*
* Return:
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_Connect(uint8 channel) `=ReentrantKeil($INSTANCE_NAME ."_Connect")`
{
    `$INSTANCE_NAME`_CYAMUXSIDE_A_Set(channel);
    `$INSTANCE_NAME`_CYAMUXSIDE_B_Set(channel);
}
#endif

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Disconnect
********************************************************************************
* Summary:
*  This function disconnects the given channel from the common or output
*  terminal without affecting other connections.
*
* Parameters:
*  channel:  The channel to disconnect from the common terminal.
*
* Return:
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_Disconnect(uint8 channel) `=ReentrantKeil($INSTANCE_NAME ."_Disconnect")`
{
    `$INSTANCE_NAME`_CYAMUXSIDE_A_Unset(channel);
    `$INSTANCE_NAME`_CYAMUXSIDE_B_Unset(channel);
}
#endif

#if(`$INSTANCE_NAME`_ATMOSTONE)
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_DisconnectAll
********************************************************************************
* Summary:
*  This function disconnects all channels.
*
* Parameters:
*  void
*
* Return:
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_DisconnectAll(void) `=ReentrantKeil($INSTANCE_NAME ."_DisconnectAll")`
{
    if(`$INSTANCE_NAME`_lastChannel != `$INSTANCE_NAME`_NULL_CHANNEL) 
    {
        `$INSTANCE_NAME`_Disconnect(`$INSTANCE_NAME`_lastChannel);
		`$INSTANCE_NAME`_lastChannel = `$INSTANCE_NAME`_NULL_CHANNEL;
    }
}
#endif

/* [] END OF FILE */
