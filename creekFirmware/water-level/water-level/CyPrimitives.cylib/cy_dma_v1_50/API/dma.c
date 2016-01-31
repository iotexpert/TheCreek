/***************************************************************************
* File Name: `$INSTANCE_NAME`_dma.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*   Provides an API for the DMAC component. The API includes functions
*   for the DMA controller, DMA channels and Transfer Descriptors.
*
*
*   Note:
*     This module requires the developer to finish or fill in the auto
*     generated funcions and setup the dma channel and TD's.
*
********************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/
#include <CYLIB.H>
#include <CYDMAC.H>
#include <`$INSTANCE_NAME`_dma.H>



/****************************************************************************
* 
* The following defines are available in Cyfitter.h
* 
* 
* 
* `$INSTANCE_NAME`__DRQ_CTL_REG
* 
* 
* `$INSTANCE_NAME`__DRQ_NUMBER
* 
* Number of TD's used by this channel.
* `$INSTANCE_NAME`__NUMBEROF_TDS
* 
* Priority of this channel.
* `$INSTANCE_NAME`__PRIORITY
* 
* True if `$INSTANCE_NAME`_TERMIN_SEL is used.
* `$INSTANCE_NAME`__TERMIN_EN
* 
* TERMIN interrupt line to signal terminate.
* `$INSTANCE_NAME`__TERMIN_SEL
* 
* 
* True if `$INSTANCE_NAME`_TERMOUT0_SEL is used.
* `$INSTANCE_NAME`__TERMOUT0_EN
* 
* 
* TERMOUT0 interrupt line to signal completion.
* `$INSTANCE_NAME`__TERMOUT0_SEL
* 
* 
* True if `$INSTANCE_NAME`_TERMOUT1_SEL is used.
* `$INSTANCE_NAME`__TERMOUT1_EN
* 
* 
* TERMOUT1 interrupt line to signal completion.
* `$INSTANCE_NAME`__TERMOUT1_SEL
* 
****************************************************************************/


/* Zero based index of `$INSTANCE_NAME` dma channel */
uint8 `$INSTANCE_NAME`_DmaHandle = DMA_INVALID_CHANNEL;

/*********************************************************************
* Function Name: uint8 `$INSTANCE_NAME`_DmaInitalize
**********************************************************************
* Summary:
*   Allocates and initialises a channel of the DMAC to be used by the
*   caller.
*
* Parameters:
*   BurstCount.
*       
*       
*   ReqestPerBurst.
*       
*       
*   UpperSrcAddress.
*       
*       
*   UpperDestAddress.
*       
*
* Return:
*   The channel that can be used by the caller for DMA activity.
*   DMA_INVALID_CHANNEL (0xFF) if there are no channels left. 
*
*
*******************************************************************/
uint8 `$INSTANCE_NAME`_DmaInitialize(uint8 BurstCount, uint8 ReqestPerBurst, uint16 UpperSrcAddress, uint16 UpperDestAddress)
{

    /* Allocate a DMA channel. */
    `$INSTANCE_NAME`_DmaHandle = `$INSTANCE_NAME`__DRQ_NUMBER;

    if(`$INSTANCE_NAME`_DmaHandle != DMA_INVALID_CHANNEL)
    {
        /* Configure the channel. */
        CyDmaChSetConfiguration(`$INSTANCE_NAME`_DmaHandle,
                                BurstCount,
                                ReqestPerBurst,
                                `$INSTANCE_NAME`__TERMOUT0_SEL,
                                `$INSTANCE_NAME`__TERMOUT1_SEL,
                                `$INSTANCE_NAME`__TERMIN_SEL);

        /* Set the extended address for the transfers */
        CyDmaChSetExtendedAddress(`$INSTANCE_NAME`_DmaHandle, UpperSrcAddress, UpperDestAddress);

        /* Set the priority for this channel */
        CyDmaChPriority(`$INSTANCE_NAME`_DmaHandle, `$INSTANCE_NAME`__PRIORITY);
    }

    return `$INSTANCE_NAME`_DmaHandle;
}

/*********************************************************************
* Function Name: void `$INSTANCE_NAME`_DmaRelease
**********************************************************************
* Summary:
*   Frees the channel associated with `$INSTANCE_NAME`.
*
*
* Parameters:
*   void.
*
*
*
* Return:
*   void.
*
*******************************************************************/
void `$INSTANCE_NAME`_DmaRelease(void) `=ReentrantKeil($INSTANCE_NAME ."_DmaRelease")`
{
    /* Disable the channel, even if someone just did! */
    CyDmaChDisable(`$INSTANCE_NAME`_DmaHandle);


    /* Free Transfer Descriptors. */


}

