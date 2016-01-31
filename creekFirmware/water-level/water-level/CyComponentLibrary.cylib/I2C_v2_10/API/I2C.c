/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the source code of APIs for the I2C component.
*  Actual protocol and operation code resides in the interrupt service routine 
*  file.
*
* Note: 
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
extern volatile uint8 `$INSTANCE_NAME`_State;    /* Current state of I2C state machine */

/* Master variables */
#if (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MASTER)
   extern volatile uint8 `$INSTANCE_NAME`_mstrStatus;          /* Master Status byte */
   extern volatile uint8 `$INSTANCE_NAME`_mstrControl;         /* Master Control byte */
   
   /* Transmit buffer variables */
   extern volatile uint8 * `$INSTANCE_NAME`_mstrRdBufPtr;      /* Pointer to Master Read buffer */
   extern volatile uint8   `$INSTANCE_NAME`_mstrRdBufSize;     /* Master Read buffer size */
   extern volatile uint8   `$INSTANCE_NAME`_mstrRdBufIndex;    /* Master Read buffer Index */
    
   /* Receive buffer variables */
   extern volatile uint8 * `$INSTANCE_NAME`_mstrWrBufPtr;      /* Pointer to Master Write buffer */
   extern volatile uint8   `$INSTANCE_NAME`_mstrWrBufSize;     /* Master Write buffer size */
   extern volatile uint8   `$INSTANCE_NAME`_mstrWrBufIndex;    /* Master Write buffer Index */
   
#endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MASTER) */

/* Slave variables */
#if (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE)
   extern volatile uint8 `$INSTANCE_NAME`_slStatus;            /* Slave Status  */
   
   #if (`$INSTANCE_NAME`_ADDR_DECODE == `$INSTANCE_NAME`_SW_DECODE)
      extern volatile uint8 `$INSTANCE_NAME`_slAddress;        /* Software address variable */
   #endif   /* End (`$INSTANCE_NAME`_ADDR_DECODE == `$INSTANCE_NAME`_SW_DECODE) */
   
   /* Transmit buffer variables */
   extern volatile uint8 * `$INSTANCE_NAME`_slRdBufPtr;        /* Pointer to Transmit buffer */
   extern volatile uint8   `$INSTANCE_NAME`_slRdBufSize;       /* Slave Transmit buffer size */
   extern volatile uint8   `$INSTANCE_NAME`_slRdBufIndex;      /* Slave Transmit buffer Index */

   /* Receive buffer variables */
   extern volatile uint8 * `$INSTANCE_NAME`_slWrBufPtr;        /* Pointer to Receive buffer */
   extern volatile uint8   `$INSTANCE_NAME`_slWrBufSize;       /* Slave Receive buffer size */
   extern volatile uint8   `$INSTANCE_NAME`_slWrBufIndex;      /* Slave Receive buffer Index */

#endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE) */


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Init
********************************************************************************
*
* Summary:
*  Initializes I2C registers with initial values provided from customizer.
*
* Parameters:  
*  None
*
* Return: 
*  None
*
* Global variables:
*  None
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
            `$INSTANCE_NAME`_XCFG_REG = `$INSTANCE_NAME`_XCFG_I2C_ON;
        
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
*  Enables I2C operations.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global variables:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`
{          
    #if ((`$INSTANCE_NAME`_IMPLEMENTATION != `$INSTANCE_NAME`_UDB) || \
         (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE))
        uint8 enableInterrupts;
    #endif  /* End ((`$INSTANCE_NAME`_IMPLEMENTATION != `$INSTANCE_NAME`_UDB) || \
                    (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE) ) */
                   
    #if(`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`_FF)
        enableInterrupts = CyEnterCriticalSection();
        /* Enable power to I2C Module */
        `$INSTANCE_NAME`_ACT_PWRMGR_REG  |= `$INSTANCE_NAME`_ACT_PWR_EN;
        `$INSTANCE_NAME`_STBY_PWRMGR_REG |= `$INSTANCE_NAME`_STBY_PWR_EN;
        CyExitCriticalSection(enableInterrupts);
        
    #else
        /* Enable the I2C */
        `$INSTANCE_NAME`_CFG_REG |= (`$INSTANCE_NAME`_ENABLE_MASTER | `$INSTANCE_NAME`_ENABLE_SLAVE);
        
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
*  Starts the I2C hardware. Enables Active mode power template bits or clock 
*  gating as appropriate. It is required to be executed before I2C bus operation.
*  The I2C interrupt remains disabled after this function call.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Side Effects:
*  This component automatically enables it's interrupt.  If I2C is enabled
*  without the interrupt enabled, it could lock up the I2C bus.
*
* Global variables:
*  `$INSTANCE_NAME`_initVar - used to check initial configuration, modified 
*  on first function call.
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
*  Disables I2C hardware and disables I2C interrupt. Disables Active mode power 
*  template bits or clock gating as appropriate.
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
*  Enables I2C interrupt. Interrupts are required for most operations.
*
* Parameters:
*  None
*
* Return:
*  None
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
*  Disables I2C interrupts. Normally this function is not required since the 
*  Stop function disables the interrupt. If the I2C interrupt is disabled while 
*  the I2C master is still running, it may cause the I2C bus to lock up.
*
* Parameters:
*  None
*
* Return: 
*  None
*
* Side Effects:
*  If the I2C interrupt is disabled and the master is addressing the current 
*  slave, the bus will be locked until the interrupt is re-enabled.
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
    *  Returns the master's communication status.
    *
    * Parameters:
    *  None
    *
    * Return:
    *  Current status of I2C master.
    *
    * Global variables:
    *  `$INSTANCE_NAME`_mstrStatus - used to store current status of I2C Master.
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_MasterStatus(void) `=ReentrantKeil($INSTANCE_NAME . "_MasterStatus")`
    {
        uint8 status = `$INSTANCE_NAME`_mstrStatus; 
        
        if(`$INSTANCE_NAME`_State != `$INSTANCE_NAME`_SM_SL_WR_IDLE)
        {
            /* Add transaction in progress activity to master status */
            status |= `$INSTANCE_NAME`_MSTAT_XFER_INP;
        }
        else
        {
            /* Current master status is valid */
        }
        
        return (status);
    }
    
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_MasterClearStatus
    ********************************************************************************
    *
    * Summary:
    *  Clears all status flags and returns the master status.
    *
    * Parameters:
    *  None
    *
    * Return: 
    *  Current status of I2C master.
    *
    * Global variables:
    *  `$INSTANCE_NAME`_mstrStatus - used to store current status of I2C Master.
    *
    * Reentrant:
    *  No
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_MasterClearStatus(void)
    {
        /* Current master status */
        uint8 status = `$INSTANCE_NAME`_mstrStatus;
        
        /* Clear master status */
        `$INSTANCE_NAME`_mstrStatus  = `$INSTANCE_NAME`_MSTAT_CLEAR; 
        
        return (status);
    }
    
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_MasterWriteBuf
    ********************************************************************************
    *
    * Summary:
    *  Automatically writes an entire buffer of data to a slave device. Once the 
    *  data transfer is initiated by this function, further data transfer is handled
    *  by the included ISR in byte by byte mode.
    *
    * Parameters:
    *  slaveAddr: 7-bit slave address.
    *  xferData:  Pointer to buffer of data to be sent.
    *  cnt:       Size of buffer to send.
    *  mode:      Transfer mode defines: start or restart condition generation at 
    *             begin of the transfer and complete the transfer or halt before 
    *             generating a stop.
    *
    * Return:
    *  Status error - zero means no errors.
    *
    * Side Effects:
    *  The included ISR will start transfer after start or restart condition will 
    *  be generated.
    *
    * Global variables:
    *  `$INSTANCE_NAME`_mstrStatus  - used to store current status of I2C Master.
    *  `$INSTANCE_NAME`_State       - used to store current state of state machine.
    *  `$INSTANCE_NAME`_mstrControl - used to control master end of transaction with
    *  or without the Stop generation.
    *  `$INSTANCE_NAME`_mstrWrBufPtr - used to store pointer to master write buffer.
    *  `$INSTANCE_NAME`_mstrWrBufIndex - used to current index within master write 
    *  buffer.
    *  `$INSTANCE_NAME`_mstrWrBufSize - used to store master write buffer size.
    *
    * Reentrant:
    *  No
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_MasterWriteBuf(uint8 slaveAddr, uint8 * xferData, uint8 cnt, uint8 mode)
    {
        uint8 errStatus = `$INSTANCE_NAME`_MSTR_SLAVE_BUSY; 
        
        /* Check if I2C in proper state to generate Start/ReStart condition */
        if((`$INSTANCE_NAME`_State == `$INSTANCE_NAME`_SM_IDLE) || 
           (`$INSTANCE_NAME`_State == `$INSTANCE_NAME`_SM_MSTR_HALT))
        {
            /* If IDLE, check if bus is free */
            if(`$INSTANCE_NAME`_State == `$INSTANCE_NAME`_SM_IDLE)
            {
                /* If Bus is free proceed, no exist */
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
                CyIntClearPending(`$INSTANCE_NAME`_ISR_NUMBER);
                `$INSTANCE_NAME`_mstrStatus &= ~`$INSTANCE_NAME`_MSTAT_XFER_HALT;
                errStatus = `$INSTANCE_NAME`_MSTR_NO_ERROR;
            }
            
            /* If no errors, generate start */
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
                
                `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_MSTR_WR_ADDR;
                slaveAddr = (slaveAddr << 1u);              /* Set Address */
                `$INSTANCE_NAME`_DATA_REG = slaveAddr;      /* Write address to data reg */
                
                `$INSTANCE_NAME`_mstrWrBufIndex = 0u;       /* Start buffer at zero */
                `$INSTANCE_NAME`_mstrWrBufSize  = cnt;      /* Set buffer size */
                `$INSTANCE_NAME`_mstrWrBufPtr   = (volatile uint8 *) xferData; /* Set buffer pointer */
                
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
    *  Automatically writes an entire buffer of data to a slave device. Once the 
    *  data transfer is initiated by this function, further data transfer is handled
    *  by the included ISR in byte by byte mode.  
    *
    * Parameters:
    *  slaveAddr: 7-bit slave address.
    *  xferData:  Pointer to buffer where to put data from slave.
    *  cnt:       Size of buffer to read.
    *  mode:      Transfer mode defines: start or restart condition generation at 
    *             begin of the transfer and complete the transfer or halt before 
    *             generating a stop.
    *
    * Return:
    *  Status error - zero means no errors.
    *
    * Side Effects:
    *  The included ISR will start transfer after start or restart condition will 
    *  be generated.
    *
    * Global variables:
    *  `$INSTANCE_NAME`_mstrStatus  - used to store current status of I2C Master.
    *  `$INSTANCE_NAME`_State       - used to store current state of state machine.
    *  `$INSTANCE_NAME`_mstrControl - used to control master end of transaction with
    *  or without the Stop generation.
    *  `$INSTANCE_NAME`_mstrRdBufPtr - used to store pointer to master write buffer.
    *  `$INSTANCE_NAME`_mstrRdBufIndex - used to current index within master write 
    *  buffer.
    *  `$INSTANCE_NAME`_mstrRdBufSize - used to store master write buffer size.
    *
    * Reentrant:
    *  No
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_MasterReadBuf(uint8 slaveAddr, uint8 * xferData, uint8 cnt, uint8 mode)
    {
        uint8 errStatus = `$INSTANCE_NAME`_MSTR_SLAVE_BUSY;
    
        /* Check if I2C in proper state to generate Start/ReStart condition */
        if((`$INSTANCE_NAME`_State == `$INSTANCE_NAME`_SM_IDLE) ||
           (`$INSTANCE_NAME`_State == `$INSTANCE_NAME`_SM_MSTR_HALT))
        {
            /* If IDLE, check if bus is free */
            if(`$INSTANCE_NAME`_State == `$INSTANCE_NAME`_SM_IDLE)
            {
                 /* If Bus is free proceed, no exist */
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
                CyIntClearPending(`$INSTANCE_NAME`_ISR_NUMBER);
                `$INSTANCE_NAME`_mstrStatus &= ~`$INSTANCE_NAME`_MSTAT_XFER_HALT;
                errStatus = `$INSTANCE_NAME`_MSTR_NO_ERROR;
            }
            
            /* If no error, generate Start/ReStart condition */
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
                
                `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_MSTR_RD_ADDR;
                slaveAddr = (slaveAddr << 1);               /* Set Address */
                slaveAddr |= `$INSTANCE_NAME`_READ_FLAG;    /* Set the Read flag */
                `$INSTANCE_NAME`_DATA_REG = slaveAddr;      /* Write address to data reg */
                 
                `$INSTANCE_NAME`_mstrRdBufIndex  = 0u;      /* Start buffer at zero */
                `$INSTANCE_NAME`_mstrRdBufSize   = cnt;     /* Set buffer size */
                `$INSTANCE_NAME`_mstrRdBufPtr    = (volatile uint8 *) xferData; /* Set buffer pointer */
                
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
    *  Generates Start condition and sends slave address with read/write bit.
    *
    * Parameters:  
    *  slaveAddress:  7-bit slave address. 
    *  R_nW:          Zero, send write command, non-zero send read command.
    *
    * Return: 
    *  Status error - zero means no errors.
    *
    * Side Effects:
    *  This function is entered without a 'byte complete' bit set in the I2C_CSR 
    *  register. It does not exit until it will be set.
    *
    * Global variables:
    *  `$INSTANCE_NAME`_State - used to store current state of state machine.
    *
    * Reentrant:
    *  No
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_MasterSendStart(uint8 slaveAddress, uint8 R_nW)
    {
        uint8 errStatus = `$INSTANCE_NAME`_MSTR_SLAVE_BUSY;
        
        /* If IDLE, check if bus is free */
        if(`$INSTANCE_NAME`_State == `$INSTANCE_NAME`_SM_IDLE)
        {
            /* If bus is free, generate Start condition */
            if(`$INSTANCE_NAME`_CHECK_BUS_FREE(`$INSTANCE_NAME`_MCSR_REG))
            {
                /* Disable ISR for Manual functions */
                `$INSTANCE_NAME`_DisableInt();
                
                slaveAddress = (slaveAddress << 1u);        /* Set Address */
                if(R_nW != 0u)                              /* Set the Read/Write flag */                                 
                {
                    slaveAddress |= `$INSTANCE_NAME`_READ_FLAG;   
                    `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_MSTR_RD_ADDR;
                }
                else
                {
                    `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_MSTR_WR_ADDR;
                }
                `$INSTANCE_NAME`_DATA_REG = slaveAddress;   /* Write address to data reg */
                
                /* Generates a START */
                `$INSTANCE_NAME`_GENERATE_START;
                
                /* Wait for the address to be transfered */
                while(`$INSTANCE_NAME`_WAIT_BYTE_COMPLETE(`$INSTANCE_NAME`_CSR_REG));
                
                #if(`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MULTI_MASTER_ENABLE)
                    /* Check for loss of arbitration */
                    if(`$INSTANCE_NAME`_CHECK_LOST_ARB(`$INSTANCE_NAME`_CSR_REG))
                    {
                        /* MultiMaster-Slave */
                        #if(`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE)
                            /* Check if Start has been generated */
                            if((`$INSTANCE_NAME`_MCSR_REG & `$INSTANCE_NAME`_MCSR_START_GEN) != 0u)
                            {
                                /* Clear if was not generated */
                                `$INSTANCE_NAME`_MCSR_REG &= ~`$INSTANCE_NAME`_MCSR_START_GEN;
                            }
                        #endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE) */
                        
                        /* Arbitration has been lost, reset state machine to IDLE */
                        `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_IDLE;
                        errStatus = `$INSTANCE_NAME`_MSTR_ERR_ARB_LOST; /* Master lost arbitrage */
                    }
                    else if(`$INSTANCE_NAME`_CHECK_ADDR_NAK(`$INSTANCE_NAME`_CSR_REG))
                    {
                        /* Address has been NACKed, reset state machine to IDLE */
                        `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_IDLE;
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
                        /* Address has been NACKed, reset state machine to IDLE */
                        `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_IDLE;
                        errStatus = `$INSTANCE_NAME`_MSTR_ERR_LB_NAK;    /* No device ACKed the Master */
                    }
                    else    
                    {
                        errStatus = `$INSTANCE_NAME`_MSTR_NO_ERROR;     /* Send START witout errors */
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
    *  Generates ReStart condition and sends slave address with read/write bit.
    *
    * Parameters:  
    *  slaveAddress:  7-bit slave address.
    *  R_nW:          Zero, send write command, non-zero send read command.
    *
    * Return: 
    *  Status error - zero means no errors.
    *
    * Side Effects:
    *  This function is entered without a 'byte complete' bit set in the I2C_CSR 
    *  register. It does not exit until it will be set.
    *
    * Global variables:
    *  `$INSTANCE_NAME`_State - used to store current state of state machine.
    *
    * Reentrant:
    *  No
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_MasterSendRestart(uint8 slaveAddress, uint8 R_nW)
    {
        uint8 errStatus = `$INSTANCE_NAME`_MSTR_SLAVE_BUSY;
        
        /* Check if START condition was generated */
        if(`$INSTANCE_NAME`_CHECK_MASTER_MODE(`$INSTANCE_NAME`_MCSR_REG))
        {
            slaveAddress = (slaveAddress << 1u);        /* Set Address */
            if(R_nW != 0u)                              /* Set the Read/Write flag */
            {
                slaveAddress |= `$INSTANCE_NAME`_READ_FLAG;
                `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_MSTR_RD_ADDR;
            }
            else
            {
                `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_MSTR_WR_ADDR;
            }
            `$INSTANCE_NAME`_DATA_REG = slaveAddress;    /* Write address to data reg */
            
            /* Generates RESTART */
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
                errStatus = `$INSTANCE_NAME`_MSTR_NO_ERROR;     /* Send RESTART witout errors */
            }
        }
        
        return (errStatus);
    }
    
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_MasterSendStop
    ********************************************************************************
    *
    * Summary:
    *  Generates I2C Stop condition on bus. Function do nothing if Start or Restart 
    *  condition was failed before call this function.
    *
    * Parameters:  
    *  None
    *
    * Return: 
    *  Status error - zero means no errors.
    *
    * Side Effects:
    *  The Stop generation is required to complete transaction.
    *  This function does not wait while Stop condition will be generated.
    *
    * Global variables:
    *  `$INSTANCE_NAME`_State - used to store current state of state machine.
    *
    * Reentrant:
    *  No
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_MasterSendStop(void)
    {
        uint8 errStatus = `$INSTANCE_NAME`_MSTR_SLAVE_BUSY;
        
        /* Check if START condition was generated */
        if(`$INSTANCE_NAME`_CHECK_MASTER_MODE(`$INSTANCE_NAME`_MCSR_REG))
        {
            `$INSTANCE_NAME`_GENERATE_STOP;                     /* Generate STOP */
            `$INSTANCE_NAME`_State  = `$INSTANCE_NAME`_SM_IDLE; /* Reset state to IDLE */
            errStatus = `$INSTANCE_NAME`_MSTR_NO_ERROR;         /* Start send STOP witout errors */
                
            /* Wait for STOP generation or BYTE COMPLETE (lost arbitrage) */
            #if(`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MULTI_MASTER_ENABLE)
                
                #if (`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`_UDB)
                    while(`$INSTANCE_NAME`_CHECK_BYTE_COMPLETE(`$INSTANCE_NAME`_CSR_REG));
                #endif /* End (`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`_UDB) */
                
                while((`$INSTANCE_NAME`_CSR_REG & (`$INSTANCE_NAME`_CSR_BYTE_COMPLETE | 
                                                   `$INSTANCE_NAME`_CSR_STOP_STATUS)) == 0u);
                
                /* Check LOST ARBITRAGE */
                if(`$INSTANCE_NAME`_CHECK_LOST_ARB(`$INSTANCE_NAME`_CSR_REG))
                {
                    errStatus = `$INSTANCE_NAME`_MSTR_ERR_ARB_LOST; /* Generate NACK and STOP with LOST ARBITRAGE */
                }
                /* STOP condition generated */
                else
                {
                    errStatus = `$INSTANCE_NAME`_MSTR_NO_ERROR;     /* Generate STOP wihtout */
                }
                
            #endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MULTI_MASTER_ENABLE) */
        }
        
        return (errStatus);
    }
    
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_MasterWriteByte
    ********************************************************************************
    *
    * Summary:
    *  Sends one byte to a slave. A valid Start or ReStart condition must be 
    *  generated before this call this function. Function do nothing if Start or 
    *  Restart condition was failed before call this function.
    *
    * Parameters:
    *  data:  The data byte to send to the slave.
    *
    * Return:
    *  Status error - zero means no errors.
    *
    * Side Effects:
    *  This function is entered without a 'byte complete' bit set in the I2C_CSR 
    *  register. It does not exit until it will be set.
    *
    * Global variables:
    *  `$INSTANCE_NAME`_State - used to store current state of state machine.
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_MasterWriteByte(uint8 theByte)
    {
        uint8 errStatus = `$INSTANCE_NAME`_MSTR_SLAVE_BUSY;
        
        /* Check if START condition was generated */
        if(`$INSTANCE_NAME`_CHECK_MASTER_MODE(`$INSTANCE_NAME`_MCSR_REG))
        {
            `$INSTANCE_NAME`_DATA_REG = theByte;        /* Write DATA register */
            `$INSTANCE_NAME`_TRANSMIT_DATA;             /* Set transmit mode */
            `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_MSTR_WR_DATA;  /* Set state WR_DATA */
            #if(`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`_UDB)
                while(`$INSTANCE_NAME`_CHECK_BYTE_COMPLETE(`$INSTANCE_NAME`_CSR_REG));
            #endif
            
            /* Make sure the last byte has been transfered first */
            while(`$INSTANCE_NAME`_WAIT_BYTE_COMPLETE(`$INSTANCE_NAME`_CSR_REG));
            
        #if (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MULTI_MASTER_ENABLE)
                /* Check for LOST ARBITRATION */
                if(`$INSTANCE_NAME`_CHECK_LOST_ARB(`$INSTANCE_NAME`_CSR_REG))
                {
                    /* Arbitration has been lost, reset state machine to IDLE */
                    `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_IDLE;          /* Reset state to IDLE */
                    errStatus = `$INSTANCE_NAME`_MSTR_ERR_ARB_LOST;             /* The Master LOST ARBITRAGE */
                }
                /* Check LRB bit */
                else if(`$INSTANCE_NAME`_CHECK_DATA_ACK(`$INSTANCE_NAME`_CSR_REG))
                {
                    `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_MSTR_HALT;     /* Set state to HALT */
                    errStatus = `$INSTANCE_NAME`_MSTR_NO_ERROR;                 /* The LRB was ACKed */
                }
                else
                {
                    `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_MSTR_HALT;     /* Set state to HALT */
                    errStatus = `$INSTANCE_NAME`_MSTR_ERR_LB_NAK;               /* The LRB was NACKed */
                }
                
            #else
                
                `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_MSTR_HALT;     /* Set state HALT */
                
                /* Check LRB bit */
                if(`$INSTANCE_NAME`_CHECK_DATA_ACK(`$INSTANCE_NAME`_CSR_REG))
                {
                    errStatus = `$INSTANCE_NAME`_MSTR_NO_ERROR;             /* The LRB was ACKed */
                }
                else
                {
                    errStatus = `$INSTANCE_NAME`_MSTR_ERR_LB_NAK;           /* The LRB was NACKed */
                }
                
            #endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MULTI_MASTER_ENABLE) */
        }
        
        return (errStatus);
    }
    
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_MasterReadByte
    ********************************************************************************
    *
    * Summary:
    *  Reads one byte from a slave and ACK or NACK the transfer. A valid Start or 
    *  ReStart condition must be generated before this call this function. Function
    *  do nothing if Start or Restart condition was failed before call this 
    *  function.
    *
    * Parameters:
    *  acknNack:  Zero, response with NACK, if non-zero response with ACK.
    *
    * Return:
    *  Byte read from slave.
    *
    * Side Effects:
    *  This function is entered without a 'byte complete' bit set in the I2C_CSR 
    *  register. It does not exit until it will be set.
    *
    * Global variables:
    *  `$INSTANCE_NAME`_State - used to store current state of state machine.
    *
    * Reentrant:
    *  No
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_MasterReadByte(uint8 acknNak)
    {
        uint8 theByte = 0u;
        
        /* Check if START condition was generated */
        if(`$INSTANCE_NAME`_CHECK_MASTER_MODE(`$INSTANCE_NAME`_MCSR_REG))
        {
            /* When address phase need release the bus and receive the byte, then decide ACK or NACK */
            if(`$INSTANCE_NAME`_State == `$INSTANCE_NAME`_SM_MSTR_RD_ADDR)
            {
                `$INSTANCE_NAME`_READY_TO_READ;
                `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_MSTR_DATA;
                #if (`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`_UDB)
                    while(`$INSTANCE_NAME`_CHECK_BYTE_COMPLETE(`$INSTANCE_NAME`_CSR_REG));
                #endif /* End (`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`_UDB) */
            }
            
            while(`$INSTANCE_NAME`_WAIT_BYTE_COMPLETE(`$INSTANCE_NAME`_CSR_REG));
            
            theByte = `$INSTANCE_NAME`_DATA_REG;
            
            /* Now if the ACK flag was set, ACK the data which will release the bus and start the next byte in
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
    *  Returns the amount of bytes that has been transferred with an 
    *  I2C_MasterReadBuf command.
    *
    * Parameters:
    *  None
    *
    * Return:
    *  Byte count of transfer. If the transfer is not yet complete, it will return 
    *  the byte count transferred so far.
    *
    * Global variables:
    *  `$INSTANCE_NAME`_mstrRdBufIndex - used to current index within master read 
    *  buffer.
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
    *  Returns the amount of bytes that has been transferred with an 
    *  I2C_MasterWriteBuf command.
    *
    * Parameters:
    *  None
    *
    * Return:
    *  Byte count of transfer. If the transfer is not yet complete, it will return 
    *  the byte count transferred so far.
    *
    * Global variables:
    *  `$INSTANCE_NAME`_mstrWrBufIndex - used to current index within master write 
    *  buffer.
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
    *  Resets the read buffer pointer back to the first byte in the buffer.
    *
    * Parameters:
    *  None
    *
    * Return:
    *  None
    *
    * Global variables:
    *  `$INSTANCE_NAME`_mstrRdBufIndex - used to current index within master read 
    *   buffer.
    *  `$INSTANCE_NAME`_mstrStatus - used to store current status of I2C Master.
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
    *  Resets the write buffer pointer back to the first byte in the buffer.
    *
    * Parameters:
    *  None
    *
    * Return:
    *  None
    *
    * Global variables:
    *  `$INSTANCE_NAME`_mstrRdBufIndex - used to current index within master read 
    *   buffer.
    *  `$INSTANCE_NAME`_mstrStatus - used to store current status of I2C Master.
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
    *  None
    *
    * Return:
    *  None
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
    *  Returns I2C slave's communication status.
    *
    * Parameters:
    *  None
    *
    * Return:
    *  Current status of I2C slave.
    *
    * Global variables:
    *  `$INSTANCE_NAME`_slStatus  - used to store current status of I2C slave.
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
    *  Clears the read status flags and returns they values. No other status flags 
    *  are affected.
    *
    * Parameters:
    *  None
    *
    * Return:
    *  Current read status of I2C slave.
    *
    * Global variables:
    *  `$INSTANCE_NAME`_slStatus  - used to store current status of I2C slave.
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
    *  Clears the write status flags and returns they values. No other status flags
    *  are affected.
    *
    * Parameters:
    *  None
    *
    * Return:
    *  Current write status of I2C slave.
    *
    * Global variables:
    *  `$INSTANCE_NAME`_slStatus  - used to store current status of I2C slave.
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
    *  Sets the I2C slave address.
    *
    * Parameters:
    *  address: I2C slave address for the primary device. This value may be any 
    *  address between 0 and 127.
    *
    * Return:
    *  None
    *
    * Global variables:
    *  `$INSTANCE_NAME`_Address  - used to store I2C slave address for the primary 
    *  device when software address detect feature is used.
    *
    * Reentrant:
    *  No
    *
    *******************************************************************************/
    #if (`$INSTANCE_NAME`_ADDR_DECODE == `$INSTANCE_NAME`_HDWR_DECODE)
        void `$INSTANCE_NAME`_SlaveSetAddress(uint8 address) `=ReentrantKeil($INSTANCE_NAME . "_SlaveSetAddress")`
        {
            `$INSTANCE_NAME`_ADDR_REG = address & `$INSTANCE_NAME`_SLAVE_ADDR_MASK; /* Set I2C Address register */
        }
        
    #else
        void `$INSTANCE_NAME`_SlaveSetAddress(uint8 address)
        {
            `$INSTANCE_NAME`_slAddress = address & `$INSTANCE_NAME`_SLAVE_ADDR_MASK;  /* Set Address variable */ 
        }
        
    #endif  /* End (`$INSTANCE_NAME`_ADDR_DECODE == `$INSTANCE_NAME`_HDWR_DECODE) */    
     
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_SlaveInitReadBuf
    ********************************************************************************
    *
    * Summary:
    *  Sets the buffer pointer and size of the read buffer. This function also 
    *  resets the transfer count returned with the I2C_SlaveGetReadBufSize function.
    *
    * Parameters:
    *  readBuf:  Pointer to the data buffer to be read by the master.
    *  bufSize:  Size of the read buffer exposed to the I2C master.
    *
    * Return:
    *  None
    *
    * Global variables:
    *  `$INSTANCE_NAME`_slRdBufPtr - used to store pointer to slave read buffer.
    *  `$INSTANCE_NAME`_slRdBufSize - used to store salve read buffer size.
    *  `$INSTANCE_NAME`_slRdBufIndex - used to store current index within slave
    *  read buffer.
    *
    * Side Effects:
    *  If this function is called during a bus transaction, data from the previous 
    *  buffer location and the beginning of current buffer may be transmitted.
    *
    * Reentrant:
    *  No
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_SlaveInitReadBuf(uint8 * readBuf, uint8 bufSize)
    {
        `$INSTANCE_NAME`_slRdBufPtr   = (volatile uint8 *) readBuf;    /* Set buffer pointer */
        `$INSTANCE_NAME`_slRdBufSize  = bufSize;    /* Set buffer size */
        `$INSTANCE_NAME`_slRdBufIndex = 0u;         /* Clears buffer index */
    }
    
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_SlaveInitWriteBuf
    ********************************************************************************
    *
    * Summary:
    *  Sets the buffer pointer and size of the read buffer. This function also 
    *  resets the transfer count returned with the I2C_SlaveGetReadBufSize function.
    *
    * Parameters:
    *  writeBuf:  Pointer to the data buffer to be read by the master.
    *  bufSize:  Size of the buffer exposed to the I2C master.
    *
    * Return:
    *  None
    *
    * Global variables:
    *  `$INSTANCE_NAME`_slWrBufPtr - used to store pointer to slave write buffer.
    *  `$INSTANCE_NAME`_slWrBufSize - used to store salve write buffer size.
    *  `$INSTANCE_NAME`_slWrBufIndex - used to store current index within slave
    *  write buffer.
    *
    * Side Effects:
    *  If this function is called during a bus transaction, data from the previous 
    *  buffer location and the beginning of current buffer may be transmitted.
    *
    * Reentrant:
    *  No
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_SlaveInitWriteBuf(uint8 * writeBuf, uint8 bufSize)
    {
        `$INSTANCE_NAME`_slWrBufPtr   = (volatile uint8 *) writeBuf;  /* Set buffer pointer */
        `$INSTANCE_NAME`_slWrBufSize  = bufSize;   /* Set buffer size */
        `$INSTANCE_NAME`_slWrBufIndex = 0u;        /* Clears buffer index */
    }
    
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_SlaveGetReadBufSize
    ********************************************************************************
    *
    * Summary:
    *  Returns the number of bytes read by the I2C master since an 
    *  I2C_SlaveInitReadBuf or I2C_SlaveClearReadBuf function was executed. 
    *  The maximum return value will be the size of the read buffer.
    *
    * Parameters:
    *  None
    *
    * Return:
    *  Bytes read by master.
    *
    * Global variables:
    *  `$INSTANCE_NAME`_slRdBufIndex - used to store current index within slave
    *  read buffer.
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_SlaveGetReadBufSize(void) `=ReentrantKeil($INSTANCE_NAME . "_SlaveGetReadBufSize")`
    {
        return (`$INSTANCE_NAME`_slRdBufIndex);
    }
    
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_SlaveGetWriteBufSize
    ********************************************************************************
    *
    * Summary:
    *  Returns the number of bytes written by the I2C master since an 
    *  I2C_SlaveInitWriteBuf or I2C_SlaveClearWriteBuf function was executed.
    *  The maximum return value will be the size of the write buffer.
    *
    * Parameters:
    *  None
    *
    * Return:
    *  Bytes written by master.
    *
    * Global variables:
    *  `$INSTANCE_NAME`_slWrBufIndex - used to store current index within slave
    *  write buffer.
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_SlaveGetWriteBufSize(void) `=ReentrantKeil($INSTANCE_NAME . "_SlaveGetWriteBufSize")`
    {
        return (`$INSTANCE_NAME`_slWrBufIndex);
    }
    
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_SlaveClearReadBuf
    ********************************************************************************
    *
    * Summary:
    *  Resets the read pointer to the first byte in the read buffer. The next byte 
    *  read by the master will be the first byte in the read buffer.
    *
    * Parameters:
    *  None
    *
    * Return:
    *  None
    *
    * Global variables:
    *  `$INSTANCE_NAME`_slRdBufIndex - used to store current index within slave
    *  read buffer.
    *
    * Reentrant:
    *  No
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_SlaveClearReadBuf(void)
    {
        `$INSTANCE_NAME`_slRdBufIndex = 0u;
    }
    
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_SlaveClearRxBuf
    ********************************************************************************
    *
    * Summary:
    *  Resets the write pointer to the first byte in the write buffer. The next byte
    *  written by the master will be the first byte in the write buffer.
    *
    * Parameters:
    *  None
    *
    * Return:
    *  None
    *
    * Global variables:
    *  `$INSTANCE_NAME`_slWrBufIndex - used to store current index within slave
    *  write buffer.
    *
    * Reentrant:
    *  No
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_SlaveClearWriteBuf(void)
    {
        `$INSTANCE_NAME`_slWrBufIndex = 0u;
    }
    
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
#endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE) */


/* [] END OF FILE */
