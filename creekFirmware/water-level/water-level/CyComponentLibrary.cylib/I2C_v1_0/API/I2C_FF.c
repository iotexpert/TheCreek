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


#undef CyDelay

#define CyDelay(x)



/**********************************
*      System variables
**********************************/


extern uint8   `$INSTANCE_NAME`_State;       /* Current state of I2C state machine */
extern uint8   `$INSTANCE_NAME`_Status;      /* Status byte */

/*** Slave variables ***/
#if ( `$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE )
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
#if ( `$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MASTER )
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
    uint8 clkSel;

    if(`$INSTANCE_NAME`_InitVar == 0)
    {
        `$INSTANCE_NAME`_InitVar = 1;          /* Set flag that I2C has been initialized */

        /* Component function code  */
        /* Configure registers */
        /* Set the clock divider to the defined value */
        /* Find the first divider that is >= to the desired divider */
        /* The divider is in a 1,2,4,8,16,32,64 sequence */
        /* Default clock divider will be 16 (4) */
        /* The mode will be set sample at 16x and a prescaler of /4 */
        `$INSTANCE_NAME`_CLKDIV  = `$INSTANCE_NAME`_CLK_DIV_16;

        for(clkSel = 0; clkSel <= 6; clkSel++ )
        {
            if((1 << clkSel) >= `$INSTANCE_NAME`_DEFAULT_CLKDIV)
            {
                `$INSTANCE_NAME`_CLKDIV  = clkSel;
                break;
            }
        }


        /* Set interrupt vector */
        `$INSTANCE_NAME`_I2C_IRQ_SetVector( `$INSTANCE_NAME`_ISR );

        /* Clear Status register */
        `$INSTANCE_NAME`_CSR  = 0x00;

        /* Set Configuration, slave, rate, IE on stop and bus error, Pins **CHECK** */
        /* Set the prescaler to the 400k bps mode, which is really a divide by 4.  This will give the */
        /* Clock divider sufficient range to work bewteen 50 and 400 kbps.   */
        `$INSTANCE_NAME`_CFG  = `$INSTANCE_NAME`_CFG_CLK_RATE_400 | `$INSTANCE_NAME`_ENABLE_SLAVE | `$INSTANCE_NAME`_ENABLE_MASTER;

        /* Turn on hardware address detection and enable the clock */
        #if (`$INSTANCE_NAME`_ADDR_DECODE == `$INSTANCE_NAME`_HDWR_DECODE )
            `$INSTANCE_NAME`_XCFG  = `$INSTANCE_NAME`_XCFG_HDWR_ADDR_EN | `$INSTANCE_NAME`_XCFG_CLK_EN;
        #else
            `$INSTANCE_NAME`_XCFG  = `$INSTANCE_NAME`_XCFG_CLK_EN;
            `$INSTANCE_NAME`_ADDR  = `$INSTANCE_NAME`_DEFAULT_ADDR;
        #endif
    }

    /* Clear all status flags */
    `$INSTANCE_NAME`_Status = 0x00; 

    /* Put state machine in idle state */
    `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_IDLE; 

    /* Enable power to I2C Module */
    `$INSTANCE_NAME`_PWRMGR |= `$INSTANCE_NAME`_ACT_PWR_EN;

    #if ( `$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE )
        /* Set default status */
        `$INSTANCE_NAME`_SlaveClearReadBuf();
          `$INSTANCE_NAME`_SlaveClearWriteBuf();
          `$INSTANCE_NAME`_SlaveClearReadStatus();
          `$INSTANCE_NAME`_SlaveClearWriteStatus();

        /* Set default address */
        `$INSTANCE_NAME`_SlaveSetAddress( `$INSTANCE_NAME`_DEFAULT_ADDR );
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
    `$INSTANCE_NAME`_I2C_IRQ_Disable( );

    /* Disable power to I2C Module */
    `$INSTANCE_NAME`_PWRMGR &= ~`$INSTANCE_NAME`_ACT_PWR_EN;
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
    `$INSTANCE_NAME`_I2C_IRQ_Enable( );
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
   `$INSTANCE_NAME`_I2C_IRQ_Disable( );
}


