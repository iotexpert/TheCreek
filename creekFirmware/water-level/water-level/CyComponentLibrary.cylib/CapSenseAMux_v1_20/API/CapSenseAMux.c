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
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#include "cytypes.h"
#include "`$INSTANCE_NAME`.h"
#include "cyfitter_cfg.h"

uint8 `$INSTANCE_NAME`_lastChannel = `$INSTANCE_NAME`_NULL_CHANNEL; 

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
* Summary:
*  This function resets all connections, to disconnected.
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
void `$INSTANCE_NAME`_Start(void) 
{
   `$INSTANCE_NAME`_DisconnectAll(); 
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
* Summary:
*  This function disconnects all connections.
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
void `$INSTANCE_NAME`_Stop(void) 
{
   `$INSTANCE_NAME`_DisconnectAll(); 
}



/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Select
********************************************************************************
* Summary:
*  This functions first disconnects all channels then connects the selected
*  channel "channel".
*
* Parameters:  
*  channel:  (uint8) Selects the channel in which to perform the operation.
*
* Return: 
*  (void) Description of return value, if there is a return value.
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_Select(uint8 channel) 
{
   `$INSTANCE_NAME`_DisconnectAll();        /* Disconnect all previous connections */
   `$INSTANCE_NAME`_Connect(channel);       /* Make the given selection */
   `$INSTANCE_NAME`_lastChannel = channel;  /* Update last channel */
}

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_FastSelect
********************************************************************************
* Summary:
*  This functions first disconnects just the last channel then connects the
*  given channel.
*
* Parameters:  
*  channel:  (uint8) Selects the channel in which to connect.
*
* Return: 
*  None
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_FastSelect(uint8 channel) 
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
*  This function connects the given channel, but does not affect any other
*  previous connection.
*
* Parameters:  
*  channel:  (uint8) Selects the channel in which to connect.
*
* Return: 
*  (void)
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_Connect(uint8 channel) 
{
   `$INSTANCE_NAME`_CYAMUXSIDE_A_Set(channel);
   `$INSTANCE_NAME`_CYAMUXSIDE_B_Set(channel);
}
#endif

#if(`$INSTANCE_NAME`_MUXTYPE == `$INSTANCE_NAME`_MUX_DIFF)
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Disconnect
********************************************************************************
* Summary:
*  This function disconnects the given channel from the common or output.  It
*  does not affect any other connection.
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
void `$INSTANCE_NAME`_Disconnect(uint8 channel) 
{
   `$INSTANCE_NAME`_CYAMUXSIDE_A_Unset(channel);
   `$INSTANCE_NAME`_CYAMUXSIDE_B_Unset(channel);
}
#endif

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
   #endif
}


/* [] END OF FILE */
