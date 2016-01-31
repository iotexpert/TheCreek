/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file contains the setup, control and status commands for the 
*  GraphicLCDCtrl component.  
*
* Note: 
*
*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

#include "`$INSTANCE_NAME`.h"  

uint8 `$INSTANCE_NAME`_initVar = 0u;


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Enable
********************************************************************************
*
* Summary:
*  Enables the GraphicLCDCtrl interface.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Enable(void)  `=ReentrantKeil($INSTANCE_NAME . "_Enable")`
{
    `$INSTANCE_NAME`_CONTROL_REG |= `$INSTANCE_NAME`_ENABLE;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Init
********************************************************************************
*
* Summary:
*  Inits/Restores default GraphicLCDCtrl configuration provided with customizer
*
* Parameters:
*  None.
*
* Return:
*  None.
* 
*******************************************************************************/
void `$INSTANCE_NAME`_Init(void)    `=ReentrantKeil($INSTANCE_NAME . "_Init")`
{    
    uint8 enableInterrupts;
    
    /* Enter critical section */
    enableInterrupts = CyEnterCriticalSection();
    
    /* Set FIFOs in the Single Buffer Mode */
    `$INSTANCE_NAME`_HORIZ_DP_AUX_CTL_REG |= `$INSTANCE_NAME`_FX_CLEAR; 
    `$INSTANCE_NAME`_VERT_DP_AUX_CTL_REG  |= `$INSTANCE_NAME`_FX_CLEAR; 
    
    /* Exit critical section */
    CyExitCriticalSection(enableInterrupts);
    
    /* Setup the Horizontal Signals */
    /* Sync Width in dotclks. Set one less than period */  
    `$INSTANCE_NAME`_HORIZ_SYNC_REG       = `$INSTANCE_NAME`_HORIZ_SYNC_WIDTH; 
    /* Back Porch in dotclks. Set 6 less than period because the BP is split into
     * two pieces to keep random access from extending into the active period. */
    `$INSTANCE_NAME`_HORIZ_BP_REG         = `$INSTANCE_NAME`_HORIZ_BACK_PORCH;
    /* Active Region in dotclks. Set as (period / 4) - 1. This allows for regions 
     * as large as 1024x1024 while only using 8 bit counters. */
    `$INSTANCE_NAME`_HORIZ_ACT_REG        = `$INSTANCE_NAME`_HORIZ_ACTIVE_REG;
    /* Front Porch in dotclks. Set one less than period */
    `$INSTANCE_NAME`_HORIZ_FP_REG         = `$INSTANCE_NAME`_HORIZ_FRONT_PORCH;
    
    /* Setup the Vertical Signals */
    /* Sync Width in lines. Set one less than period. */ 
    `$INSTANCE_NAME`_VERT_SYNC_REG       = `$INSTANCE_NAME`_VERT_SYNC_WIDTH; 
    /* Back Porch in lines. Set one less than period. */
    `$INSTANCE_NAME`_VERT_BP_REG         = `$INSTANCE_NAME`_VERT_BACK_PORCH;
    /* Active Region in lines. Set as (period / 4) - 1. This allows for regions 
     * as large as 1024x1024 while only using 8 bit counters. */
    `$INSTANCE_NAME`_VERT_ACT_REG        = `$INSTANCE_NAME`_VERT_ACTIVE_REG;
    /* Front Porch in lines. Set one less than period. */
    `$INSTANCE_NAME`_VERT_FP_REG         = `$INSTANCE_NAME`_VERT_FRONT_PORCH;
    
    /* Setup the frame starting address */
    CY_SET_REG24(`$INSTANCE_NAME`_FRAME_BUF_LSB_PTR, `$INSTANCE_NAME`_INIT_FRAME_ADDRESS);

    /* Setup the increment at the end of the line 
    * In order to seperate the X and Y portion of the address a larger addition
    * is made at the end of the line instead of an increment to get to the next
    * power of 2.
    */
    CY_SET_REG24(`$INSTANCE_NAME`_LINE_INCR_LSB_PTR, (`$INSTANCE_NAME`_INIT_LINE_INCR + 1u)); 
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
*
* Summary:
*  Starts the GraphicLCDCtrl interface. Configures the component for operation, 
*  begins generation of clock, timing signals, interrupt, and starts refreshing
*  the screen from the frame buffer. Sets the frame buffer address to 0 and the 
*  number of entries between lines to the line width.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Global Variables:
*  `$INSTANCE_NAME`_initVar - used to check initial configuration, modified on 
*  first function call.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Start(void) `=ReentrantKeil($INSTANCE_NAME . "_Start")`
{
    if (`$INSTANCE_NAME`_initVar == 0u)
    {
        `$INSTANCE_NAME`_Init();
        `$INSTANCE_NAME`_initVar = 1u;
    }
      
    `$INSTANCE_NAME`_Enable();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
*
* Summary:
*  Disables the GraphicLCDCtrl interface.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void)    `=ReentrantKeil($INSTANCE_NAME . "_Stop")`
{    
    `$INSTANCE_NAME`_CONTROL_REG &= ~`$INSTANCE_NAME`_ENABLE; 
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Write
********************************************************************************
*
* Summary:
*  Initiates a write transaction to the frame buffer using the address and data
*  provided. The write is a posted write, so this function will return before
*  the write has actually completed on the interface. If the command queue is 
*  full, this function will not return until space is available to queue this 
*  write request.
*
* Parameters:
*  addr:   Address to be sent on the address lines of the component 
*          (addr2[6:0], addr1[7:0], addr0[7:0]).
*
*  wrData: Data sent on the do_msb[7:0] (most significant byte) 
*          and do_lsb[7:0] (least significant byte) pins.
*
* Return:
*  None.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Write(uint32 addr, uint16 wrData) `=ReentrantKeil($INSTANCE_NAME . "_Write")`
{
    while((`$INSTANCE_NAME`_STATUS_REG & `$INSTANCE_NAME`_CMD_FIFO_FULL) != 0u)
    {
            /* Command queue is full */
    }
    /* The upper byte of the address/cmd must be written last after the lower 
    *  address bits and the data, because block status from this FIFO used as 
    *  indication that there is a valid command in the command FIFO. 
    */
    `$INSTANCE_NAME`_DOUT_LSB_REG = LO8(wrData);
    `$INSTANCE_NAME`_DOUT_MSB_REG = HI8(wrData);
    
    /* The code is depending on CY_SET_REG24 writing from LSB to MSB for proper 
    * operation.
    */
    CY_SET_REG24(`$INSTANCE_NAME`_ADDR_LSB_PTR, (addr & `$INSTANCE_NAME`_WRITE_ADDR_MASK));      
}
    
    
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Read
********************************************************************************
*
* Summary:
*  Initiates a read transaction from the frame buffer.  The read will execute 
*  after all currently posted writes have completed. This function will wait 
*  until the read completes and then returns the read value.
*
* Parameters:
*  addr: Address to be sent on the address lines of the component 
*        (addr2[6:0], addr1[7:0], addr0[7:0]).
* Return:
*  Read value from the di_msb[7:0] (most significant byte) and di_lsb[7:0] 
*  (least significant byte) pins.
*
* Reentrant:
*  No.
*
*******************************************************************************/
uint16 `$INSTANCE_NAME`_Read(uint32 addr) `=ReentrantKeil($INSTANCE_NAME . "_Read")`
{
    while((`$INSTANCE_NAME`_STATUS_REG & `$INSTANCE_NAME`_CMD_FIFO_FULL) != 0u)
    {
        /* Command queue is full */
    }       
  
    /* The upper byte of the address/cmd must be written last after the lower 
    *  address bits, because block status from this FIFO used as indication that 
    *  there is a valid command in the command FIFO. 
    * 
    * The code is depending on CY_SET_REG24 writing from LSB to MSB for proper 
    * operation.
    */  
    CY_SET_REG24(`$INSTANCE_NAME`_ADDR_LSB_PTR, (addr | `$INSTANCE_NAME`_READ_ADDR_MASK));
    
    while((`$INSTANCE_NAME`_STATUS_REG & `$INSTANCE_NAME`_DATA_VALID) == 0u)
    {
        /* wait until input data are valid */    
    }
    
    return (CY_GET_REG16(`$INSTANCE_NAME`_DIN_LSB_PTR));
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_WriteFrameAddr
********************************************************************************
*
* Summary:
*  Sets the starting frame buffer address used when refreshing the screen. This
*  register is read during each vertical blanking interval. To implement an 
*  atomic update of this register it should be written during the active refresh 
*  region.
*
* Parameters:
*  addr: Address to be sent on the address lines of the component 
*        (addr2[6:0], addr1[7:0], addr0[7:0]).
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteFrameAddr(uint32 addr)   `=ReentrantKeil($INSTANCE_NAME . "_WriteFrameAddr")`
{
    CY_SET_REG24(`$INSTANCE_NAME`_FRAME_BUF_LSB_PTR, addr); 
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadFrameAddr
********************************************************************************
*
* Summary:
*  Reads the starting frame buffer address used when refreshing the screen. 
*
* Parameters:
*  None.
*
* Return:
*  Address of the start of the frame buffer.
*
*******************************************************************************/
uint32 `$INSTANCE_NAME`_ReadFrameAddr(void)     `=ReentrantKeil($INSTANCE_NAME . "_ReadFrameAddr")`
{
    return CY_GET_REG24(`$INSTANCE_NAME`_FRAME_BUF_LSB_PTR); 
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_WriteLineIncr
********************************************************************************
*
* Summary:
*  Sets the address spacing between adjacent lines. By default this is the 
*  display size of a line. This setting can be used to align lines to a 
*  different word boundary or to implement a virtual line length that is larger 
*  than the display region.
*
* Parameters:
*  incr: Address increment between lines.  Must be at least the display size of 
*        a line.
* 
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteLineIncr(uint32 incr)    `=ReentrantKeil($INSTANCE_NAME . "_WriteLineIncr")`
{
    CY_SET_REG24(`$INSTANCE_NAME`_LINE_INCR_LSB_PTR, (incr - `$INSTANCE_NAME`_PHYS_LINE_WIDTH + 1u)); 
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadLineIncr
********************************************************************************
*
* Summary:
*  Reads the address increment between lines. 
*
* Parameters:
*  None.
*
* Return:
*  Address increment between lines.
*
*******************************************************************************/
uint32 `$INSTANCE_NAME`_ReadLineIncr(void)      `=ReentrantKeil($INSTANCE_NAME . "_ReadLineIncr")`
{
    return (CY_GET_REG24(`$INSTANCE_NAME`_LINE_INCR_LSB_PTR) + `$INSTANCE_NAME`_PHYS_LINE_WIDTH - 1u); 
}


/* [] END OF FILE */
