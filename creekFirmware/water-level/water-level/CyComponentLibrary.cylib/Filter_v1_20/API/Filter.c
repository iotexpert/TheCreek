/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the API source code for the FILT component.
*
* Note:
*  
*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#include "`$INSTANCE_NAME`.h"
#include "`$INSTANCE_NAME`_PVT.h"
#include "CyLib.h"


/*******************************************************************************
* FILT component internal variables.
*******************************************************************************/

static uint8 XDATA `$INSTANCE_NAME`_isInitialized = 0;

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
* Summary:
*  This method does the prep work necessary to setup DFB.  This includes loading 
*
* Parameters:  
*  none
* 
* Return: 
*  void
* 
* Note: 
*  Use `$INSTANCE_NAME`_InterruptConfig to control which events trigger 
*  interrupts in the DFB. 
*
*******************************************************************************/
void `$INSTANCE_NAME`_Start()
{
    /* Power on DFB */
    `$INSTANCE_NAME`_PM_ACT_CFG |= `$INSTANCE_NAME`_PM_ACT_MSK;
        
    if (`$INSTANCE_NAME`_isInitialized == 0)
    {        
        /* Turn off Run Bit */
        `$INSTANCE_NAME`_CR &= ~`$INSTANCE_NAME`_RUN_MASK;
                
        /* Power up the DFB RAMS */
        `$INSTANCE_NAME`_RAM_EN = `$INSTANCE_NAME`_RAM_DIR_BUS;
        
        /* Put DFB RAM on the bus */
        `$INSTANCE_NAME`_RAM_DIR = `$INSTANCE_NAME`_RAM_DIR_BUS;
        
        /* Write DFB RAMs */
        /* Control Store RAMs */
        cymemcpy( `$INSTANCE_NAME`_CSA_RAM,
            `$INSTANCE_NAME`_control, `$INSTANCE_NAME`_CSA_RAM_SIZE); 
        cymemcpy(`$INSTANCE_NAME`_CSB_RAM,
            `$INSTANCE_NAME`_control, `$INSTANCE_NAME`_CSB_RAM_SIZE); 
        /* CFSM RAM */
        cymemcpy(`$INSTANCE_NAME`_CFSM_RAM,
            `$INSTANCE_NAME`_cfsm, `$INSTANCE_NAME`_CFSM_RAM_SIZE); 
        /* DAta RAMs */
        cymemcpy(`$INSTANCE_NAME`_DA_RAM,
            `$INSTANCE_NAME`_data_a, `$INSTANCE_NAME`_DA_RAM_SIZE); 
        cymemcpy(`$INSTANCE_NAME`_DB_RAM,
            `$INSTANCE_NAME`_data_b, `$INSTANCE_NAME`_DB_RAM_SIZE); 
        /* ACU RAM */
        cymemcpy(`$INSTANCE_NAME`_ACU_RAM,
            `$INSTANCE_NAME`_acu, `$INSTANCE_NAME`_ACU_RAM_SIZE); 

        /* Take DFB RAM off the bus */
        `$INSTANCE_NAME`_RAM_DIR =`$INSTANCE_NAME`_RAM_DIR_DFB;

        /* Set up interrupt and DMA events */
        `$INSTANCE_NAME`_SetInterruptMode(`$INSTANCE_NAME`_INIT_INTERRUPT_MODE);
        `$INSTANCE_NAME`_SetDMAMode(`$INSTANCE_NAME`_INIT_DMA_MODE);
        
        /* Clear any pending interrupts */
        `$INSTANCE_NAME`_SR = 0xff;
        
        `$INSTANCE_NAME`_isInitialized = 1;
    }

    /* Turn on Run Bit */
    `$INSTANCE_NAME`_CR |= `$INSTANCE_NAME`_RUN_MASK;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
* Summary:
*  Turn off the run bit.  If DMA control is used to feed the channels, allow 
*  arguments to turn one of the TD channels off. 
*
* Parameters:  
*  none
*
* Return: 
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop()
{
    /* Power off DFB */
    `$INSTANCE_NAME`_PM_ACT_CFG &= ~`$INSTANCE_NAME`_PM_ACT_MSK;
        
    `$INSTANCE_NAME`_CR &= ~(`$INSTANCE_NAME`_RUN_MASK);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Read8
********************************************************************************
* Summary:
*  Get the value in one of the DFB Output Holding Registers 
*
* Parameters:  
*  channel:  `$INSTANCE_NAME`_CHANNEL_A or `$INSTANCE_NAME`_CHANNEL_B
*            
* Return: 
*  The most significant 8 bits of the current output value sitting in the 
*  selected channel's holding register or 0x00 for invalid channel numbers.
*
* Note:
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_Read8(uint8 channel)
{
    if (channel == `$INSTANCE_NAME`_CHANNEL_A)
    {
        return `$INSTANCE_NAME`_HOLDAH;
    }
    else if (channel == `$INSTANCE_NAME`_CHANNEL_B)
    {
        return `$INSTANCE_NAME`_HOLDBH;
    }
    else
    {
        return 0;
    }
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Read16
********************************************************************************
* Summary:
*  Get the value in one of the DFB Output Holding Registers 
*
* Parameters:  
*  channel:  `$INSTANCE_NAME`_CHANNEL_A or `$INSTANCE_NAME`_CHANNEL_B
*            
* Return: 
*  The most significant 16 bits of the current output value sitting in the 
*  selected channel's holding register or 0x0000 for invalid channel numbers
*
* Note:
*  Order of the read is important.  On the read of the high byte, the DFB clears
*  the data ready bit.
*******************************************************************************/
#if defined(__C51__) || defined(__CX51__)

uint16 `$INSTANCE_NAME`_Read16(uint8 channel)
{
    uint16 val;

    if (channel == `$INSTANCE_NAME`_CHANNEL_A)
    {        
        val = `$INSTANCE_NAME`_HOLDAM;
        val |= (uint16)(`$INSTANCE_NAME`_HOLDAH) << 8;
        return val;
    }
    else if (channel == `$INSTANCE_NAME`_CHANNEL_B)
    {      
        val = `$INSTANCE_NAME`_HOLDBM;
        val |= (uint16)`$INSTANCE_NAME`_HOLDBH << 8;
        return val;
    }
    else
    {
        return 0;
    }
}

#else

uint16 `$INSTANCE_NAME`_Read16(uint8 channel)
{
    if (channel == `$INSTANCE_NAME`_CHANNEL_A)
    {        
        return `$INSTANCE_NAME`_HOLDA16;
    }
    else if (channel == `$INSTANCE_NAME`_CHANNEL_B)
    {      
        return `$INSTANCE_NAME`_HOLDB16;
    }
    else
    {
        return 0;
    }
}

#endif

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Read24
********************************************************************************
* Summary:
*  Get the value in one of the DFB Output Holding Registers 
*
* Parameters:  
*  channel:  `$INSTANCE_NAME`_CHANNEL_A or `$INSTANCE_NAME`_CHANNEL_B
*            
* Return: 
*  The current 24-bit output value sitting in the selected channel's
*  holding register or 0x00000000 for invalid channel numbers
*
* Note:
*  Order of the read is important.  On the read of the high byte, the DFB clears
*  the data ready bit.
*******************************************************************************/
#if defined(__C51__) || defined(__CX51__)

uint32 `$INSTANCE_NAME`_Read24(uint8 channel)
{
    uint32 val;

    if (channel == `$INSTANCE_NAME`_CHANNEL_A)
    {        
        val = `$INSTANCE_NAME`_HOLDA;
        val |= (uint32)(`$INSTANCE_NAME`_HOLDAM) << 8;
        val |= (uint32)(`$INSTANCE_NAME`_HOLDAH) << 16;
        
        /* SignExtend */
        if(val & `$INSTANCE_NAME`_SIGN_BIT)
            val |= `$INSTANCE_NAME`_SIGN_BYTE;
            
        return val;
    }
    else if (channel == `$INSTANCE_NAME`_CHANNEL_B)
    {      
        val = `$INSTANCE_NAME`_HOLDB;
        val |= (uint32)`$INSTANCE_NAME`_HOLDBM << 8;
        val |= (uint32)`$INSTANCE_NAME`_HOLDBH << 16;
        
        /* SignExtend */
        if(val & `$INSTANCE_NAME`_SIGN_BIT)
            val |= `$INSTANCE_NAME`_SIGN_BYTE;
        
        return val;
    }
    else
    {
        return 0;
    }
}

#else

uint32 `$INSTANCE_NAME`_Read24(uint8 channel)
{
    if (channel == `$INSTANCE_NAME`_CHANNEL_A)
    {        
        return `$INSTANCE_NAME`_HOLDA24;
    }
    else if (channel == `$INSTANCE_NAME`_CHANNEL_B)
    {      
        return `$INSTANCE_NAME`_HOLDB24;
    }
    else
    {
        return 0;
    }
}

#endif


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Write8
********************************************************************************
* Summary:
*  Set the value in one of the DFB Output Staging Registers 
*
* Parameters:  
*  channel:  Use either `$INSTANCE_NAME`_CHANNEL_A or 
*            `$INSTANCE_NAME`_CHANNEL_B as arguments to the function.  
*  sample:   The 8-bit, right justified input sample. 
*
* Return: 
*  void
*
* Note:
*  Order of the write is important.  On the load of the high byte, the DFB sets
*  the input ready bit.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Write8(uint8 channel, uint8 sample)
{
    if (channel == `$INSTANCE_NAME`_CHANNEL_A)
    {
        `$INSTANCE_NAME`_STAGEAH = sample;
    }
    else if (channel == `$INSTANCE_NAME`_CHANNEL_B)
    {
        `$INSTANCE_NAME`_STAGEBH = sample;
    }
    /* No Final else statement: No value is loaded on bad channel input */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Write16
********************************************************************************
* Summary:
*  Set the value in one of the DFB Output Staging Registers 
*
* Parameters:  
*  channel:  Use either `$INSTANCE_NAME`_CHANNEL_A or 
*            `$INSTANCE_NAME`_CHANNEL_B as arguments to the function.  
*  sample:   The 16-bit, right justified input sample. 
*
* Return: 
*  void
*
* Note:
*  Order of the write is important.  On the load of the high byte, the DFB sets
*  the input ready bit.
*
*******************************************************************************/
#if defined(__C51__) || defined(__CX51__)

void `$INSTANCE_NAME`_Write16(uint8 channel, uint16 sample)
{
    /* Write the STAGE MSB reg last as it signals a complete wrote to the DFB.*/
    if (channel == `$INSTANCE_NAME`_CHANNEL_A)
    {
        `$INSTANCE_NAME`_STAGEAM = (uint8)(sample);
        `$INSTANCE_NAME`_STAGEAH = (uint8)(sample >> 8 );
    }
    else if (channel == `$INSTANCE_NAME`_CHANNEL_B)
    {
        `$INSTANCE_NAME`_STAGEBM = (uint8)(sample);
        `$INSTANCE_NAME`_STAGEBH = (uint8)(sample >> 8);
    }
    /* No Final else statement: No value is loaded on bad channel input */
}

#else

void `$INSTANCE_NAME`_Write16(uint8 channel, uint16 sample)
{
    if (channel == `$INSTANCE_NAME`_CHANNEL_A)
    {
        `$INSTANCE_NAME`_STAGEA16 = sample;
    }
    else if (channel == `$INSTANCE_NAME`_CHANNEL_B)
    {
        `$INSTANCE_NAME`_STAGEB16 = sample;
    }
    /* No Final else statement: No value is loaded on bad channel input */
}

#endif

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Write24
********************************************************************************
* Summary:
*  Set the value in one of the DFB Output Staging Registers 
*
* Parameters:  
*  channel:  Use either `$INSTANCE_NAME`_CHANNEL_A or 
*            `$INSTANCE_NAME`_CHANNEL_B as arguments to the function.  
*  sample:   The 24-bit, right justified input sample inside of a uint32. 
*
* Return: 
*  void
*
* Note:
*  Order of the write is important.  On the load of the high byte, the DFB sets
*  the input ready bit.
*
*******************************************************************************/
#if defined(__C51__) || defined(__CX51__)

void `$INSTANCE_NAME`_Write24(uint8 channel, uint32 sample)
{
    /* Write the STAGE LSB reg last as it signals a complete wrote to the DFB.*/
    if (channel == `$INSTANCE_NAME`_CHANNEL_A)
    {
        `$INSTANCE_NAME`_STAGEA  = (uint8)(sample);
        `$INSTANCE_NAME`_STAGEAM = (uint8)(sample >> 8 );
        `$INSTANCE_NAME`_STAGEAH = (uint8)(sample >> 16);
    }
    else if (channel == `$INSTANCE_NAME`_CHANNEL_B)
    {
        `$INSTANCE_NAME`_STAGEB = (uint8)(sample);
        `$INSTANCE_NAME`_STAGEBM = (uint8)(sample >> 8);
        `$INSTANCE_NAME`_STAGEBH = (uint8)(sample >> 16);
    }
    /* No Final else statement: No value is loaded on bad channel input */
}

#else

void `$INSTANCE_NAME`_Write24(uint8 channel, uint32 sample)
{
    if (channel == `$INSTANCE_NAME`_CHANNEL_A)
    {
        `$INSTANCE_NAME`_STAGEA24 = sample;
    }
    else if (channel == `$INSTANCE_NAME`_CHANNEL_B)
    {
        `$INSTANCE_NAME`_STAGEB24 = sample;
    }
    /* No Final else statement: No value is loaded on bad channel input */
}

#endif


