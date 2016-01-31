/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the source code to the API for the EMIF component
*
* Note:
*  None
*
********************************************************************************
* Copyright 2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"

uint8 `$INSTANCE_NAME`_initVar = 0u;


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Init
********************************************************************************
*
* Summary:
*  Initializes the EMIF configuration to the current customizer state
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_Init")`
{
    /* Asynchronous External Memory EMIF Config */
    #if (`$INSTANCE_NAME`_MODE == `$INSTANCE_NAME`_ASYNCH)
        `$INSTANCE_NAME`_NO_UDB_REG = `$INSTANCE_NAME`_MODE_NOUDB;
        `$INSTANCE_NAME`_MEM_TYPE_REG = `$INSTANCE_NAME`_MEM_ASYNC;
        
        #if (`$INSTANCE_NAME`_VDDD <= `$INSTANCE_NAME`_VDD_LOW)
            `$INSTANCE_NAME`_RD_WAIT_STATE_REG = `$INSTANCE_NAME`_READ_WTSTATES + `$INSTANCE_NAME`_WAIT_STATE;
            `$INSTANCE_NAME`_WR_WAIT_STATE_REG = `$INSTANCE_NAME`_WRITE_WTSTATES + `$INSTANCE_NAME`_WAIT_STATE;   
        #else
            `$INSTANCE_NAME`_RD_WAIT_STATE_REG = `$INSTANCE_NAME`_READ_WTSTATES;
            `$INSTANCE_NAME`_WR_WAIT_STATE_REG = `$INSTANCE_NAME`_WRITE_WTSTATES;   
        #endif /* End `$INSTANCE_NAME`_VDDD <= `$INSTANCE_NAME`_VDD_LOW */
    
    /* Synchronous External Memory EMIF Config */
    #elif (`$INSTANCE_NAME`_MODE == `$INSTANCE_NAME`_SYNCH)
        `$INSTANCE_NAME`_NO_UDB_REG = `$INSTANCE_NAME`_MODE_NOUDB;
        `$INSTANCE_NAME`_MEM_PWR_REG = `$INSTANCE_NAME`_DEFAULT_STATE;
        `$INSTANCE_NAME`_MEM_TYPE_REG = `$INSTANCE_NAME`_MEM_SYNC;
        
        #if (`$INSTANCE_NAME`_VDDD <= `$INSTANCE_NAME`_VDD_LOW)
            `$INSTANCE_NAME`_RD_WAIT_STATE_REG = `$INSTANCE_NAME`_READ_WTSTATES + `$INSTANCE_NAME`_WAIT_STATE;
            `$INSTANCE_NAME`_WR_WAIT_STATE_REG = `$INSTANCE_NAME`_WRITE_WTSTATES + `$INSTANCE_NAME`_WAIT_STATE;   
        #else
            `$INSTANCE_NAME`_RD_WAIT_STATE_REG = `$INSTANCE_NAME`_READ_WTSTATES;
            `$INSTANCE_NAME`_WR_WAIT_STATE_REG = `$INSTANCE_NAME`_WRITE_WTSTATES;   
        #endif /* End `$INSTANCE_NAME`_VDDD <= `$INSTANCE_NAME`_VDD_LOW */
    
    /* Custom External Memory EMIF Config */
    #else
        `$INSTANCE_NAME`_NO_UDB_REG = `$INSTANCE_NAME`_MODE_UDB;
    #endif /* End of `$INSTANCE_NAME`_MODE configuration */


    /* EMIF clock divider */
   `$INSTANCE_NAME`_CLK_DIV_REG = `$INSTANCE_NAME`_CLOCK_DIV;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Enable
********************************************************************************
*
* Summary:
*  Enables the EMIF hardware block, associated IO ports and pins, and for 
*  custom mode associated UDB logic
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`
{
    uint8 enableInterrupts = 0u;
    
    /* Change Power regs, need to be safety */
    enableInterrupts = CyEnterCriticalSection();
    
    `$INSTANCE_NAME`_POWER_REG |= `$INSTANCE_NAME`_POWER_ON;
    `$INSTANCE_NAME`_STBY_REG  |= `$INSTANCE_NAME`_POWER_ON;
                
    /* Power regs config are done */
    CyExitCriticalSection(enableInterrupts);

    /* Enable Clock tp EMIF */
    `$INSTANCE_NAME`_ENABLE_REG |= `$INSTANCE_NAME`_ENABLE;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
*
* Summary:
*  Initializes the EMIF configuration to the current customizer state.
*  Enables the EMIF hardware block, associated IO ports and pins
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global Variables:
*  `$INSTANCE_NAME`_initVar - used to check initial configuration, modified on 
*  first function call.
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_Start(void)
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
*  Disables the EMIF block and associated UDB logic (custom mode). 
*  Returns all associated IO ports and pins to Hi-Z mode.  
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`
{
    uint8 enableInterrupts = 0u;
    
    /* Stop EMIF Clock */    
    `$INSTANCE_NAME`_ENABLE_REG = `$INSTANCE_NAME`_DEFAULT_STATE;

    /* Change Power regs, need to be safety */
    enableInterrupts = CyEnterCriticalSection();

    `$INSTANCE_NAME`_POWER_REG &= ~`$INSTANCE_NAME`_POWER_ON;
    `$INSTANCE_NAME`_STBY_REG  &= ~`$INSTANCE_NAME`_POWER_ON;
                
    /* Power regs config are done */
    CyExitCriticalSection(enableInterrupts);
    
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ExtMemSleep
********************************************************************************
*
* Summary:
*  Sets the ‘mem_pd’ bit in the EMIF_PWR_DWN register. This sets the external 
*  memory sleep signal high; note that depending on the type of external memory 
*  IC used the signal may need to be inverted.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_ExtMemSleep(void) `=ReentrantKeil($INSTANCE_NAME . "_ExtMemSleep")`
{
    #if (`$INSTANCE_NAME`_MODE == `$INSTANCE_NAME`_SYNCH)
        `$INSTANCE_NAME`_MEM_PWR_REG = `$INSTANCE_NAME`_MEM_PWR_DOWN;
    #endif /* End `$INSTANCE_NAME`_MODE == `$INSTANCE_NAME`_SYNCH */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ExtMemWakeup
********************************************************************************
*
* Summary:
*  Resets the ‘mem_pd’ bit in the EMIF_PWR_DWN register. This sets the 
*  external memory sleep signal low; note that depending on the type of 
*  external memory IC used the signal may need to be inverted.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/

void `$INSTANCE_NAME`_ExtMemWakeup(void) `=ReentrantKeil($INSTANCE_NAME . "_ExtMemWakeup")`
{
    #if (`$INSTANCE_NAME`_MODE == `$INSTANCE_NAME`_SYNCH)
        `$INSTANCE_NAME`_MEM_PWR_REG = `$INSTANCE_NAME`_DEFAULT_STATE;
    #endif /* End `$INSTANCE_NAME`_MODE == `$INSTANCE_NAME`_SYNCH */
}

/* [] END OF FILE */
