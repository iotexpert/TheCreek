/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*    This file contains the constants and function prototypes for the Analog
*    Multiplexer User Module AmuxSeq.
*
*   Note:
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#include "cytypes.h"
#include "cyfitter.h"
#include "cyfitter_cfg.h"

#if !defined(CY_AMUXSEQ_`$INSTANCE_NAME`_H) 
#define CY_AMUXSEQ_`$INSTANCE_NAME`_H 

/***************************************
*        Function Prototypes 
***************************************/

#define `$INSTANCE_NAME`_Start() `$INSTANCE_NAME`_DisconnectAll()
#define `$INSTANCE_NAME`_Stop() `$INSTANCE_NAME`_DisconnectAll()
void    `$INSTANCE_NAME`_Next(void);
void    `$INSTANCE_NAME`_DisconnectAll(void);
int8    `$INSTANCE_NAME`_GetChannel(void);

/***************************************
*           Parameter Defaults        
***************************************/
#define `$INSTANCE_NAME`_CHANNELS  `$Channels`
#define `$INSTANCE_NAME`_MUXTYPE   `$MuxType`

/***************************************
*              Constants        
***************************************/


#define `$INSTANCE_NAME`_NULL_CHANNEL  0xFF
#define `$INSTANCE_NAME`_MUX_SINGLE   1
#define `$INSTANCE_NAME`_MUX_DIFF     2

/***************************************
*              Registers        
***************************************/


#endif /* CY_AMUXSEQ_`$INSTANCE_NAME`_H */


/* [] END OF FILE */
