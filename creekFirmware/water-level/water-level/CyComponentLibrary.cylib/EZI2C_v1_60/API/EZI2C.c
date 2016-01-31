/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file contains the setup, control and status commands for the EzI2C
*  component.  Actual protocol and operation code resides in the interrupt
*  service routine file.
*
* Note:
*
********************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"
#include "CyLib.h"

/* Current state of I2C state machine */
extern volatile uint8 `$INSTANCE_NAME`_curState;

/* Status byte */
extern volatile uint8 `$INSTANCE_NAME`_curStatus;

/* Pointer to data exposed to I2C Master */
extern volatile uint8 * `$INSTANCE_NAME`_dataPtrS1;

#if(`$INSTANCE_NAME`_SUBADDR_WIDTH == `$INSTANCE_NAME`_SUBADDR_8BIT)

    /* Offset for read and write operations, set at each write sequence */
    extern volatile uint8   `$INSTANCE_NAME`_rwOffsetS1;

    /* Points to next value to be read or written */
    extern volatile uint8   `$INSTANCE_NAME`_rwIndexS1;

    /* Offset where data is read only */
    extern volatile uint8   `$INSTANCE_NAME`_wrProtectS1;

    /* Size of array between 1 and 255 */
    extern volatile uint8   `$INSTANCE_NAME`_bufSizeS1;

#else   /* 16 bit sub-address */

    /* Offset for read and write operations, set at each write sequence */
    extern volatile uint16  `$INSTANCE_NAME`_rwOffsetS1;

    /* Points to next value to be read or written */
    extern volatile uint16  `$INSTANCE_NAME`_rwIndexS1;

    /* Offset where data is read only */
    extern volatile uint16  `$INSTANCE_NAME`_wrProtectS1;

    /* Size of array between 1 and 65535 */
    extern volatile uint16  `$INSTANCE_NAME`_bufSizeS1;

#endif  /* End of (`$INSTANCE_NAME`_SUBADDR_WIDTH == `$INSTANCE_NAME`_SUBADDR_8BIT) */


#if(`$INSTANCE_NAME`_ADDRESSES == `$INSTANCE_NAME`_TWO_ADDRESSES)

    /* Pointer to data exposed to I2C Master */
    extern volatile uint8 * `$INSTANCE_NAME`_dataPtrS2;

    /* Software address compare 1 */
    extern volatile uint8   `$INSTANCE_NAME`_addrS1;

    /* Software address compare 2 */
    extern volatile uint8   `$INSTANCE_NAME`_addrS2;

    #if(`$INSTANCE_NAME`_SUBADDR_WIDTH == `$INSTANCE_NAME`_SUBADDR_8BIT)

        /* Offset for read and write operations, set at each write sequence */
        extern volatile uint8   `$INSTANCE_NAME`_rwOffsetS2;

        /* Points to next value to be read or written */
        extern volatile uint8   `$INSTANCE_NAME`_rwIndexS2;

        /* Offset where data is read only */
        extern volatile uint8   `$INSTANCE_NAME`_wrProtectS2;

        /* Size of array between 1 and 255 */
        extern volatile uint8   `$INSTANCE_NAME`_bufSizeS2;

    #else /* 16 bit subaddress */

        /* Offset for read and write operations, set at each write sequence */
        extern volatile uint16  `$INSTANCE_NAME`_rwOffsetS2;

        /* Points to next value to be read or written */
        extern volatile uint16  `$INSTANCE_NAME`_rwIndexS2;

        /* Offset where data is read only */
        extern volatile uint16  `$INSTANCE_NAME`_wrProtectS2;

        /* Size of array between 1 and 65535 */
        extern volatile uint16  `$INSTANCE_NAME`_bufSizeS2;

    #endif /* End of (`$INSTANCE_NAME`_SUBADDR_WIDTH == `$INSTANCE_NAME`_SUBADDR_8BIT) */

#endif /* End of (`$INSTANCE_NAME`_ADDRESSES == `$INSTANCE_NAME`_TWO_ADDRESSES) */

