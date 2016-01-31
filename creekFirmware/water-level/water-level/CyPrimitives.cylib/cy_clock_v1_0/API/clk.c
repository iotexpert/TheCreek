/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*    API for controlling a clock.
*
*    A clock has an input source and a divider.  These are assigned 
*    by CyDesigner.  Durring code generation, CyDesigner creates
*    the necessary defines for manipulating the registers associated
*    with the assigned clock.  This API uses those register definitions
*    to control the clock.  
*
*   Note:
*
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/


#include <CYDEVICE.H>
#include "`$INSTANCE_NAME`.H"







/* Clock Distribution registers. */
#define CLK_DIST_CR                  ((reg8 *) CYDEV_CLKDIST_CR)
#define CLK_DIST_LD                  ((reg8 *) CYDEV_CLKDIST_LD)

#define CLK_DIST_BCFG0               ((reg8 *) CYDEV_CLKDIST_BCFG0)
#define CLK_DIST_BCFG1               ((reg8 *) CYDEV_CLKDIST_BCFG1)
#define CLK_DIST_BCFG2               ((reg8 *) CYDEV_CLKDIST_BCFG2)

#define BCFG2_SHADOW_MASK            (0x80)

#define CLK_DIST_WRK0                ((reg8 *) CYDEV_CLKDIST_WRK0)
#define CLK_DIST_WRK1                ((reg8 *) CYDEV_CLKDIST_WRK1)

#define CLK_DIST_DMASK               ((reg8 *) CYDEV_CLKDIST_DMASK)
#define CLK_DIST_AMASK               ((reg8 *) CYDEV_CLKDIST_AMASK)


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
* Summary:
*   Enables the clock by setting the enable bit in the 'Active Power Mode
*   Configuration Register' corresponding to this clock.
*
*
* Parameters:
*   void.
*
* Return:
*   void.
*   
*
*******************************************************************************/
void `$INSTANCE_NAME`_Start(void)
{
    /* Set the bit to enable the clock. */
    *`$INSTANCE_NAME`_CLKEN |= `$INSTANCE_NAME`_CLKEN_MASK;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
* Summary:
*   Disables the clock by clearing the enable bit in the 'Active Power Mode
*   Configuration Register' corresponding to this clock.
*
*   if the clock is disabled, the output will be held at logic 0 when the
*   signal transitions to a logic 0.
*
* Parameters:
*   void.
*
*
* Return:
*   void.
*   
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void)
{
    /* Clear the bit to enable the clock. */
    *`$INSTANCE_NAME`_CLKEN &= ~`$INSTANCE_NAME`_CLKEN_MASK;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_StandbyPower
********************************************************************************
* Summary:
*   Selects the power state for this clock when in standby power mode.
*
* Parameters:
*   state:
*       state of this clock during standby power mode. 1 is active, 0 inactive.
*
*   
* Return:
*   void.
*
*
*******************************************************************************/
void `$INSTANCE_NAME`_StandbyPower(uint8 state)
{
    if(state == 0)
    {
        *`$INSTANCE_NAME`_CLKSTBY &= ~`$INSTANCE_NAME`_CLKSTBY_MASK;
    }
    else
    {
        *`$INSTANCE_NAME`_CLKSTBY |= `$INSTANCE_NAME`_CLKSTBY_MASK;
    }
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetDivider
********************************************************************************
* Summary:
*   Sets the divider of the clock.
*
*
* Parameters:
* clkDivider:
*   Value to device the input clock by.
*
*
* Return:
*   void.
*   
*
* Theory:
*   Any value other than zero is acceptable.
*
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetDivider(uint16 clkDivider)
{
    // Clear all the mask bits except ours.
#if defined(`$INSTANCE_NAME`__CFG3)
    *CLK_DIST_AMASK = `$INSTANCE_NAME`_CLKEN_MASK;
    *CLK_DIST_DMASK = 0;
#else
    *CLK_DIST_DMASK = `$INSTANCE_NAME`_CLKEN_MASK;
    *CLK_DIST_AMASK = 0;
#endif

    /* Set the mask bit to enable shadow loads. */
    *CLK_DIST_BCFG2 |= BCFG2_SHADOW_MASK;

    *CLK_DIST_WRK0 = LO8(clkDivider);
    *CLK_DIST_WRK1 = HI8(clkDivider);

    /* Load mask and restart together. */
    *CLK_DIST_LD = 3;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetMode
********************************************************************************
* Summary:
*   Sets the operating mode of the clock.
*
*
* Parameters:
* clkMode: The following optional bits or'ed together.
*
*   CYCLK_PIPE 
*   CYCLK_SSS  
*   CYCLK_EARLY
*   CYCLK_DUTY 
*   CYCLK_SYNC 
*
* Return:
*   void.
*   
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetMode(uint8 clkMode)
{
    uint8 value;


    /* Get the mode and source for the clock. */
    value = *`$INSTANCE_NAME`_MOD_SRC;

    /* Clear out the mode. */
    value &= 0x07u;

    /* Write the new mode. */
    value |= clkMode;

    /* Set the new mode of the clock. */
    *`$INSTANCE_NAME`_MOD_SRC = value;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetSource
********************************************************************************
* Summary:
*   Sets the input source of the clock.
*
* Parameters:
* clkSource:
*   One of the following input sources.
*
*   CYCLK_SRC_SEL_SYNC_DIG
*   CYCLK_SRC_SEL_IMO  
*   CYCLK_SRC_SEL_XTALM
*   CYCLK_SRC_SEL_ILO  
*   CYCLK_SRC_SEL_PLL  
*   CYCLK_SRC_SEL_XTALK
*   CYCLK_SRC_SEL_DSI_G
*   CYCLK_SRC_SEL_DSI_D
*
* Return:
*   void.
*   
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetSource(uint8 clkSource)
{
    uint8 value;


    /* Get the mode and source for the clock. */
    value = *`$INSTANCE_NAME`_MOD_SRC;

    /* Clear out the source. */
    value &= 0xF8u;

    /* Write the new source. */
    value |= clkSource;

    /* Set the new mode of the clock. */
    *`$INSTANCE_NAME`_MOD_SRC = value;
}


#if defined(`$INSTANCE_NAME`__CFG3)

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetPhase
********************************************************************************
* Summary:
*   Sets the phase delay of the analog clock.
*
*   0x01      2.5ns delay
*   0x02      3.5ns delay
*
*   ...
*
*   0x0a      11.5ns delay
*   0x0b      12.5ns delay
*
*
* Parameters:
* clkPhase: Amount to delay the phase of the clock, in 1.0 ns increments
*           up to 0x0B (12.5ns).
*
* Return:
*   void.
*   
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetPhase(uint8 clkPhase)
{

    *`$INSTANCE_NAME`_PHASE = 0x0Fu & clkPhase;

}

#endif





