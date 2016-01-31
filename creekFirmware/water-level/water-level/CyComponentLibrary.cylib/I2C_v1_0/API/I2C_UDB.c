/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*    This file contains the setup, control and status commands for the I2C
*    component.  Actual protocol and operation code resides in the interrupt
*    service routine file.
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
#include <CyLib.h>
#include "`$INSTANCE_NAME`.h"
#include "`$INSTANCE_NAME`_I2C_IRQ.h"




/**********************************
*      System variables
**********************************/

extern uint8   `$INSTANCE_NAME`_State;       /* Current state of I2C state machine */
extern uint8   `$INSTANCE_NAME`_Status;      /* Status byte */

/*** Slave variables ***/
#if (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE)
   #if (`$INSTANCE_NAME`_ADDR_DECODE == `$INSTANCE_NAME`_SW_DECODE)
      extern uint8   `$INSTANCE_NAME`_Address;     /* Software address variable */
   #endif

   /*** Slave Buffer variables ***/
   extern uint8   `$INSTANCE_NAME`_slStatus;      /* Slave Status  */       

   /* Transmit buffer vars */
   extern uint8 * `$INSTANCE_NAME`_readBufPtr;    /* Pointer to Transmit buffer */       
   extern uint8   `$INSTANCE_NAME`_readBufSize;   /* Slave Transmit buffer size */
   extern uint8   `$INSTANCE_NAME`_readBufIndex;  /* Slave Transmit buffer Index */

   /* Receive buffer vars */
   extern uint8 * `$INSTANCE_NAME`_writeBufPtr;    /* Pointer to Receive buffer */       
   extern uint8   `$INSTANCE_NAME`_writeBufSize;   /* Slave Receive buffer size */
   extern uint8   `$INSTANCE_NAME`_writeBufIndex;  /* Slave Receive buffer Index */

#endif

/*** Master Buffer variables ***/
#if (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MASTER)
   extern uint8 * `$INSTANCE_NAME`_mstrRdBufPtr;    /* Pointer to Master Read buffer */       
   extern uint8   `$INSTANCE_NAME`_mstrRdBufSize;   /* Master Read buffer size  */
   extern uint8   `$INSTANCE_NAME`_mstrRdBufIndex;  /* Master Read buffer Index */

   extern uint8 * `$INSTANCE_NAME`_mstrWrBufPtr;    /* Pointer to Master Write buffer */       
   extern uint8   `$INSTANCE_NAME`_mstrWrBufSize;   /* Master Write buffer size  */
   extern uint8   `$INSTANCE_NAME`_mstrWrBufIndex;  /* Master Write buffer Index */

   extern uint8   `$INSTANCE_NAME`_mstrStatus;      /* Master Status byte */
   extern uint8   `$INSTANCE_NAME`_mstrControl;     /* Master Control byte */
#endif


uint8 `$INSTANCE_NAME`_InitVar = 0;
            
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
* Summary:
*  Starts the component and enables the interupt.  
*
* Parameters:  
*  void
*
* Return: 
*  void 
*
* Theory: 
*
* Side Effects:
*   This component automatically enables it's interrupt.  If I2C is enabled
*   without the interrupt enabled, it could lock up the I2C bus.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Start(void) 
{
    uint8 tmp;


    if(`$INSTANCE_NAME`_InitVar == 0)
    {
        /* Set flag that I2C has been initialized */
        `$INSTANCE_NAME`_InitVar = 1;

        /* Reset the I2C before starting. */
        `$INSTANCE_NAME`_CFG &= ~(`$INSTANCE_NAME`_CTRL_ENABLE_MASK);
 
        /* Enable Byte Complete for interrupt in the mask register. */
        `$INSTANCE_NAME`_INT_MASK |= `$INSTANCE_NAME`_BYTE_COMPLETE_MASK;

        /* Clear the status register before starting. */
        tmp = `$INSTANCE_NAME`_CSR;

        #if (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE)
        /* Enable the coutner module. */
        `$INSTANCE_NAME`_COUNTER_AUX_CTL |= `$INSTANCE_NAME`_COUNTER_ENABLE_MASK;
        #endif

        /* Set interrupt vector */
        `$INSTANCE_NAME`_I2C_IRQ_SetVector(`$INSTANCE_NAME`_ISR);

        /* Enable the I2C. */
        `$INSTANCE_NAME`_CFG = `$INSTANCE_NAME`_CTRL_ENABLE_MASK;
 
    #if (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE)
        #if (`$INSTANCE_NAME`_ADDR_DECODE == `$INSTANCE_NAME`_HDWR_DECODE)
            /* Turn off any address match. */
            `$INSTANCE_NAME`_CFG &= ~(`$INSTANCE_NAME`_CTRL_ANY_ADDRESS_MASK);
        #else
            /* Turn on any address match. */
            `$INSTANCE_NAME`_CFG |= `$INSTANCE_NAME`_CTRL_ANY_ADDRESS_MASK;
        #endif
    #endif
    }

    /* Clear all status flags */
    `$INSTANCE_NAME`_Status = 0x00; 

    /* Put state machine in idle state */
    `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_IDLE; 


    #if (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE)
        /* Set default status */
        `$INSTANCE_NAME`_SlaveClearReadBuf();
        `$INSTANCE_NAME`_SlaveClearWriteBuf();
        `$INSTANCE_NAME`_SlaveClearReadStatus();
        `$INSTANCE_NAME`_SlaveClearWriteStatus();

        /* Set default address */
        `$INSTANCE_NAME`_SlaveSetAddress(`$INSTANCE_NAME`_DEFAULT_ADDR);
    #endif
    
    #if ( `$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MASTER )
        /* Set default status */
        `$INSTANCE_NAME`_MasterClearReadBuf();
        `$INSTANCE_NAME`_MasterClearWriteBuf();
        `$INSTANCE_NAME`_MasterClearStatus();

    #endif

}

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
* Summary:
*  Disable the component and disable the interrupt.
*
* Parameters:  
*  void 
*
* Return: 
*  void 
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void) 
{
    /* Disable Interrupt */
    `$INSTANCE_NAME`_I2C_IRQ_Disable();

    /* Reset the I2C before starting. */
    `$INSTANCE_NAME`_CFG &= ~(`$INSTANCE_NAME`_CTRL_ENABLE_MASK);

#if (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE)
    /* Disable the bit counter module. */
    `$INSTANCE_NAME`_COUNTER_AUX_CTL &= ~(`$INSTANCE_NAME`_COUNTER_ENABLE_MASK); 
#endif
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_EnableInt
********************************************************************************
* Summary:
*  Enables the interrupt service routine for the component.  This is normally
*  handled with the start command.
*
* Parameters:  
*  void
*
* Return: 
*  void 
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_EnableInt(void) 
{
    `$INSTANCE_NAME`_INT_ENABLE |= `$INSTANCE_NAME`_INT_ENABLE_MASK;
    `$INSTANCE_NAME`_I2C_IRQ_Enable();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_DisableInt
********************************************************************************
* Summary:
*  This function disables the interrupt service routine.  Normally this 
*  function should never be called, instead use the Stop() function.
*
* Parameters:  
*  void  
*
* Return: 
*  void 
*
* Theory: 
*
* Side Effects:
*   If this function is called during normal operation, it will stop the 
*   I2C function from working and it may lock up the I2C bus.
*
*******************************************************************************/
void `$INSTANCE_NAME`_DisableInt(void) 
{
    `$INSTANCE_NAME`_INT_MASK &= ~(`$INSTANCE_NAME`_INT_ENABLE_MASK);
    `$INSTANCE_NAME`_I2C_IRQ_Disable();
}

