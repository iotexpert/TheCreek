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
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

#include "`$INSTANCE_NAME`.h"  


/**********************************
*      System variables
**********************************/

uint8 `$INSTANCE_NAME`_initVar = 0u;
extern volatile uint8 `$INSTANCE_NAME`_State;       /* Current state of I2C state machine */
extern volatile uint8 `$INSTANCE_NAME`_Status;      /* Status byte */

/* Master variables */
#if (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MASTER)
   extern volatile uint8 `$INSTANCE_NAME`_mstrStatus;       /* Master Status byte */
   extern volatile uint8 `$INSTANCE_NAME`_mstrControl;      /* Master Control byte */
   
   /* Transmit buffer variables */
   extern uint8 * `$INSTANCE_NAME`_mstrRdBufPtr;            /* Pointer to Master Read buffer */       
   extern volatile uint8 `$INSTANCE_NAME`_mstrRdBufSize;    /* Master Read buffer size */
   extern volatile uint8 `$INSTANCE_NAME`_mstrRdBufIndex;   /* Master Read buffer Index */
    
   /* Receive buffer variables */
   extern uint8 * `$INSTANCE_NAME`_mstrWrBufPtr;            /* Pointer to Master Write buffer */       
   extern volatile uint8 `$INSTANCE_NAME`_mstrWrBufSize;    /* Master Write buffer size */
   extern volatile uint8 `$INSTANCE_NAME`_mstrWrBufIndex;   /* Master Write buffer Index */
   
#endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MASTER) */

/* Slave variables */
#if (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE)
   extern volatile uint8 `$INSTANCE_NAME`_slStatus;         /* Slave Status  */   
   
   #if (`$INSTANCE_NAME`_ADDR_DECODE == `$INSTANCE_NAME`_SW_DECODE)
      extern volatile uint8 `$INSTANCE_NAME`_Address;       /* Software address variable */
   #endif   /* End (`$INSTANCE_NAME`_ADDR_DECODE == `$INSTANCE_NAME`_SW_DECODE) */    
   
   /* Transmit buffer variables */
   extern uint8 * `$INSTANCE_NAME`_readBufPtr;              /* Pointer to Transmit buffer */       
   extern volatile uint8 `$INSTANCE_NAME`_readBufSize;      /* Slave Transmit buffer size */
   extern volatile uint8 `$INSTANCE_NAME`_readBufIndex;     /* Slave Transmit buffer Index */

   /* Receive buffer variables */
   extern uint8 * `$INSTANCE_NAME`_writeBufPtr;             /* Pointer to Receive buffer */       
   extern volatile uint8 `$INSTANCE_NAME`_writeBufSize;     /* Slave Receive buffer size */
   extern volatile uint8 `$INSTANCE_NAME`_writeBufIndex;    /* Slave Receive buffer Index */

#endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE) */


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Init
********************************************************************************
*
* Summary:
*  Initializes I2C component with initial values.
*
* Parameters:  
*  void
*
* Return: 
*  void
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_Init(void)
{
    #if (`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`_FF)           
        
        /* Enable Master or Slave */
        `$INSTANCE_NAME`_CFG_REG = (`$INSTANCE_NAME`_ENABLE_SLAVE | `$INSTANCE_NAME`_ENABLE_MASTER);
                                
        /* 50 kHz - 32 samples/bit */
        `$INSTANCE_NAME`_CFG_REG |= `$INSTANCE_NAME`_DEFAULT_CLK_RATE;
         
        /* Set devide factor */
        #if (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
            `$INSTANCE_NAME`_CLKDIV_REG = `$INSTANCE_NAME`_DEFAULT_DIVIDE_FACTOR;
        #else
            `$INSTANCE_NAME`_CLKDIV1_REG = LO8(`$INSTANCE_NAME`_DEFAULT_DIVIDE_FACTOR);
            `$INSTANCE_NAME`_CLKDIV2_REG = HI8(`$INSTANCE_NAME`_DEFAULT_DIVIDE_FACTOR);
        #endif /* End (I2C_PSOC3_ES2 || I2C_PSOC5_ES1) */
        
        /* if I2C block will be used as wake up source */
        #if (`$INSTANCE_NAME`_ENABLE_WAKEUP)
            /* I2C block as wake-up source */
            `$INSTANCE_NAME`_XCFG_REG  = `$INSTANCE_NAME`_XCFG_I2C_ON;
        
            /* Process sio_select and pselect */                
            #if(`$INSTANCE_NAME`_I2C_PAIR_SELECTED == `$INSTANCE_NAME`_I2C_PAIR0)
                /* Set I2C0 SIO pair P12[0,1] */
                `$INSTANCE_NAME`_CFG_REG |= `$INSTANCE_NAME`_CFG_SIO_SELECT;
            #else
                /* Do nothing for I2C1 SIO pair P12[4,5] */
            #endif /* End (`$INSTANCE_NAME`_ENABLE_WAKEUP) */
            
            `$INSTANCE_NAME`_CFG_REG |= `$INSTANCE_NAME`_CFG_PSELECT;
        
        #endif  /* End ((`$INSTANCE_NAME`_ENABLE_WAKEUP)*/
        
        /* Clear Status register */
        `$INSTANCE_NAME`_CSR_REG = 0x00u;
        
    #else
        /* Enable Byte Complete for interrupt in the mask register */
        `$INSTANCE_NAME`_INT_MASK_REG |= `$INSTANCE_NAME`_BYTE_COMPLETE_IE_MASK;

        /* Clear the status register before starting */
        `$INSTANCE_NAME`_initVar = `$INSTANCE_NAME`_CSR_REG;
        
    #endif  /* End (`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`_FF) */

    /* Set address detection type */
    #if (`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`_FF)
        #if (`$INSTANCE_NAME`_ADDR_DECODE == `$INSTANCE_NAME`_HDWR_DECODE)
            /* Turn on hardware address detection and enable the clock */
            `$INSTANCE_NAME`_XCFG_REG  |= (`$INSTANCE_NAME`_XCFG_HDWR_ADDR_EN | `$INSTANCE_NAME`_XCFG_CLK_EN);
            
        #else
            /* Enable the clock */
            `$INSTANCE_NAME`_XCFG_REG  |= `$INSTANCE_NAME`_XCFG_CLK_EN;
                            
        #endif  /* End (`$INSTANCE_NAME`_ADDR_DECODE == `$INSTANCE_NAME`_HDWR_DECODE) */
        
    #else
        #if (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE)
            #if (`$INSTANCE_NAME`_ADDR_DECODE == `$INSTANCE_NAME`_HDWR_DECODE)
                /* Turn off any address match */
                `$INSTANCE_NAME`_CFG_REG &= ~(`$INSTANCE_NAME`_CTRL_ANY_ADDRESS_MASK);
                
            #else
                /* Turn on any address match */
                `$INSTANCE_NAME`_CFG_REG |= `$INSTANCE_NAME`_CTRL_ANY_ADDRESS_MASK;
                
            #endif  /* End (`$INSTANCE_NAME`_ADDR_DECODE == `$INSTANCE_NAME`_HDWR_DECODE) */

        #endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE) */
    
    #endif  /* End (`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`_FF) */
              
    /* Disable Interrupt */
    CyIntDisable(`$INSTANCE_NAME`_ISR_NUMBER);
    
    /* Set the ISR to point to the RTC_SUT_isr Interrupt */
    CyIntSetVector(`$INSTANCE_NAME`_ISR_NUMBER, `$INSTANCE_NAME`_ISR);
    
    /* Set the priority */
    CyIntSetPriority(`$INSTANCE_NAME`_ISR_NUMBER, `$INSTANCE_NAME`_ISR_PRIORITY);
    
    /* Clear all status flags */
    `$INSTANCE_NAME`_Status = 0x00u; 

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
    
    #endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE) */

    #if (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MASTER)
    
        /* Set default status */
        `$INSTANCE_NAME`_MasterClearReadBuf();
        `$INSTANCE_NAME`_MasterClearWriteBuf();
        `$INSTANCE_NAME`_MasterClearStatus();
        
    #endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MASTER) */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Enable
********************************************************************************
*
* Summary:
*  Starts I2C component operation.
*
* Parameters:
*  void
*
* Return:
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`
{          
    #if ( (`$INSTANCE_NAME`_IMPLEMENTATION != `$INSTANCE_NAME`_UDB) || \
          (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE) )
        uint8 enableInterrupts;
    #endif  /* End ( (`$INSTANCE_NAME`_IMPLEMENTATION != `$INSTANCE_NAME`_FF) || \
                   (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE) ) */
                   
    #if(`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`_FF)
        enableInterrupts = CyEnterCriticalSection();
        /* Enable power to I2C Module */
        `$INSTANCE_NAME`_ACT_PWRMGR_REG  |= `$INSTANCE_NAME`_ACT_PWR_EN;
        `$INSTANCE_NAME`_STBY_PWRMGR_REG |= `$INSTANCE_NAME`_STBY_PWR_EN;
        CyExitCriticalSection(enableInterrupts);
        
    #else
        /* Enable the I2C */
        `$INSTANCE_NAME`_CFG_REG = (`$INSTANCE_NAME`_ENABLE_MASTER | `$INSTANCE_NAME`_ENABLE_SLAVE);
        
        /* Enable bit counter */
        #if (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE)
            enableInterrupts = CyEnterCriticalSection();
            `$INSTANCE_NAME`_COUNTER_AUX_CTL_REG |= `$INSTANCE_NAME`_COUNTER_ENABLE_MASK;
            CyExitCriticalSection(enableInterrupts);
        #endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE) */
        
    #endif  /* End (`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`_FF) */   
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
*
* Summary:
*  Starts the component and enables the interupt.
*
* Parameters:
*  void
*
* Return:
*  void
*
* Side Effects:
*   This component automatically enables it's interrupt.  If I2C is enabled
*   without the interrupt enabled, it could lock up the I2C bus.
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_Start(void)
{
    /* Initialize I2C registers, reset I2C buffer index and clears status */
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
*  Disable the component and disable the interrupt.
*
* Parameters:
*  void 
*
* Return: 
*  void 
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")` 
{   
    #if ( (`$INSTANCE_NAME`_IMPLEMENTATION != `$INSTANCE_NAME`_UDB) || \
          (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE) )
        uint8 enableInterrupts;
    #endif  /* End ( (`$INSTANCE_NAME`_IMPLEMENTATION != `$INSTANCE_NAME`_FF) || \
                   (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE) ) */
    
    /* Disable Interrupt */
    `$INSTANCE_NAME`_DisableInt();
    
    #if (`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`_FF)
        enableInterrupts = CyEnterCriticalSection();
        /* Disable power to I2C block */
        `$INSTANCE_NAME`_ACT_PWRMGR_REG  &= ~`$INSTANCE_NAME`_ACT_PWR_EN;
        `$INSTANCE_NAME`_STBY_PWRMGR_REG &= ~`$INSTANCE_NAME`_STBY_PWR_EN;
        CyExitCriticalSection(enableInterrupts);
        
    #else
        /* Reset the I2C before starting */
        `$INSTANCE_NAME`_CFG_REG &= ~(`$INSTANCE_NAME`_ENABLE_MASTER | `$INSTANCE_NAME`_ENABLE_SLAVE);
        
        /* Disable bit counter */
        #if (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE)
            enableInterrupts = CyEnterCriticalSection();
            `$INSTANCE_NAME`_COUNTER_AUX_CTL_REG &= ~`$INSTANCE_NAME`_COUNTER_ENABLE_MASK;
            CyExitCriticalSection(enableInterrupts);
	    #endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE) */
        
    #endif  /* End (`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`_FF) */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_EnableInt
