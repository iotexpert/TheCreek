/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*    This file contains the constants and function prototypes for the Analog
*    Multiplexer User Module AMux.
*
*   Note:
*
********************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

#if !defined(CY_AMUX_`$INSTANCE_NAME`_H)
#define CY_AMUX_`$INSTANCE_NAME`_H

#include "cytypes.h"
#include "cyfitter.h"
#include "cyfitter_cfg.h"


/***************************************
*        Function Prototypes
***************************************/

void `$INSTANCE_NAME`_Start(void);
void `$INSTANCE_NAME`_Init(void);
void `$INSTANCE_NAME`_Stop(void);
void `$INSTANCE_NAME`_Select(uint8 channel) `=ReentrantKeil($INSTANCE_NAME ."_Select")`;
void `$INSTANCE_NAME`_FastSelect(uint8 channel) `=ReentrantKeil($INSTANCE_NAME ."_FastSelect")`;
void `$INSTANCE_NAME`_DisconnectAll(void) `=ReentrantKeil($INSTANCE_NAME ."_DisconnectAll")`;
/* The Connect and Disconnect functions are declared elsewhere */
/* void `$INSTANCE_NAME`_Connect(uint8 channel); */
/* void `$INSTANCE_NAME`_Disconnect(uint8 channel); */


/***************************************
*     Initial Parameter Constants
***************************************/

#define `$INSTANCE_NAME`_CHANNELS  `$Channels`u
#define `$INSTANCE_NAME`_MUXTYPE   `$MuxType`u


/***************************************
*             API Constants
***************************************/

#define `$INSTANCE_NAME`_NULL_CHANNEL  0xFFu
#define `$INSTANCE_NAME`_MUX_SINGLE   1u
#define `$INSTANCE_NAME`_MUX_DIFF     2u


/***************************************
*        Conditional Functions
***************************************/

#if (`$INSTANCE_NAME`_MUXTYPE == `$INSTANCE_NAME`_MUX_SINGLE)
    # define `$INSTANCE_NAME`_Connect(channel) `$INSTANCE_NAME`_Set(channel)
    # define `$INSTANCE_NAME`_Disconnect(channel) `$INSTANCE_NAME`_Unset(channel)
#else
    void `$INSTANCE_NAME`_Connect(uint8 channel) `=ReentrantKeil($INSTANCE_NAME ."_Connect")`;
    void `$INSTANCE_NAME`_Disconnect(uint8 channel) `=ReentrantKeil($INSTANCE_NAME ."_Disconnect")`;
#endif /* `$INSTANCE_NAME`_MUXTYPE == `$INSTANCE_NAME`_MUX_SINGLE */

#endif /* CY_AMUX_`$INSTANCE_NAME`_H */


/* [] END OF FILE */