uint8 `$INSTANCE_NAME`_initVar = 0u;


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
*
* Summary:
*  Starts the component and enables the interupt. If this function is called at
*  first (or `$INSTANCE_NAME`_initVar was cleared, then `$INSTANCE_NAME`_Init()
*  function is called and all offsets and pointers are reset. Anyway, the 
*  state machine state is set to IDLE, status variable is cleared and the
*  interrupt is enabled.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Global variables:
*  `$INSTANCE_NAME`_initVar - is used to indicate initial configuration
*  of this component.  The variable is initialized to zero and set to 1
*  the first time `$INSTANCE_NAME`_Start() is called. This allows for component
*  initialization without re-initialization in all subsequent calls
*  to the `$INSTANCE_NAME`_Start() routine. 
*
*  `$INSTANCE_NAME`_dataPtrS1 global variable, which stores pointer to the
*  data exposed to an I2C master for the first slave address is reset if
*  `$INSTANCE_NAME`_initVar is set 0 by `$INSTANCE_NAME`_initVar function call.
*
*  `$INSTANCE_NAME`_rwOffsetS1 - global variable, which stores offset for read
*  and write operations, is set at each write sequence of the first slave
*  address is reset if `$INSTANCE_NAME`_initVar is 0, by 
*  `$INSTANCE_NAME`_initVar function call.
*
*  `$INSTANCE_NAME`_rwIndexS1 - global variable, which stores pointer to the
*  next value to be read or written for the first slave address is reset if
*  `$INSTANCE_NAME`_initVar is 0, by `$INSTANCE_NAME`_initVar function call.
*
* `$INSTANCE_NAME`_wrProtectS1 - global variable, which stores offset where data
*  is read only for the first slave address is reset if
*  `$INSTANCE_NAME`_initVar is 0, by `$INSTANCE_NAME`_initVar function call.
*
* `$INSTANCE_NAME`_bufSizeS1 - global variable, which stores size of data array
*  exposed to an I2C master for the first slave address is reset if
*  `$INSTANCE_NAME`_initVar is 0, by `$INSTANCE_NAME`_initVar function call.
*
*  `$INSTANCE_NAME`_dataPtrS2 - global variable, which stores pointer to the
*  data exposed to an I2C master for the second slave address is reset if
*  `$INSTANCE_NAME`_initVar is 0, by `$INSTANCE_NAME`_initVar function call.
*
*  `$INSTANCE_NAME`_rwOffsetS2 - global variable, which stores offset for read
*  and write operations, is set at each write sequence of the second slave
*  device is reset if `$INSTANCE_NAME`_initVar is 0, by `$INSTANCE_NAME`_initVar
*  function call.
*
*  `$INSTANCE_NAME`_rwIndexS2 - global variable, which stores pointer to the 
*  next value to be read or written for the second slave address is reset if
*  `$INSTANCE_NAME`_initVar is 0, by `$INSTANCE_NAME`_initVar function call.
*
* `$INSTANCE_NAME`_wrProtectS2 - global variable, which stores offset where data
*  is read only for the second slave address is reset if
*  `$INSTANCE_NAME`_initVar is 0, by `$INSTANCE_NAME`_initVar function call.
*
* `$INSTANCE_NAME`_bufSizeS2 - global variable, which stores size of data array
*  exposed to an I2C master for the second slave address is reset if
*  `$INSTANCE_NAME`_initVar is 0, by `$INSTANCE_NAME`_initVar function call.
*
* Side Effects:
*  This component automatically enables it's interrupt. If I2C is enabled
*  without the interrupt enabled, it could lock up the I2C bus.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Start(void)
{
    if(0u == `$INSTANCE_NAME`_initVar)
    {
        /* Initialize component's parameters */
        `$INSTANCE_NAME`_Init();

        /* Set init flag */
        `$INSTANCE_NAME`_initVar = 1u;
    }

    /* Enable slave mode for the device */
    `$INSTANCE_NAME`_Enable();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
*
* Summary:
*  Disable the I2C block's slave operation and the corresponding interrupt.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`
{
    uint8 interruptState;
    
    /* Disable Interrupt */
    `$INSTANCE_NAME`_DisableInt();

    /* Enter critical section */
    interruptState = CyEnterCriticalSection();

    /* Disable I2C block in Active mode template */
    `$INSTANCE_NAME`_PM_ACT_CFG_REG &= ~`$INSTANCE_NAME`_ACT_PWR_EN;
    
    /* Disable I2C block in Alternate Active (Standby) mode template */
    `$INSTANCE_NAME`_PM_STBY_CFG_REG &= ~`$INSTANCE_NAME`_STBY_PWR_EN;
    
    /* Exit critical section */
    CyExitCriticalSection(interruptState);
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
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_EnableInt(void) `=ReentrantKeil($INSTANCE_NAME . "_EnableInt")`
{
    /* Enable interrupt */
    CyIntEnable(`$INSTANCE_NAME`_ISR_NUMBER);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_DisableInt
********************************************************************************
*
* Summary:
*  Disable I2C interrupts. Normally this function is not required since the
*  Stop function disables the interrupt. If the I2C interrupt is disabled while
*  the I2C master is still running, it may cause the I2C bus to lock up.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Side Effects:
*  If the I2C interrupt is disabled and the master is addressing the current
*  slave, the bus will be locked until the interrupt is re-enabled.
*
*******************************************************************************/
void `$INSTANCE_NAME`_DisableInt(void) `=ReentrantKeil($INSTANCE_NAME . "_DisableInt")`
{
    /* Disable interrupt */
    CyIntDisable(`$INSTANCE_NAME`_ISR_NUMBER);
}

#if(`$INSTANCE_NAME`_ADDRESSES == `$INSTANCE_NAME`_TWO_ADDRESSES)
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_SetAddress1
    ********************************************************************************
    *
    * Summary:
    *  This function sets the main address of this I2C slave device. This value may
    *  be any value between 0 and 127.
    *
    * Parameters:
    *  address:  The 7-bit slave address between 0 and 127.
    *
    * Return:
    *  None.
    *
    * Global variables:
    *  `$INSTANCE_NAME`_addrS1 - the new slave address for the first slave device is
    *  saved in it, if the component is configured to act as two slave devices.
    *
    * Reentrant:
    *  No.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_SetAddress1(uint8 address)
    {
        /* Address is stored in variable */
        `$INSTANCE_NAME`_addrS1  = address & `$INSTANCE_NAME`_SADDR_MASK;
    }

#else

    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_SetAddress1
    ********************************************************************************
    *
    * Summary:
    *  This function sets the main address of this I2C slave device. This value may
    *  be any value between 0 and 127.
    *
    * Parameters:
    *  address:  The 7-bit slave address between 0 and 127.
    *
    * Return:
    *  None.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_SetAddress1(uint8 address) `=ReentrantKeil($INSTANCE_NAME . "_SetAddress1")`
    {
        /* Address is stored in hardware */
        `$INSTANCE_NAME`_ADDR_REG = address & `$INSTANCE_NAME`_SADDR_MASK;
    }

#endif /* End of (`$INSTANCE_NAME`_ADDRESSES == `$INSTANCE_NAME`_TWO_ADDRESSES) */



/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetAddress1
********************************************************************************
*
* Summary:
*  Returns address of the first slave device.
*
* Parameters:
*  None.
*
* Return:
*  If the component is configured to has two slave addresses than primary
*  address is returned, otherwise address from the the address register is
*  returned.
*
* Global variables:
*  `$INSTANCE_NAME`_addrS1 - if component is configured to has two slave
*  addresses than primary address is saved here, otherwise address is written to
*  the register.
*
* Reentrant:
*  No.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_GetAddress1(void)
{
    /* Get 1st slave address */
    #if(`$INSTANCE_NAME`_ADDRESSES == `$INSTANCE_NAME`_TWO_ADDRESSES)

        /* Return address from variable */
        return(`$INSTANCE_NAME`_addrS1);

    #else

        /* Return address from hardware */
        return(`$INSTANCE_NAME`_ADDR_REG);

    #endif /* End of (`$INSTANCE_NAME`_ADDRESSES == `$INSTANCE_NAME`_TWO_ADDRESSES) */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetActivity
********************************************************************************
*
* Summary:
*  This function returns a nonzero value if the I2C read or write cycle
*  occurred since the last time this function was called.  The activity
*  flag resets to zero at the end of this function call.
*  The Read and Write busy flags are cleared when read, but the "BUSY"
*  flag is only cleared by an I2C Stop.
*
* Parameters:
*  None.
*
* Return:
*  A non-zero value is returned if activity is detected:
*   `$INSTANCE_NAME`_STATUS_READ1   Set if Read sequence is detected for first
*                                   address. Cleared when status read.
*
*   `$INSTANCE_NAME`_STATUS_WRITE1  Set if Write sequence is detected for first
*                                   address. Cleared when status read.
*
*   `$INSTANCE_NAME`_STATUS_READ2   Set if Read sequence is detected for second
*                                   address (if enabled). Cleared when status 
*                                   read.
*
*   `$INSTANCE_NAME`_STATUS_WRITE2  Set if Write sequence is detected for second 
*                                   address (if enabled). Cleared when status
*                                   read.
*
*   `$INSTANCE_NAME`_STATUS_BUSY    Set if Start detected, cleared when stop 
*                                   detected.
*
*   `$INSTANCE_NAME`_STATUS_ERR     Set when I2C hardware detected, cleared
*                                   when status read.
*
* Global variables:
*  `$INSTANCE_NAME`_curStatus - global variable, which stores the current
*  component status.
*
* Reentrant:
*  No.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_GetActivity(void)
{
    uint8 tmpStatus;

    tmpStatus = `$INSTANCE_NAME`_curStatus;

    /* Clear status, but no Busy one */
    `$INSTANCE_NAME`_curStatus &= `$INSTANCE_NAME`_STATUS_BUSY;

    return(tmpStatus);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetBuffer1
********************************************************************************
*
* Summary:
*  This function sets the buffer, size of the buffer, and the R/W boundry
*  for the memory buffer.
*
* Parameters:
*  size:  Size of the buffer in bytes.
*
*  rwBoundry: Sets how many bytes are writable in the beginning of the buffer.
*  This value must be less than or equal to the buffer size.
*
*  dataPtr:  Pointer to the data buffer.
*
* Return:
*  None.
*
* Global variables:
*  `$INSTANCE_NAME`_dataPtrS1 - stores pointer to the data exposed to an I2C
*  master for the first slave address is modified with the the new pointer to
*  data, passed by function parameter.
*
*  `$INSTANCE_NAME`_rwOffsetS1 - stores offset for read and write operations, is
*  modified at each write sequence of the first slave address is reset.
*
*  `$INSTANCE_NAME`_rwIndexS1 - stores pointer to the next value to be read or
*  written for the first slave address is set to 0.
*
* Reentrant:
*  No.
*
* Side Effects:
*  It is recommended to disable component interrupt before calling this function
*  and enable it afterwards for the proper component operation.
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetBuffer1(uint16 bufSize, uint16 rwBoundry, void * dataPtr)
{
    /* Set pointer to data and clear index and offset. */
    `$INSTANCE_NAME`_dataPtrS1   = dataPtr;
    `$INSTANCE_NAME`_rwOffsetS1  = 0u;
    `$INSTANCE_NAME`_rwIndexS1   = 0u;

    #if(`$INSTANCE_NAME`_SUBADDR_WIDTH == `$INSTANCE_NAME`_SUBADDR_8BIT)
        `$INSTANCE_NAME`_bufSizeS1   = (uint8) bufSize;
        `$INSTANCE_NAME`_wrProtectS1 = (uint8) rwBoundry;
    #else
        `$INSTANCE_NAME`_bufSizeS1   = bufSize;
        `$INSTANCE_NAME`_wrProtectS1 = rwBoundry;
    #endif /* End of (`$INSTANCE_NAME`_SUBADDR_WIDTH == `$INSTANCE_NAME`_SUBADDR_8BIT) */
}


#if (`$INSTANCE_NAME`_ADDRESSES == `$INSTANCE_NAME`_ONE_ADDRESS)
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_SlaveSetSleepMode
    ********************************************************************************
    *
    * Summary:
    *  Disables the run time I2C regulator and enables the sleep Slave I2C.
    *  Should be called just prior to entering sleep. This function is only
    *  provided if a single I2C address is used.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  None.
    *
    * Side Effects:
    *  The I2C interrupt will be disabled if Wake up from Sleep mode option is
    *  enabled (only for PSoC3 ES3).
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_SlaveSetSleepMode(void) `=ReentrantKeil($INSTANCE_NAME . "_SlaveSetSleepMode")`
    {
        /* PSoC3 ES3 or later, PSoC5 ES2 or later*/
        #if(`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2)
           
            /* if I2C block will be used as wake up source */
            #if(`$INSTANCE_NAME`_ENABLE_WAKEUP == 1u)
                
                uint8 interruptState;
                
                /* Enter critical section */
                interruptState = CyEnterCriticalSection();
                
                /* Enable the I2C regulator backup */
                `$INSTANCE_NAME`_PWRSYS_CR1_REG |= `$INSTANCE_NAME`_PWRSYS_CR1_I2C_BACKUP;
                
                /* Exit critical section */
                CyExitCriticalSection(interruptState);
                
                /* Set force nack before putting the device to power off mode.
                *  It is cleared on wake up.
                */
                `$INSTANCE_NAME`_XCFG_REG |= `$INSTANCE_NAME`_XCFG_FORCE_NACK;
                while(0u == (`$INSTANCE_NAME`_XCFG_REG & `$INSTANCE_NAME`_XCFG_SLEEP_READY))
                {
                    /* Waits for ongoing transaction to be completed. */
                }
                
                /* Disable interrupt for proper wake up procedure */
                `$INSTANCE_NAME`_DisableInt();
            
            #endif  /* End of (`$INSTANCE_NAME`_ENABLE_WAKEUP == 1u) */

        #endif  /* End of (`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2) */
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_SlaveSetWakeMode
    ********************************************************************************
    *
    * Summary:
    *  Disables the sleep EzI2C slave and re-enables the run time I2C.  Should be
    *  called just after awaking from sleep.  Must preserve address to continue.
    *  This function is only provided if a single I2C address is used.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  None.
    *
    * Side Effects:
    *  The I2C interrupt will be enabled if Wake up from Sleep mode option is
    *  enabled (only for PSoC3 ES3).
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_SlaveSetWakeMode(void) `=ReentrantKeil($INSTANCE_NAME . "_SlaveSetWakeMode")`
    {
        /* PSoC3 ES3 or later, PSoC5 ES2 or later*/
        #if(`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2)
            
            /* if I2C block will be used as wake up source */
            #if(`$INSTANCE_NAME`_ENABLE_WAKEUP == 1u)
                
                uint8 interruptState;
            
                /* Enter critical section */
                interruptState = CyEnterCriticalSection();
                
                /* Disable the I2C regulator backup */
                `$INSTANCE_NAME`_PWRSYS_CR1_REG &= ~`$INSTANCE_NAME`_PWRSYS_CR1_I2C_BACKUP;
                
                /* Exit critical section */
                CyExitCriticalSection(interruptState);
                
                /* Enable interrupt. The ISR is supposed to ready to be executed. */
                `$INSTANCE_NAME`_EnableInt();

            #endif  /* End of (`$INSTANCE_NAME`_ENABLE_WAKEUP == 1u) */ 

        #endif  /* End of (`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2) */ 
    }
    
#endif /* End of (`$INSTANCE_NAME`_ADDRESSES == `$INSTANCE_NAME`_ONE_ADDRESS) */


#if (`$INSTANCE_NAME`_ADDRESSES == `$INSTANCE_NAME`_TWO_ADDRESSES)
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_SetBuffer2
    ********************************************************************************
    *
    * Summary:
    *  This function sets the buffer pointer, size and read/write area for the
    *  second slave data. This is the data that is exposed to the I2C Master
    *  for the second I2C address. This function is only provided if two I2C
    *  addresses have been selected in the user parameters.
    *
    * Parameters:
    *  bufSize:  Size of the buffer exposed to the I2C master.
    *
    *  rwBoundry: Sets how many bytes are readable and writable by the the I2C
    *  master. This value must be less than or equal to the buffer size. Data
    *  located at offset rwBoundry and above are read only.
    *
    *  dataPtr:  This is a pointer to the data array or structure that is used
    *  for the I2C data buffer.
    *
    * Return:
    *  None.
    *
    * Global variables:
    *  `$INSTANCE_NAME`_dataPtrS2 - stores pointer to the data exposed to an I2C
    *  master for the second slave address is modified with the the new pointer to
    *  data, passed by function parameter.
    *
    *  `$INSTANCE_NAME`_rwOffsetS2 - stores offset for read and write operations,
    *  is modified at each write sequence of the second slave address is reset.
    *
    *  `$INSTANCE_NAME`_rwIndexS2 - stores pointer to the next value to be read or
    *  written for the second slave address is set to 0.
    *
    * Reentrant:
    *  No.
    *
    * Side Effects:
    *  It is recommended to disable component interrupt before calling this
    *  function and enable it afterwards for the proper component operation.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_SetBuffer2(uint16 bufSize, uint16 rwBoundry, void * dataPtr)
    {
        /* Set pointer to data and clear index and offset. */
        `$INSTANCE_NAME`_dataPtrS2   = dataPtr;
        `$INSTANCE_NAME`_rwOffsetS2  = 0u;
        `$INSTANCE_NAME`_rwIndexS2   = 0u;

        #if(`$INSTANCE_NAME`_SUBADDR_WIDTH == `$INSTANCE_NAME`_SUBADDR_8BIT)
            `$INSTANCE_NAME`_bufSizeS2   = (uint8) bufSize;
            `$INSTANCE_NAME`_wrProtectS2 = (uint8) rwBoundry;
        #else
            `$INSTANCE_NAME`_bufSizeS2   = bufSize;
            `$INSTANCE_NAME`_wrProtectS2 = rwBoundry;
        #endif /* End of (`$INSTANCE_NAME`_SUBADDR_WIDTH == `$INSTANCE_NAME`_SUBADDR_8BIT) */
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_SetAddress2
    ********************************************************************************
    *
    * Summary:
    *  Sets the I2C slave address for the second device. This value may be any 
    *  value between 0 and 127. This function is only provided if two I2C
    *  addresses have been selected in the user parameters.
    *
    * Parameters:
    *  address:  The 7-bit slave address between 0 and 127.
    *
    * Return:
    *  None.
    *
    * Global variables:
    *  `$INSTANCE_NAME`_addrS2 - the secondary slave address is modified.
    *
    * Reentrant:
    *  No.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_SetAddress2(uint8 address)
    {
        /* Set slave address */
        `$INSTANCE_NAME`_addrS2  = address & `$INSTANCE_NAME`_SADDR_MASK;
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_GetAddress2
    ********************************************************************************
    *
    * Summary:
    *  Returns the I2C slave address for the second device. This function is only
    *  provided if two I2C addresses have been selected in the user parameters.
    *
    * Parameters:
    *  `$INSTANCE_NAME`_addrS2 - global variable, which stores the second I2C 
    *   address.
    *
    * Return:
    *  The secondary I2C slave address.
    *
    * Global variables:
    *  `$INSTANCE_NAME`_addrS2 - the secondary slave address is used.
    *
    * Reentrant:
    *  No.
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_GetAddress2(void)
    {
        return(`$INSTANCE_NAME`_addrS2);
    }

#endif  /* End of (`$INSTANCE_NAME`_ADDRESSES == `$INSTANCE_NAME`_TWO_ADDRESSES) */


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Init
********************************************************************************
*
* Summary:
*  Initializes/restores default EZI2C configuration provided with customizer.
*  Usually called in `$INSTANCE_NAME`_Start().
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Global variables:
*  `$INSTANCE_NAME`_addrS1 - the new slave address for the first slave device is
*   saved.
*
*  `$INSTANCE_NAME`_addrS2 - the new slave address for the second slave device
*   is saved, if EzI2C component is configured for two slave addresses.
*
* Reentrant:
*  No.
*
* Side Effects:
*  All changes applied by API to the component's configuration will be reset. 
*
*******************************************************************************/
void `$INSTANCE_NAME`_Init(void)
{
    /* PSoC3 ES2 or early, PSoC5 ES1*/
    #if(`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
    
        uint8 clkSel;
        
    #endif  /* End of (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1) */

    /* Clear Status register */
    `$INSTANCE_NAME`_CSR_REG  = 0x00u;

    /* PSoC3 ES2 or early, PSoC5 ES1*/
    #if(`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
        
        /* Enable I2C block's slave operation */
        `$INSTANCE_NAME`_CFG_REG |= `$INSTANCE_NAME`_CFG_EN_SLAVE;
        
        /* The resolution of generated I2C bus SCL frequencies is reduced. */

        /* Temprorary set CLK divider to 16 */
        `$INSTANCE_NAME`_CLKDIV_REG  = `$INSTANCE_NAME`_CLK_DIV_16;

        /* Set clock divider for the I2C bus clock to be the closest HIGHER 
        *  clock to ensure it can work with an ideal master clock.
        */
        for(clkSel = 0u; clkSel <= 6u; clkSel++ )
        {
            if((1u << clkSel) >= `$INSTANCE_NAME`_DEFAULT_CLKDIV)
            {
                `$INSTANCE_NAME`_CLKDIV_REG  = clkSel;
                break;
            }
        }
        
        /* Set clock rate to Fast mode 400k. This will give the Clock 
        *  divider sufficient range to work bewteen 50 and 400 kbps.
        */
        `$INSTANCE_NAME`_CFG_REG  |= `$INSTANCE_NAME`_CFG_CLK_RATE_400;

    /* PSoC3 ES3 or later, PSoC5 ES2 or later*/
    #elif(`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2)
    
        /* Enable I2C block's slave operation.
        *  These revisions require slave to be enabled for registers to be
        *  written.
        */
        `$INSTANCE_NAME`_CFG_REG |= `$INSTANCE_NAME`_CFG_EN_SLAVE;
        
        /* 8 LSB bits of the 10-bit are written with the divide factor */
		`$INSTANCE_NAME`_CLKDIV1_REG = LO8(`$INSTANCE_NAME`_DIVIDE_FACTOR);

        /* 2 MSB bits of the 10-bit are written with the divide factor */
		`$INSTANCE_NAME`_CLKDIV2_REG = HI8(`$INSTANCE_NAME`_DIVIDE_FACTOR);
		
        /* Define clock rate */
        if(`$INSTANCE_NAME`_BUS_SPEED <= `$INSTANCE_NAME`_BUS_SPEED_50KHZ)
        {   /* 50 kHz - 32 samples/bit */
            `$INSTANCE_NAME`_CFG_REG |= `$INSTANCE_NAME`_CFG_CLK_RATE;
        }
        else
        {   /* 100kHz or 400kHz - 16 samples/bit */
            `$INSTANCE_NAME`_CFG_REG &= ~`$INSTANCE_NAME`_CFG_CLK_RATE;
        }
        
        /* if I2C block will be used as wake up source, this availabe only for
        * PSoC3 ES3, PSoC5 ES2 and later revisions. Refer to errata.
        */
        #if(1u == `$INSTANCE_NAME`_ENABLE_WAKEUP)

            /* Configure I2C address match to act as wake-up source */
           `$INSTANCE_NAME`_XCFG_REG |= `$INSTANCE_NAME`_XCFG_I2C_ON;

            /* Process sio_select and pselect */
            #if(`$INSTANCE_NAME`_ADDRESSES == `$INSTANCE_NAME`_ONE_ADDRESS)
                if(`$INSTANCE_NAME`__ANY != `$INSTANCE_NAME`_BUS_PORT)
                {
                    /* SCL and SDA lines get their inputs from SIO block */
                    `$INSTANCE_NAME`_CFG_REG |= `$INSTANCE_NAME`_CFG_PSELECT;

                    if(`$INSTANCE_NAME`__I2C0 == `$INSTANCE_NAME`_BUS_PORT)
                    {
                        /* SCL and SDA lines get their inputs from SIO1 */
                        `$INSTANCE_NAME`_CFG_REG &= ~`$INSTANCE_NAME`_CFG_SIO_SELECT;
                    }
                    else /* SIO2 */
                    {
                        /* SCL and SDA lines get their inputs from SIO2 */
                        `$INSTANCE_NAME`_CFG_REG |= `$INSTANCE_NAME`_CFG_SIO_SELECT;
                    }
                }
                else    /* GPIO is used */
                {
                    /* SCL and SDA lines get their inputs from GPIO module. */
                    `$INSTANCE_NAME`_CFG_REG &= ~`$INSTANCE_NAME`_CFG_PSELECT;
                }
            #endif  /* End if (`$INSTANCE_NAME`_ADDRESSES == `$INSTANCE_NAME`_ONE_ADDRESS) */

        #endif /* End of (1u == `$INSTANCE_NAME`_ENABLE_WAKEUP) */

    #endif  /* End of (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1) */


    #if(`$INSTANCE_NAME`_ADDRESSES == `$INSTANCE_NAME`_ONE_ADDRESS)

        /* Set default slave address */
        `$INSTANCE_NAME`_ADDR_REG  = `$INSTANCE_NAME`_DEFAULT_ADDR1;

        /* Turn on hardware address detection */
        `$INSTANCE_NAME`_XCFG_REG  |= `$INSTANCE_NAME`_XCFG_HDWR_ADDR_EN;

    #else   /* Two devices */

        /* Set default slave addresses */
        `$INSTANCE_NAME`_addrS1  = `$INSTANCE_NAME`_DEFAULT_ADDR1;
        `$INSTANCE_NAME`_addrS2  = `$INSTANCE_NAME`_DEFAULT_ADDR2;

    #endif  /* End of (`$INSTANCE_NAME`_ADDRESSES == `$INSTANCE_NAME`_ONE_ADDRESS) */
    
    /* Reset offsets and pointers */
    `$INSTANCE_NAME`_dataPtrS1 = (uint8 *)0x0000u;
    `$INSTANCE_NAME`_rwOffsetS1 = 0u;
    `$INSTANCE_NAME`_rwIndexS1 = 0u;
    `$INSTANCE_NAME`_wrProtectS1 = 0u;
    `$INSTANCE_NAME`_bufSizeS1 = 0u;

    /* Reset offsets and pointers for 2nd slave address */
    #if(`$INSTANCE_NAME`_ADDRESSES == `$INSTANCE_NAME`_TWO_ADDRESSES)
        `$INSTANCE_NAME`_dataPtrS2 = (uint8 *)0x0000u;
        `$INSTANCE_NAME`_rwOffsetS2 = 0u;
        `$INSTANCE_NAME`_rwIndexS2 = 0u;
        `$INSTANCE_NAME`_wrProtectS2 = 0u;
        `$INSTANCE_NAME`_bufSizeS2 = 0u;
    #endif  /* End of (`$INSTANCE_NAME`_ADDRESSES == `$INSTANCE_NAME`_TWO_ADDRESSES) */
    
    /* Enable the I2C block clock */
    `$INSTANCE_NAME`_XCFG_REG  |= `$INSTANCE_NAME`_XCFG_CLK_EN;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Enable
********************************************************************************
*
* Summary: 
*  Enables the I2C block operation, sets interrupt priority, sets 
*  interrupt vector, clears pending interrupts and enables interrupts. Clears
*  status variables and reset state machine variable.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Global variables:
*  `$INSTANCE_NAME`_curStatus - this global variable are cleared, it stores the
*  current component status.
*
* `$INSTANCE_NAME`_curState - global variable are cleared, it stores the current
*  state of the state machine.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Enable(void)
{
    uint8 interruptState;
    
    /* Enter critical section */
    interruptState = CyEnterCriticalSection();

    /* Enable I2C block in Active mode template */
    `$INSTANCE_NAME`_PM_ACT_CFG_REG |= `$INSTANCE_NAME`_ACT_PWR_EN;
    
    /* Enable I2C block in Alternate Active (Standby) mode template */
    `$INSTANCE_NAME`_PM_STBY_CFG_REG |= `$INSTANCE_NAME`_STBY_PWR_EN;
    
    /* Exit critical section */
    CyExitCriticalSection(interruptState);
    
    /* Set the interrupt priority */
    CyIntSetPriority(`$INSTANCE_NAME`_ISR_NUMBER, `$INSTANCE_NAME`_ISR_PRIORITY);

    /* Set the interrupt vector */
    CyIntSetVector(`$INSTANCE_NAME`_ISR_NUMBER, `$INSTANCE_NAME`_ISR);

    /* Clear any pending interrupt */
    CyIntClearPending(`$INSTANCE_NAME`_ISR_NUMBER);
    
    /* Reset State Machine to IDLE */
    `$INSTANCE_NAME`_curState = `$INSTANCE_NAME`_SM_IDLE;

    /* Clear Status variable */
    `$INSTANCE_NAME`_curStatus = 0x00u;
    
    /* Enable the interrupt */
    `$INSTANCE_NAME`_EnableInt();
}


/* [] END OF FILE */
