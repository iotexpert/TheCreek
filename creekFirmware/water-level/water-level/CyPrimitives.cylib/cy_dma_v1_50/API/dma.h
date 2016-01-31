/******************************************************************************
* File Name: `$INSTANCE_NAME`_dma.h  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*   Provides the function definitions for the DMA Controller.
*
*
********************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/
#if !defined(__`$INSTANCE_NAME`_DMA_H__)
#define __`$INSTANCE_NAME`_DMA_H__



#include <CYDMAC.H>
#include <CYFITTER.H>

#define `$INSTANCE_NAME`__TD_TERMOUT_EN ((`$INSTANCE_NAME`__TERMOUT0_EN ? TD_TERMOUT0_EN : 0) | \
    (`$INSTANCE_NAME`__TERMOUT1_EN ? TD_TERMOUT1_EN : 0))

/* Zero based index of `$INSTANCE_NAME` dma channel */
extern uint8 `$INSTANCE_NAME`_DmaHandle;


uint8 `$INSTANCE_NAME`_DmaInitialize(uint8 BurstCount, uint8 ReqestPerBurst, uint16 UpperSrcAddress, uint16 UpperDestAddress);
void  `$INSTANCE_NAME`_DmaRelease(void) `=ReentrantKeil($INSTANCE_NAME ."_DmaRelease")`;


/* __`$INSTANCE_NAME`_DMA_H__ */
#endif