********************************************************************************
*
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
*******************************************************************************/
void `$INSTANCE_NAME`_EnableInt(void) `=ReentrantKeil($INSTANCE_NAME . "_EnableInt")`  
{
    #if (`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`_UDB)
        `$INSTANCE_NAME`_INT_ENABLE_REG |= `$INSTANCE_NAME`_INT_ENABLE_MASK;
    #endif  /* End (`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`_UDB) */
    
    CyIntEnable(`$INSTANCE_NAME`_ISR_NUMBER);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_DisableInt
********************************************************************************
*
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
* Side Effects:
*   If this function is called during normal operation, it will stop the
*   I2C function from working and it may lock up the I2C bus.
*
*******************************************************************************/
void `$INSTANCE_NAME`_DisableInt(void) `=ReentrantKeil($INSTANCE_NAME . "_DisableInt")`  
{
    #if (`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`_UDB)
        `$INSTANCE_NAME`_INT_ENABLE_REG |= `$INSTANCE_NAME`_INT_ENABLE_MASK;
    #endif  /* End (`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`_UDB) */
    
    CyIntDisable(`$INSTANCE_NAME`_ISR_NUMBER);
}


#if (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MASTER)

    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_MasterStatus
    ********************************************************************************
    *
    * Summary:
    *  Returns status of the I2C Master.
    *
    * Parameters:
    *  void
    *
    * Return:
    *  Returns master status register.
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_MasterStatus(void) `=ReentrantKeil($INSTANCE_NAME . "_MasterStatus")`
    {
        if(`$INSTANCE_NAME`_State == `$INSTANCE_NAME`_SM_SL_WR_IDLE)
        {
            return(`$INSTANCE_NAME`_mstrStatus);
        }
        else
        {
            return(`$INSTANCE_NAME`_mstrStatus | `$INSTANCE_NAME`_MSTAT_XFER_INP);
        }
    }
    
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_MasterClearStatus
    ********************************************************************************
    *
    * Summary:
    *  Clears master status flags.
    *
    * Parameters:
    *  void
    *
    * Return: 
    *  Returns the read status.
    *
    * Reentrant:
    *  No
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_MasterClearStatus(void)
    {
        uint8 status;
        
        status = `$INSTANCE_NAME`_mstrStatus ; 
        `$INSTANCE_NAME`_mstrStatus  = `$INSTANCE_NAME`_MSTAT_CLEAR; 
    
        return (status);
    }    
    
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_MasterWriteBuf
    ********************************************************************************
    *
    * Summary:
    *  This function initiates a write transaction with an addressed slave.  Writes
    *  one or more bytes (cnt) to the slave I2C device and gets the data from RAM 
    *  or ROM array pointed to by the array pointer.  Once this routine is called, 
    *  the included ISR will handle further data in byte by byte mode.  
    *
    * Parameters:
    *  slaveAddr: 7-bit slave address
    *  xferData:  Pointer to data in array.
    *  cnt:       Count of data to write.
    *  mode:      Mode of operation.  It defines normal start, restart,
    *             stop, no-stop, etc.
    *
    * Return:
    *  void
    *
    * Reentrant:
    *  No
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_MasterWriteBuf(uint8 slaveAddr, uint8 * xferData, uint8 cnt, uint8 mode)
    {
        uint8 errStatus = `$INSTANCE_NAME`_MSTR_SLAVE_BUSY; 
        
        /* Check if I2C in proper state to generate Start/ReStart condition */
        if((`$INSTANCE_NAME`_State == `$INSTANCE_NAME`_SM_IDLE) || \
           (`$INSTANCE_NAME`_State == `$INSTANCE_NAME`_SM_MSTR_HALT))
        {
            /* If IDLE, check if bus is free */
            if(`$INSTANCE_NAME`_State != `$INSTANCE_NAME`_SM_MSTR_HALT)
            {
                /* If Bus is free proceed, no exist with timeout */
                if(`$INSTANCE_NAME`_CHECK_BUS_FREE(`$INSTANCE_NAME`_MCSR_REG))
                {
                    errStatus = `$INSTANCE_NAME`_MSTR_NO_ERROR;
                }
                else
                {
                    errStatus = `$INSTANCE_NAME`_MSTR_BUS_BUSY;
                }
            }
            else   /* Bus halted waiting for restart */
            {
                errStatus = `$INSTANCE_NAME`_MSTR_NO_ERROR;
                `$INSTANCE_NAME`_mstrStatus &= ~`$INSTANCE_NAME`_MSTAT_XFER_HALT; 
                CyIntClearPending(`$INSTANCE_NAME`_ISR_NUMBER);
            }
    
            /* If no timeout error, generate start */
            if(errStatus == `$INSTANCE_NAME`_MSTR_NO_ERROR)
            {
                /* Determine whether or not to automatically generate a stop condition */
                if((mode & `$INSTANCE_NAME`_MODE_NO_STOP) != 0u)
                {
                    /* Do not generate a Stop at the end of transfer */
                    `$INSTANCE_NAME`_mstrControl |= `$INSTANCE_NAME`_MSTR_NO_STOP;
                }
                else  /* Generate a Stop */
                {
                    `$INSTANCE_NAME`_mstrControl &= ~`$INSTANCE_NAME`_MSTR_NO_STOP;
                }
        
                slaveAddr = (slaveAddr << 1u);
                `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_MSTR_WR_ADDR;
    
                `$INSTANCE_NAME`_mstrWrBufPtr   = xferData; /* Set buffer pointer */
                `$INSTANCE_NAME`_mstrWrBufIndex = 0u;       /* Start buffer at zero */
                `$INSTANCE_NAME`_mstrWrBufSize  = cnt;      /* Set buffer size */
    
                `$INSTANCE_NAME`_DATA_REG = slaveAddr;          /* Write address to data reg */
    
                /* Generate a Start or ReStart depending on flag passed */
                if((mode & `$INSTANCE_NAME`_MODE_REPEAT_START) != 0u)
                {
                    `$INSTANCE_NAME`_GENERATE_RESTART;  /* Generate a ReStart */
                }
                else
                {
                    `$INSTANCE_NAME`_GENERATE_START;    /* Generate a Start */
                }
                
                /* Enable interrupts to process transfer */
                `$INSTANCE_NAME`_EnableInt();
    
                /* Clear write complete flag */
                `$INSTANCE_NAME`_mstrStatus &= ~`$INSTANCE_NAME`_MSTAT_WR_CMPLT; 
            }
        }
   
        return (errStatus);
    }
    
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_MasterReadBuf
    ********************************************************************************
    *
    * Summary:
    *   The function intiates a read transaction with an addressed slave.  Reads
    *   one or more bytes (cnt) from the slave I2C device and writes data to the
    *   array.  Once this routine is called, the included ISR will handle further
    *   data in byte by byte mode.
    *
    * Parameters:
    *  slaveAddr: 7-bit slave address
    *  xferData:  Pointer to data in array.
    *  cnt:       Count of data to write.
    *  mode:      Mode of operation.  It defines normal start, restart,
    *             stop, no-stop, etc.
    *
    * Return:
    *  void
    *
    * Reentrant:
    *  No
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_MasterReadBuf(uint8 slaveAddr, uint8 * xferData, uint8 cnt, uint8 mode )
    {
        uint8 errStatus = `$INSTANCE_NAME`_MSTR_SLAVE_BUSY;
    
        /* Check if I2C in proper state to generate Start/ReStart condition */
        if((`$INSTANCE_NAME`_State == `$INSTANCE_NAME`_SM_IDLE) || \
           (`$INSTANCE_NAME`_State == `$INSTANCE_NAME`_SM_MSTR_HALT))
        {
            /* If IDLE, check if bus is free */
            if(`$INSTANCE_NAME`_State != `$INSTANCE_NAME`_SM_MSTR_HALT)
            {
                 /* If Bus is free proceed, no exist with timeout */
                if(`$INSTANCE_NAME`_CHECK_BUS_FREE(`$INSTANCE_NAME`_MCSR_REG))
                {
                    errStatus = `$INSTANCE_NAME`_MSTR_NO_ERROR;
                }
                else
                {
                    errStatus = `$INSTANCE_NAME`_MSTR_BUS_BUSY;
                }
            }
            else   /* Bus halted waiting for restart */
            {
                errStatus = `$INSTANCE_NAME`_MSTR_NO_ERROR;
                `$INSTANCE_NAME`_mstrStatus &= ~`$INSTANCE_NAME`_MSTAT_XFER_HALT; 
                CyIntClearPending(`$INSTANCE_NAME`_ISR_NUMBER);
            }
    
            /* If no timeout error, generate Start/ReStart condition */
            if(errStatus == `$INSTANCE_NAME`_MSTR_NO_ERROR)
            {
                /* Determine whether or not to automatically generate a stop condition */
                if((mode & `$INSTANCE_NAME`_MODE_NO_STOP) != 0u)
                {
                    /* Do not generate a Stop at the end of transfer */
                    `$INSTANCE_NAME`_mstrControl |= `$INSTANCE_NAME`_MSTR_NO_STOP;
                }
                else  /* Generate a Stop */
                {
                    `$INSTANCE_NAME`_mstrControl &= ~`$INSTANCE_NAME`_MSTR_NO_STOP;
                }
                
                slaveAddr = (slaveAddr << 1);
                slaveAddr |= `$INSTANCE_NAME`_READ_FLAG;   /* Set the Read flag */
                `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_MSTR_RD_ADDR;
    
                `$INSTANCE_NAME`_mstrRdBufPtr    = xferData;
                `$INSTANCE_NAME`_mstrRdBufIndex  = 0u;
                `$INSTANCE_NAME`_mstrRdBufSize   = cnt;    /* Set buffer size */
    
                `$INSTANCE_NAME`_DATA_REG = slaveAddr;         /* Write address to data reg */
    
                /* Generate a Start or ReStart depending on flag passed */
                if((mode & `$INSTANCE_NAME`_MODE_REPEAT_START) != 0u)
                {
                    `$INSTANCE_NAME`_GENERATE_RESTART;  /* Generate a ReStart */
                }
                else
                {
                    `$INSTANCE_NAME`_GENERATE_START;    /* Generate a Start */
                }

                /* Enable interrupts to process transfer */
                `$INSTANCE_NAME`_EnableInt();
    
                /* Clear read complete flag */
                `$INSTANCE_NAME`_mstrStatus &= ~`$INSTANCE_NAME`_MSTAT_RD_CMPLT; 
            }
        }

        return (errStatus);
    }
    
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_MasterSendStart
    ********************************************************************************
    *
    * Summary:
    *  Sends a start with address and R/W bit.
    *
    * Parameters:  
    *  slaveAddress: Address of slave recipiant. 
    *  R_nW:         Send or recieve mode.
    *
    * Return: 
    *  Returns a non-zero value if an error is detected
    *
    * Reentrant:
    *  No
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_MasterSendStart(uint8 slaveAddress, uint8 R_nW)
    {
        uint8 errStatus = `$INSTANCE_NAME`_MSTR_SLAVE_BUSY;
    
        if(`$INSTANCE_NAME`_State == `$INSTANCE_NAME`_SM_IDLE)
        {
            /* If Bus is free proceed, no exist with timeout */
            if(`$INSTANCE_NAME`_CHECK_BUS_FREE(`$INSTANCE_NAME`_MCSR_REG))
            {
                /* If no timeout error, generate Start/ReStart condition */
                CyIntDisable(`$INSTANCE_NAME`_ISR_NUMBER);
                slaveAddress = (slaveAddress << 1u);
                if(R_nW != 0u)
                {
                    slaveAddress |= `$INSTANCE_NAME`_READ_FLAG;   /* Set the Read flag */
                    `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_MSTR_RD_ADDR;
                }
                else
                {
                    `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_MSTR_WR_ADDR;
                }
                
                `$INSTANCE_NAME`_DATA_REG = slaveAddress;    /* Write address to data reg */
                
                /* Generates a Start */
                `$INSTANCE_NAME`_GENERATE_START;
                
                /* Wait for the address to be transfered */
                while(`$INSTANCE_NAME`_WAIT_BYTE_COMPLETE(`$INSTANCE_NAME`_CSR_REG));
                
                #if (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MULTI_MASTER_ENABLE)
                    /* Check for loss of arbitration */
                    if(`$INSTANCE_NAME`_CHECK_LOST_ARB(`$INSTANCE_NAME`_CSR_REG))
                    {
                        /* Clear CSR to release the bus, if no Slave */
                        #if ((`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE) == 0u)
                            `$INSTANCE_NAME`_READY_TO_READ;
                        #endif  /* (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE) == 0u) */    
    
                        /* Arbitration has been lost, reset state machine to Idle */
                        `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_IDLE;
                    
                        errStatus = `$INSTANCE_NAME`_MSTR_ERR_ARB_LOST;
                    }
                    else if(`$INSTANCE_NAME`_CHECK_ADDR_NAK(`$INSTANCE_NAME`_CSR_REG))
                    {
                        errStatus = `$INSTANCE_NAME`_MSTR_ERR_LB_NAK;    /* No device ACKed the Master */
                    }
                    else
                    {
                        errStatus = `$INSTANCE_NAME`_MSTR_NO_ERROR;     /* Send Start witout errors */
                    }
                    
                #else
                    /* Check ACK address if Master mode */
                    if(`$INSTANCE_NAME`_CHECK_ADDR_NAK(`$INSTANCE_NAME`_CSR_REG))
                    {
                        errStatus = `$INSTANCE_NAME`_MSTR_ERR_LB_NAK;    /* No device ACKed the Master */
                    }
                    else    
                    {
                        errStatus = `$INSTANCE_NAME`_MSTR_NO_ERROR;     /* Send Start witout errors */
                    }
                #endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MULTI_MASTER_ENABLE) */
            }
            else
            {
                errStatus = `$INSTANCE_NAME`_MSTR_BUS_BUSY;     /* Bus is busy */
            } 
        }

        return (errStatus);
    }
    
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_MasterSendRestart
    ********************************************************************************
    *
    * Summary:
    *   Sends a restart with address and R/W bit.
    *  
    * Parameters:
    *  slaveAddress: Address of slave recipiant. 
    *  R_nW:         Send or recieve mode.
    *
    * Return: 
    *  Returns a non-zero value if an error is detected
    *
    * Reentrant:
    *  No
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_MasterSendRestart(uint8 slaveAddress, uint8 R_nW)
    {
        uint8 errStatus = `$INSTANCE_NAME`_MSTR_SLAVE_BUSY;
        
        /* Check if Start condition was generated */
        if(`$INSTANCE_NAME`_CHECK_MASTER_MODE(`$INSTANCE_NAME`_MCSR_REG))
        {
            slaveAddress = (slaveAddress << 1u);
            if(R_nW != 0u)
            {
                slaveAddress |= `$INSTANCE_NAME`_READ_FLAG;    /* Set the Read flag */
                `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_MSTR_RD_ADDR;
            }
            else
            {
                `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_MSTR_WR_ADDR;
            }
            
            `$INSTANCE_NAME`_DATA_REG = slaveAddress;    /* Write address to data reg */
            
            /* Generates restart */
            `$INSTANCE_NAME`_GENERATE_RESTART;
            #if (`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`_UDB)
                while(`$INSTANCE_NAME`_CHECK_BYTE_COMPLETE(`$INSTANCE_NAME`_CSR_REG));
            #endif /* End (`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`_UDB) */

            /* Wait for the address to be transfered  */
            while(`$INSTANCE_NAME`_WAIT_BYTE_COMPLETE(`$INSTANCE_NAME`_CSR_REG));

            if(`$INSTANCE_NAME`_CHECK_ADDR_NAK(`$INSTANCE_NAME`_CSR_REG))
            {
                errStatus = `$INSTANCE_NAME`_MSTR_ERR_LB_NAK;   /* No device ACKed the Master */
            }
            else
            {
                errStatus = `$INSTANCE_NAME`_MSTR_NO_ERROR;     /* Send Start witout errors */
            }
        }
        
        return (errStatus);
    }
    
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_MasterSendStop
    ********************************************************************************
    *
    * Summary:
    *  Sends stop condition.
    *
    * Parameters:
    *  void
    *
    * Return:
    *  Returns a non-zero value if an error is detected 
    *
    * Reentrant:
    *  No
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_MasterSendStop(void)
    {
        /* Always return the success */
        uint8 errStatus = `$INSTANCE_NAME`_MSTR_NO_ERROR;

        /* Generates stop */
        if(`$INSTANCE_NAME`_CHECK_MASTER_MODE(`$INSTANCE_NAME`_MCSR_REG))
        {
            `$INSTANCE_NAME`_GENERATE_STOP; 
        }
        
        /* Reset state to IDLE */
        `$INSTANCE_NAME`_State  = `$INSTANCE_NAME`_SM_IDLE;
                
        return (errStatus);
    }
    
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_MasterWriteByte
    ********************************************************************************
    *
    * Summary:
    *  This function sends a single-byte I2C bus write and ACK.  This function does
    *  not generate a start or stop condition.  This routine should ONLY be called
    *  when a prevous start and address has been generated on the I2Cbus.
    *
    * Parameters:
    *  data:  Byte to be sent to the I2C slave
    *
    * Return:
    *  The return value is non-zero, if the slave acknowledged the master.
    *  The return value is zero, if the slave did not acknoledge the
    *  master.  If the slave failed to acknowledged the master, the
    *  value will be 0xFF.
    *
    *******************************************************************************/
    uint8  `$INSTANCE_NAME`_MasterWriteByte(uint8 theByte) `=ReentrantKeil($INSTANCE_NAME . "_MasterWriteByte")`
    {
        uint8 errStatus = `$INSTANCE_NAME`_MSTR_SLAVE_BUSY;
        
        /* Check if Start condition was generated */
        if(`$INSTANCE_NAME`_CHECK_MASTER_MODE(`$INSTANCE_NAME`_MCSR_REG))
        {
            `$INSTANCE_NAME`_DATA_REG = theByte;
            `$INSTANCE_NAME`_TRANSMIT_DATA;
            `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_MSTR_WR_DATA;
            #if(`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`_UDB)
                while(`$INSTANCE_NAME`_CHECK_BYTE_COMPLETE(`$INSTANCE_NAME`_CSR_REG));
            #endif
            
            /* Make sure the last byte has been transfered first. */
            while(`$INSTANCE_NAME`_WAIT_BYTE_COMPLETE(`$INSTANCE_NAME`_CSR_REG));
            
            if(`$INSTANCE_NAME`_CHECK_DATA_ACK(`$INSTANCE_NAME`_CSR_REG))
            {    
                errStatus = `$INSTANCE_NAME`_MSTR_NO_ERROR;     /* Send Start witout errors */
            }
            else
            {
                errStatus = `$INSTANCE_NAME`_MSTR_ERR_LB_NAK;   /* The last bit was NACKed */
            }
            
            `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_MSTR_HALT;
        }

        return (errStatus);
    }
    
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_MasterReadByte
    ********************************************************************************
    *
    * Summary:
    *  This function sends a single-byte I2C bus read and ACK phase.  This function 
    *  does not generate a start or stop condition.  This routine should ONLY be 
    *  called when a prevous start and address has been generated on the I2Cbus.
    *
    * Parameters:
    *  acknNak:  If non-zero an ACK will be the response, else a zero will
    *            cause a NAK to be sent.
    *
    * Return:
    *  Returns the data received from the I2C slave.
    *
    * Reentrant:
    *  No
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_MasterReadByte(uint8 acknNak)
    {
        uint8 theByte = 0u;
        
        /* Check if Start condition was generated */
        if(`$INSTANCE_NAME`_CHECK_MASTER_MODE(`$INSTANCE_NAME`_MCSR_REG))
        {
            /* When address phase need release the bus and receive the byte,
            than decide ACK or NACK */
            if (`$INSTANCE_NAME`_SM_MSTR_RD_ADDR == `$INSTANCE_NAME`_State)
            {
                `$INSTANCE_NAME`_READY_TO_READ;
                `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_MSTR_DATA;
                #if (`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`_UDB)
                    while(`$INSTANCE_NAME`_CHECK_BYTE_COMPLETE(`$INSTANCE_NAME`_CSR_REG));
                #endif /* End (`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`_UDB) */
            }
            
            while(`$INSTANCE_NAME`_WAIT_BYTE_COMPLETE(`$INSTANCE_NAME`_CSR_REG));

            theByte = `$INSTANCE_NAME`_DATA_REG;
        
            /* Now if the ACK flag was set, Ack the data which will release the bus and start the next byte in
               otherwise do NOTHING to the CSR reg.  
               This will allow the calling routine to generate a repeat start or a stop depending on it's preference. */
            if(acknNak != 0u)   /* Do ACK */
            {
                `$INSTANCE_NAME`_ACK_AND_RECEIVE;
                #if (`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`_UDB)
                    while(`$INSTANCE_NAME`_CHECK_BYTE_COMPLETE(`$INSTANCE_NAME`_CSR_REG));
                #endif /* End (`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`_UDB) */
            }
            else                /* Do NACK */
            {
                /* Do nothing to be able work with ReStart */
                `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_MSTR_HALT;
            }
        }
        
        return (theByte);
    }
    
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_MasterGetReadBufSize
    ********************************************************************************
    *
    * Summary:
    *  Determines the number of bytes used in the RX buffer. Empty returns 0.
    *
    * Parameters:
    *  void
    *
    * Return:
    *  Number of bytes in buffer until full.
    *
    *******************************************************************************/
    uint16 `$INSTANCE_NAME`_MasterGetReadBufSize(void) `=ReentrantKeil($INSTANCE_NAME . "_MasterGetReadBufSize")`
    {
        return (`$INSTANCE_NAME`_mstrRdBufIndex);
    }
    
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_MasterGetWriteBufSize
    ********************************************************************************
    * 
    * Summary:
    *  Determine the number of bytes used in the TX buffer.  Empty returns 0.
    *
    * Parameters:
    *  void
    *
    * Return:
    *  void
    *
    *******************************************************************************/
    uint16 `$INSTANCE_NAME`_MasterGetWriteBufSize(void) `=ReentrantKeil($INSTANCE_NAME . "_MasterGetWriteBufSize")`
    {
        return (`$INSTANCE_NAME`_mstrWrBufIndex);
    }
    
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_MasterClearReadBuf
    ********************************************************************************
    *
    * Summary:
    *  Sets the buffer read and write pointers to 0.
    *
    * Parameters:
    *  void
    *
    * Return:
    *  void
    *
    * Reentrant:
    *  No
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_MasterClearReadBuf(void)
    {
        `$INSTANCE_NAME`_mstrRdBufIndex = 0u;
        `$INSTANCE_NAME`_mstrStatus &= ~`$INSTANCE_NAME`_MSTAT_RD_CMPLT;
    }
    
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_MasterClearWriteBuf
    ********************************************************************************
    *
    * Summary:
    *  Sets the buffer read and write pointers to 0.
    *
    * Parameters:
    *  void
    *
    * Return:
    *  void
    *
    * Reentrant:
    *  No
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_MasterClearWriteBuf(void)
    {
        `$INSTANCE_NAME`_mstrWrBufIndex = 0u;
        `$INSTANCE_NAME`_mstrStatus &= ~`$INSTANCE_NAME`_MSTAT_WR_CMPLT;
    }
    
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_Workaround
    ********************************************************************************
    *
    * Summary:
    *  Do nothing. This fake fuction use as workaround for CDT 78083.
    *
    * Parameters:
    *  void
    *
    * Return:
    *  void
    *
    * Reentrant:
    *  No
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_Workaround(void)
    {

    }
    
#endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MASTER) */


#if (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE)

    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_SlaveStatus
    ********************************************************************************
    *
    * Summary:
    *  Returns status of the I2C status register. 
    *
    * Parameters:
    *  void
    *
    * Return:
    *  Returns status of I2C slave status register.
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_SlaveStatus(void) `=ReentrantKeil($INSTANCE_NAME . "_SlaveStatus")`  
    {
        return (`$INSTANCE_NAME`_slStatus);
    }
    
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_SlaveClearReadStatus
    ********************************************************************************
    *
    * Summary:
    *  Clears the read status bits in the I2C_RsrcStatus register and returns read
    *  status.  No other bits are affected.
    *
    * Parameters:
    *  void
    *
    * Return:
    *  Return the read status.
    *
    * Reentrant:
    *  No
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_SlaveClearReadStatus(void) 
    {
        uint8 status;
    
        status = `$INSTANCE_NAME`_slStatus & `$INSTANCE_NAME`_SSTAT_RD_MASK;
        
        /* Mask of transfer complete flag and Error status */
        `$INSTANCE_NAME`_slStatus &= ~`$INSTANCE_NAME`_SSTAT_RD_CLEAR;
        
        return (status);
    }
    
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_SlaveClearWriteStatus
    ********************************************************************************
    *
    * Summary:
    *  Clears the write status bits in the I2C_Status register and returns write
    *  status. No other bits are affected.
    *
    * Parameters:
    *  void
    *
    * Return:
    *  Return the write status.
    *
    * Reentrant:
    *  No
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_SlaveClearWriteStatus(void)
    {
        uint8 status;
    
        status = `$INSTANCE_NAME`_slStatus & `$INSTANCE_NAME`_SSTAT_WR_MASK;
    
        /* Mask of transfer complete flag and Error status */
        `$INSTANCE_NAME`_slStatus &= ~`$INSTANCE_NAME`_SSTAT_WR_CLEAR;
        
        return (status);
    }
    
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_SlaveSetAddress
    ********************************************************************************
    *
    * Summary:
    *  Sets the address for the first device.
    *
    * Parameters:
    *  (uint8) address:  The slave adderss for the first device.          
    *
    * Return:
    *  void
    *
    * Reentrant:
    *  No
    *
    *******************************************************************************/
    #if (`$INSTANCE_NAME`_ADDR_DECODE == `$INSTANCE_NAME`_HDWR_DECODE)
        void `$INSTANCE_NAME`_SlaveSetAddress(uint8 address) `=ReentrantKeil($INSTANCE_NAME . "_SlaveSetAddress")`
        { 
            `$INSTANCE_NAME`_ADDR_REG = address & `$INSTANCE_NAME`_SADDR_MASK;     /* Set I2C Address register */
        }
        
    #else
        void `$INSTANCE_NAME`_SlaveSetAddress(uint8 address)
        {  
            `$INSTANCE_NAME`_Address = address & `$INSTANCE_NAME`_SADDR_MASK;  /* Set Address variable */ 
        }
        
    #endif  /* End (`$INSTANCE_NAME`_ADDR_DECODE == `$INSTANCE_NAME`_HDWR_DECODE) */ 
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_SlavePutReadByte
    ********************************************************************************
    *
    * Summary:
    *  For Master Read, sends 1 byte out Slave transmit buffer.
    *  Wait to send byte until buffer has room.  Used to preload
    *  the transmit buffer.
    *
    *  In byte by byte mode if the last byte was ACKed, stall the master
    *  (on the first bit of the next byte) if needed until the next byte
    *  is PutChared.  If the last byte was NAKed it does not stall the bus
    *  because the master will generate a stop or restart condition.
    *
    * Parameters:
    *  (uint8) transmitDataByte: Byte containing the data to transmit.
    *
    * Return:
    *  void
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_SlavePutReadByte(uint8 transmitDataByte) `=ReentrantKeil($INSTANCE_NAME . "_SlavePutReadByte")`
    {
        `$INSTANCE_NAME`_DATA_REG = transmitDataByte;
        `$INSTANCE_NAME`_ACK_AND_TRANSMIT;
    }
    
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_SlaveGetWriteByte
    ********************************************************************************
    *
    * Summary:
    *  For a Master Write, ACKs or NAKs the previous byte and reads out the last
    *  byte tranmitted.  The first byte read of a packet is the Address byte in 
    *  which case there is no previous data so no ACK or NAK is generated.  The
    *  bus is stalled until the next GetByte, therefore a GetByte must be executed
    *  after the last byte in order to send the final ACK or NAK before the Master
    *  can send a Stop or restart condition.
    *
    * Parameters:
    *  ackNak:  1 = ACK, 0 = NAK for the previous byte received.
    *
    * Return: 
    *  Last byte transmitted or last byte in buffer from Master.
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_SlaveGetWriteByte(uint8 ackNak) `=ReentrantKeil($INSTANCE_NAME . "_SlaveGetWriteByte")`
    {
        uint8 dataByte;
    
        dataByte = `$INSTANCE_NAME`_DATA_REG;
        
        if(ackNak == `$INSTANCE_NAME`_ACK_DATA)
        {
            `$INSTANCE_NAME`_ACK_AND_RECEIVE;
        }
        else
        {
            `$INSTANCE_NAME`_NAK_AND_RECEIVE;
        }            
        
        return (dataByte);
    }
    
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_SlaveInitReadBuf
    ********************************************************************************
    *
    * Summary:
    *  This function sets up the buffer in which data will be read by the 
    *  Master.  The buffer index will be reset to zero and the status flags
    *  will be cleared with this command.
    *
    * Parameters:
    *  readBuf:  Pointer to the array to be sent to the Slave transmit register.
    *  bufSize:  Size of the buffer to transfer.
    *
    * Return:
    *  void
    *
    * Side Effects:
    *     If this function is called during a bus transaction, data from the 
    *     previous buffer location and the beginning of this buffer may be
    *     transmitted.
    *
    * Reentrant:
    *  No
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_SlaveInitReadBuf(uint8 * readBuf, uint8 bufSize)
    {
        `$INSTANCE_NAME`_readBufPtr   = readBuf;
        `$INSTANCE_NAME`_readBufIndex = 0u;
        `$INSTANCE_NAME`_readBufSize  = bufSize;  /* Set buffer size */
    }
    
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_SlaveInitWriteBuf
    ********************************************************************************
    *
    * Summary:
    *  This function initializes the write buffer.  The write buffer is the array
    *  that is written to when the master performs a write operation.
    *
    * Parameters:
    *  writeBuf:  Pointer to the array used to store the data written by the Master 
    *             and read by the Slave.
    *  bufSize:   Size of buffer to receive data from master.
    *
    * Return: 
    *  void
    *
    * Reentrant:
    *  No
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_SlaveInitWriteBuf(uint8 * writeBuf, uint8 bufSize)
    {
        `$INSTANCE_NAME`_writeBufPtr   = writeBuf;
        `$INSTANCE_NAME`_writeBufIndex = 0u;
        `$INSTANCE_NAME`_writeBufSize  = bufSize;  /* Set buffer size */
    }
    
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_SlaveGetReadBufSize
    ********************************************************************************
    *
    * Summary:
    *  Returns the count of bytes read by the Master since the buffer was reset.
    *  The maximum return value will be the size of the buffer.
    *
    * Parameters:
    *  void
    *
    * Return:
    *  (uint8) Bytes read by Master.
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_SlaveGetReadBufSize(void) `=ReentrantKeil($INSTANCE_NAME . "_SlaveGetReadBufSize")`
    {
        return (`$INSTANCE_NAME`_readBufIndex);
    }
    
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_SlaveGetWriteBufSize
    ********************************************************************************
    *
    * Summary:
    *  Returns the count of bytes written by the I2C Master. The maximum value
    *  that will be returned in the buffer size itself.
    *
    * Parameters:
    *  void
    *
    * Return:
    *  The valid number of bytes in Tx buffer.
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_SlaveGetWriteBufSize(void) `=ReentrantKeil($INSTANCE_NAME . "_SlaveGetWriteBufSize")`
    {
        return (`$INSTANCE_NAME`_writeBufIndex);
    }
    
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_SlaveClearReadBuf
    ********************************************************************************
    *
    * Summary:
    *  Sets the buffer read buffer index back to zero.
    *
    * Parameters:
    *  void
    *
    * Return:
    *  void
    *
    * Reentrant:
    *  No
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_SlaveClearReadBuf(void)
    {
        `$INSTANCE_NAME`_readBufIndex = 0u;
    }
    
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_SlaveClearRxBuf
    ********************************************************************************
    *
    * Summary:
    *  Sets the I2C write buffer index to 0.
    *
    * Parameters:
    *  void
    *
    * Return:
    *  void
    *
    * Reentrant:
    *  No
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_SlaveClearWriteBuf(void)
    {
        `$INSTANCE_NAME`_writeBufIndex = 0u;
    }
    
    
#endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE) */


/* [] END OF FILE */
