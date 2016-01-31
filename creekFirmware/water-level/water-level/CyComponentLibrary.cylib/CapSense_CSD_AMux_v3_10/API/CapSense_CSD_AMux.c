/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*    This file contains all functions required for the analog multiplexer
*    CapSense_CSD_AMux User Module.
*
*   Note:
*
*******************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

#include "`$INSTANCE_NAME`.h"

uint8 `$INSTANCE_NAME`_initVar = 0u;
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
    `$INSTANCE_NAME`_DisconnectAll();
    `$INSTANCE_NAME`_initVar = 1u;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Init
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
void `$INSTANCE_NAME`_Init(void)
{
    `$INSTANCE_NAME`_DisconnectAll();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
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
void `$INSTANCE_NAME`_Stop(void)
{
    `$INSTANCE_NAME`_DisconnectAll();
}


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
    if( `$INSTANCE_NAME`_lastChannel != `$INSTANCE_NAME`_NULL_CHANNEL)   /* Update last channel */
    {
        `$INSTANCE_NAME`_Disconnect(`$INSTANCE_NAME`_lastChannel);
    }

    /* Make the new channel connection */
    `$INSTANCE_NAME`_Connect(channel);
    `$INSTANCE_NAME`_lastChannel = channel;   /* Update last channel */
}


#if(`$INSTANCE_NAME`_MUXTYPE == `$INSTANCE_NAME`_MUX_DIFF)
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
    
#endif  /* End (`$INSTANCE_NAME`_MUXTYPE == `$INSTANCE_NAME`_MUX_DIFF) */


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
    uint8 chan;

    #if(`$INSTANCE_NAME`_MUXTYPE == `$INSTANCE_NAME`_MUX_SINGLE)
        for(chan = 0; chan < `$INSTANCE_NAME`_CHANNELS ; chan++)
        {
            `$INSTANCE_NAME`_Unset(chan);
        }
    #else
        for(chan = 0; chan < `$INSTANCE_NAME`_CHANNELS ; chan++)
        {
            `$INSTANCE_NAME`_CYAMUXSIDE_A_Unset(chan);
            `$INSTANCE_NAME`_CYAMUXSIDE_B_Unset(chan);
        }
    #endif  /* End (`$INSTANCE_NAME`_MUXTYPE == `$INSTANCE_NAME`_MUX_SINGLE) */
}


/* [] END OF FILE */
