/*******************************************************************************
* File Name: i2c.c
* Version 1.70
*
* Description:
*  This file contains the setup, control and status commands for the EzI2C
*  component.  Actual protocol and operation code resides in the interrupt
*  service routine file.
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "i2c.h"
#include "CyLib.h"

/* Current state of I2C state machine */
extern volatile uint8 i2c_curState;

/* Status byte */
extern volatile uint8 i2c_curStatus;

/* Pointer to data exposed to I2C Master */
extern volatile uint8 * i2c_dataPtrS1;

#if(CY_PSOC3)
    extern i2c_BACKUP_STRUCT  i2c_backup;
#endif  /* (CY_PSOC3) */

#if(i2c_SUBADDR_WIDTH == i2c_SUBADDR_8BIT)

    /* Offset for read and write operations, set at each write sequence */
    extern volatile uint8   i2c_rwOffsetS1;

    /* Points to next value to be read or written */
    extern volatile uint8   i2c_rwIndexS1;

    /* Offset where data is read only */
    extern volatile uint8   i2c_wrProtectS1;

    /* Size of array between 1 and 255 */
    extern volatile uint8   i2c_bufSizeS1;

#else   /* 16 bit sub-address */

    /* Offset for read and write operations, set at each write sequence */
    extern volatile uint16  i2c_rwOffsetS1;

    /* Points to next value to be read or written */
    extern volatile uint16  i2c_rwIndexS1;

    /* Offset where data is read only */
    extern volatile uint16  i2c_wrProtectS1;

    /* Size of array between 1 and 65535 */
    extern volatile uint16  i2c_bufSizeS1;

#endif  /* (i2c_SUBADDR_WIDTH == i2c_SUBADDR_8BIT) */


#if(i2c_ADDRESSES == i2c_TWO_ADDRESSES)

    /* Pointer to data exposed to I2C Master */
    extern volatile uint8 * i2c_dataPtrS2;

    /* Software address compare 1 */
    extern volatile uint8   i2c_addrS1;

    /* Software address compare 2 */
    extern volatile uint8   i2c_addrS2;

    #if(i2c_SUBADDR_WIDTH == i2c_SUBADDR_8BIT)

        /* Offset for read and write operations, set at each write sequence */
        extern volatile uint8   i2c_rwOffsetS2;

        /* Points to next value to be read or written */
        extern volatile uint8   i2c_rwIndexS2;

        /* Offset where data is read only */
        extern volatile uint8   i2c_wrProtectS2;

        /* Size of array between 1 and 255 */
        extern volatile uint8   i2c_bufSizeS2;

    #else /* 16 bit subaddress */

        /* Offset for read and write operations, set at each write sequence */
        extern volatile uint16  i2c_rwOffsetS2;

        /* Points to next value to be read or written */
        extern volatile uint16  i2c_rwIndexS2;

        /* Offset where data is read only */
        extern volatile uint16  i2c_wrProtectS2;

        /* Size of array between 1 and 65535 */
        extern volatile uint16  i2c_bufSizeS2;

    #endif /* (i2c_SUBADDR_WIDTH == i2c_SUBADDR_8BIT) */

#endif /* (i2c_ADDRESSES == i2c_TWO_ADDRESSES) */

uint8 i2c_initVar = 0u;


/*******************************************************************************
* Function Name: i2c_Start
********************************************************************************
*
* Summary:
*  Starts the component and enables the interupt. If this function is called at
*  first (or i2c_initVar was cleared, then i2c_Init()
*  function is called and all offsets and pointers are reset. Anyway, the
*  state machine state is set to IDLE, status variable is cleared and the
*  interrupt is enabled.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global variables:
*  i2c_initVar - is used to indicate initial configuration
*  of this component.  The variable is initialized to zero and set to 1
*  the first time i2c_Start() is called. This allows for component
*  initialization without re-initialization in all subsequent calls
*  to the i2c_Start() routine.
*
*  i2c_dataPtrS1 global variable, which stores pointer to the
*  data exposed to an I2C master for the first slave address is reset if
*  i2c_initVar is set 0 by i2c_initVar function call.
*
*  i2c_rwOffsetS1 - global variable, which stores offset for read
*  and write operations, is set at each write sequence of the first slave
*  address is reset if i2c_initVar is 0, by
*  i2c_initVar function call.
*
*  i2c_rwIndexS1 - global variable, which stores pointer to the
*  next value to be read or written for the first slave address is reset if
*  i2c_initVar is 0, by i2c_initVar function call.
*
* i2c_wrProtectS1 - global variable, which stores offset where data
*  is read only for the first slave address is reset if
*  i2c_initVar is 0, by i2c_initVar function call.
*
* i2c_bufSizeS1 - global variable, which stores size of data array
*  exposed to an I2C master for the first slave address is reset if
*  i2c_initVar is 0, by i2c_initVar function call.
*
*  i2c_dataPtrS2 - global variable, which stores pointer to the
*  data exposed to an I2C master for the second slave address is reset if
*  i2c_initVar is 0, by i2c_initVar function call.
*
*  i2c_rwOffsetS2 - global variable, which stores offset for read
*  and write operations, is set at each write sequence of the second slave
*  device is reset if i2c_initVar is 0, by i2c_initVar
*  function call.
*
*  i2c_rwIndexS2 - global variable, which stores pointer to the
*  next value to be read or written for the second slave address is reset if
*  i2c_initVar is 0, by i2c_initVar function call.
*
* i2c_wrProtectS2 - global variable, which stores offset where data
*  is read only for the second slave address is reset if
*  i2c_initVar is 0, by i2c_initVar function call.
*
* i2c_bufSizeS2 - global variable, which stores size of data array
*  exposed to an I2C master for the second slave address is reset if
*  i2c_initVar is 0, by i2c_initVar function call.
*
* Side Effects:
*  This component automatically enables it's interrupt. If I2C is enabled
*  without the interrupt enabled, it could lock up the I2C bus.
*
* Reentrant:
*  No
*
*******************************************************************************/
void i2c_Start(void) 
{
    if(0u == i2c_initVar)
    {
        /* Initialize component's parameters */
        i2c_Init();

        /* Set init flag */
        i2c_initVar = 1u;
    }

    /* Enable slave mode for the device */
    i2c_Enable();
}


/*******************************************************************************
* Function Name: i2c_Stop
********************************************************************************
*
* Summary:
*  Disable the I2C block's slave operation and the corresponding interrupt.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void i2c_Stop(void) 
{
    uint8 interruptState;

    /* Disable Interrupt */
    i2c_DisableInt();

    #if(CY_PSOC3)

        /* Store resgisters which are held in reset when Slave is disabled */
        i2c_backup.adr = i2c_ADDR_REG;
        i2c_backup.clkDiv1  = i2c_CLKDIV1_REG;
        i2c_backup.clkDiv2  = i2c_CLKDIV2_REG;

        /* Reset fixed-function block */
        i2c_CFG_REG &= ~i2c_CFG_EN_SLAVE;
        i2c_CFG_REG |= i2c_CFG_EN_SLAVE;

        /* Restore registers */
        i2c_ADDR_REG = i2c_backup.adr;
        i2c_CLKDIV1_REG = i2c_backup.clkDiv1;
        i2c_CLKDIV2_REG = i2c_backup.clkDiv2;

    #endif  /* (CY_PSOC3) */

    /* Enter critical section */
    interruptState = CyEnterCriticalSection();

    /* Disable I2C block in Active mode template */
    i2c_PM_ACT_CFG_REG &= ~i2c_ACT_PWR_EN;

    /* Disable I2C block in Alternate Active (Standby) mode template */
    i2c_PM_STBY_CFG_REG &= ~i2c_STBY_PWR_EN;

    /* Exit critical section */
    CyExitCriticalSection(interruptState);
}


/*******************************************************************************
* Function Name: i2c_EnableInt
********************************************************************************
*
* Summary:
*  Enables the interrupt service routine for the component.  This is normally
*  handled with the start command.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void i2c_EnableInt(void) 
{
    /* Enable interrupt */
    CyIntEnable(i2c_ISR_NUMBER);
}


/*******************************************************************************
* Function Name: i2c_DisableInt
********************************************************************************
*
* Summary:
*  Disable I2C interrupts. Normally this function is not required since the
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
void i2c_DisableInt(void) 
{
    /* Disable interrupt */
    CyIntDisable(i2c_ISR_NUMBER);
}


/*******************************************************************************
* Function Name: i2c_SetAddress1
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
*  None
*
* Global variables:
*  i2c_addrS1 - the new slave address for the first slave device is
*  saved in it, if the component is configured to act as two slave devices.
*
* Reentrant:
*  No, if two addresses are used.
*
*******************************************************************************/
void i2c_SetAddress1(uint8 address) 
{
    #if(i2c_ADDRESSES == i2c_TWO_ADDRESSES)

        /* Address is stored in variable */
        i2c_addrS1  = address & i2c_SADDR_MASK;

    #else

        /* Address is stored in hardware */
        i2c_ADDR_REG = address & i2c_SADDR_MASK;

    #endif /* (i2c_ADDRESSES == i2c_TWO_ADDRESSES) */
}


/*******************************************************************************
* Function Name: i2c_GetAddress1
********************************************************************************
*
* Summary:
*  Returns address of the first slave device.
*
* Parameters:
*  None
*
* Return:
*  If the component is configured to has two slave addresses than primary
*  address is returned, otherwise address from the the address register is
*  returned.
*
* Global variables:
*  i2c_addrS1 - if component is configured to has two slave
*  addresses than primary address is saved here, otherwise address is written to
*  the register.
*
* Reentrant:
*  No
*
*******************************************************************************/
uint8 i2c_GetAddress1(void) 
{
    /* Get 1st slave address */
    #if(i2c_ADDRESSES == i2c_TWO_ADDRESSES)

        /* Return address from variable */
        return(i2c_addrS1);

    #else

        /* Return address from hardware */
        return(i2c_ADDR_REG);

    #endif /* (i2c_ADDRESSES == i2c_TWO_ADDRESSES) */
}


/*******************************************************************************
* Function Name: i2c_GetActivity
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
*  None
*
* Return:
*  A non-zero value is returned if activity is detected:
*   i2c_STATUS_READ1   Set if Read sequence is detected for first
*                                   address. Cleared when status read.
*
*   i2c_STATUS_WRITE1  Set if Write sequence is detected for first
*                                   address. Cleared when status read.
*
*   i2c_STATUS_READ2   Set if Read sequence is detected for second
*                                   address (if enabled). Cleared when status
*                                   read.
*
*   i2c_STATUS_WRITE2  Set if Write sequence is detected for second
*                                   address (if enabled). Cleared when status
*                                   read.
*
*   i2c_STATUS_BUSY    Set if Start detected, cleared when stop
*                                   detected.
*
*   i2c_STATUS_ERR     Set when I2C hardware detected, cleared
*                                   when status read.
*
* Global variables:
*  i2c_curStatus - global variable, which stores the current
*  component status.
*
* Reentrant:
*  No
*
*******************************************************************************/
uint8 i2c_GetActivity(void) 
{
    uint8 tmpStatus;

    tmpStatus = i2c_curStatus;

    /* Clear status, but no Busy one */
    i2c_curStatus &= i2c_STATUS_BUSY;

    return(tmpStatus);
}


/*******************************************************************************
* Function Name: i2c_SetBuffer1
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
*  None
*
* Global variables:
*  i2c_dataPtrS1 - stores pointer to the data exposed to an I2C
*  master for the first slave address is modified with the the new pointer to
*  data, passed by function parameter.
*
*  i2c_rwOffsetS1 - stores offset for read and write operations, is
*  modified at each write sequence of the first slave address is reset.
*
*  i2c_rwIndexS1 - stores pointer to the next value to be read or
*  written for the first slave address is set to 0.
*
* Reentrant:
*  No
*
* Side Effects:
*  It is recommended to disable component interrupt before calling this function
*  and enable it afterwards for the proper component operation.
*
*******************************************************************************/
void i2c_SetBuffer1(uint16 bufSize, uint16 rwBoundry, void * dataPtr) 
{
    /* Set pointer to data and clear index and offset. */
    i2c_dataPtrS1   = dataPtr;
    i2c_rwOffsetS1  = 0u;
    i2c_rwIndexS1   = 0u;

    #if(i2c_SUBADDR_WIDTH == i2c_SUBADDR_8BIT)
        i2c_bufSizeS1   = (uint8) bufSize;
        i2c_wrProtectS1 = (uint8) rwBoundry;
    #else
        i2c_bufSizeS1   = bufSize;
        i2c_wrProtectS1 = rwBoundry;
    #endif /* (i2c_SUBADDR_WIDTH == i2c_SUBADDR_8BIT) */
}


#if (i2c_ADDRESSES == i2c_ONE_ADDRESS)
    /*******************************************************************************
    * Function Name: i2c_SlaveSetSleepMode
    ********************************************************************************
    *
    * Summary:
    *  Disables the run time I2C regulator and enables the sleep Slave I2C.
    *  Should be called just prior to entering sleep. This function is only
    *  provided if a single I2C address is used.
    *
    * Parameters:
    *  None
    *
    * Return:
    *  None
    *
    * Side Effects:
    *  The I2C interrupt will be disabled if Wake up from Sleep mode option is
    *  enabled (only for PSoC3).
    *
    *******************************************************************************/
    void i2c_SlaveSetSleepMode(void) 
    {
        #if(CY_PSOC3 || CY_PSOC5A)

            /* if I2C block will be used as wake up source */
            #if(i2c_ENABLE_WAKEUP == 1u)

                uint8 interruptState;

                /* Enter critical section */
                interruptState = CyEnterCriticalSection();

                /* Enable the I2C regulator backup */
                i2c_PWRSYS_CR1_REG |= i2c_PWRSYS_CR1_I2C_BACKUP;

                /* Exit critical section */
                CyExitCriticalSection(interruptState);

                /* Set force nack before putting the device to power off mode.
                *  It is cleared on wake up.
                */
                i2c_XCFG_REG |= i2c_XCFG_FORCE_NACK;
                while(0u == (i2c_XCFG_REG & i2c_XCFG_SLEEP_READY))
                {
                    /* Waits for ongoing transaction to be completed. */
                }

                /* Disable interrupt for proper wake up procedure */
                i2c_DisableInt();

            #endif  /* (i2c_ENABLE_WAKEUP == 1u) */

        #endif  /* (CY_PSOC3 || CY_PSOC5A) */
    }


    /*******************************************************************************
    * Function Name: i2c_SlaveSetWakeMode
    ********************************************************************************
    *
    * Summary:
    *  Disables the sleep EzI2C slave and re-enables the run time I2C.  Should be
    *  called just after awaking from sleep.  Must preserve address to continue.
    *  This function is only provided if a single I2C address is used.
    *
    * Parameters:
    *  None
    *
    * Return:
    *  None
    *
    * Side Effects:
    *  The I2C interrupt will be enabled if Wake up from Sleep mode option is
    *  enabled (only for PSoC3).
    *
    *******************************************************************************/
    void i2c_SlaveSetWakeMode(void) 
    {
        #if(CY_PSOC3 || CY_PSOC5A)

            /* if I2C block will be used as wake up source */
            #if(i2c_ENABLE_WAKEUP == 1u)

                uint8 interruptState;

                /* Enter critical section */
                interruptState = CyEnterCriticalSection();

                /* Disable the I2C regulator backup */
                i2c_PWRSYS_CR1_REG &= ~i2c_PWRSYS_CR1_I2C_BACKUP;

                /* Exit critical section */
                CyExitCriticalSection(interruptState);

                /* Enable interrupt. The ISR is supposed to ready to be executed. */
                i2c_EnableInt();

            #endif  /* (i2c_ENABLE_WAKEUP == 1u) */

        #endif  /* (CY_PSOC3 || CY_PSOC5A) */ 
    }

#endif /* (i2c_ADDRESSES == i2c_ONE_ADDRESS) */


#if (i2c_ADDRESSES == i2c_TWO_ADDRESSES)
    /*******************************************************************************
    * Function Name: i2c_SetBuffer2
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
    *  None
    *
    * Global variables:
    *  i2c_dataPtrS2 - stores pointer to the data exposed to an I2C
    *  master for the second slave address is modified with the the new pointer to
    *  data, passed by function parameter.
    *
    *  i2c_rwOffsetS2 - stores offset for read and write operations,
    *  is modified at each write sequence of the second slave address is reset.
    *
    *  i2c_rwIndexS2 - stores pointer to the next value to be read or
    *  written for the second slave address is set to 0.
    *
    * Reentrant:
    *  No
    *
    * Side Effects:
    *  It is recommended to disable component interrupt before calling this
    *  function and enable it afterwards for the proper component operation.
    *
    *******************************************************************************/
    void i2c_SetBuffer2(uint16 bufSize, uint16 rwBoundry, void * dataPtr) 
    {
        /* Set pointer to data and clear index and offset. */
        i2c_dataPtrS2   = dataPtr;
        i2c_rwOffsetS2  = 0u;
        i2c_rwIndexS2   = 0u;

        #if(i2c_SUBADDR_WIDTH == i2c_SUBADDR_8BIT)
            i2c_bufSizeS2   = (uint8) bufSize;
            i2c_wrProtectS2 = (uint8) rwBoundry;
        #else
            i2c_bufSizeS2   = bufSize;
            i2c_wrProtectS2 = rwBoundry;
        #endif /* (i2c_SUBADDR_WIDTH == i2c_SUBADDR_8BIT) */
    }


    /*******************************************************************************
    * Function Name: i2c_SetAddress2
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
    *  None
    *
    * Global variables:
    *  i2c_addrS2 - the secondary slave address is modified.
    *
    * Reentrant:
    *  No
    *
    *******************************************************************************/
    void i2c_SetAddress2(uint8 address) 
    {
        /* Set slave address */
        i2c_addrS2  = address & i2c_SADDR_MASK;
    }


    /*******************************************************************************
    * Function Name: i2c_GetAddress2
    ********************************************************************************
    *
    * Summary:
    *  Returns the I2C slave address for the second device. This function is only
    *  provided if two I2C addresses have been selected in the user parameters.
    *
    * Parameters:
    *  i2c_addrS2 - global variable, which stores the second I2C
    *   address.
    *
    * Return:
    *  The secondary I2C slave address.
    *
    * Global variables:
    *  i2c_addrS2 - the secondary slave address is used.
    *
    * Reentrant:
    *  No
    *
    *******************************************************************************/
    uint8 i2c_GetAddress2(void) 
    {
        return(i2c_addrS2);
    }

#endif  /* (i2c_ADDRESSES == i2c_TWO_ADDRESSES) */


/*******************************************************************************
* Function Name: i2c_Init
********************************************************************************
*
* Summary:
*  Initializes/restores default EZI2C configuration provided with customizer.
*  Usually called in i2c_Start().
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global variables:
*  i2c_addrS1 - the new slave address for the first slave device is
*   saved.
*
*  i2c_addrS2 - the new slave address for the second slave device
*   is saved, if EzI2C component is configured for two slave addresses.
*
* Reentrant:
*  No
*
* Side Effects:
*  All changes applied by API to the component's configuration will be reset.
*
*******************************************************************************/
void i2c_Init(void) 
{
    #if(CY_PSOC5A)

        uint8 clkSel;

    #endif  /* (CY_PSOC5A) */

    /* Clear Status register */
    i2c_CSR_REG  = 0x00u;

    #if(CY_PSOC5A)

        /* Enable I2C block's slave operation */
        i2c_CFG_REG |= i2c_CFG_EN_SLAVE;

        /* The resolution of generated I2C bus SCL frequencies is reduced. */

        /* Temprorary set CLK divider to 16 */
        i2c_CLKDIV_REG  = i2c_CLK_DIV_16;

        /* Set clock divider for the I2C bus clock to be the closest HIGHER
        *  clock to ensure it can work with an ideal master clock.
        */
        for(clkSel = 0u; clkSel <= 6u; clkSel++ )
        {
            if((1u << clkSel) >= i2c_DEFAULT_CLKDIV)
            {
                i2c_CLKDIV_REG  = clkSel;
                break;
            }
        }

        /* Set clock rate to Fast mode 400k. This will give the Clock
        *  divider sufficient range to work bewteen 50 and 400 kbps.
        */
        i2c_CFG_REG  |= i2c_CFG_CLK_RATE_400;

    #elif(CY_PSOC3 || CY_PSOC5LP)

        /* Enable I2C block's slave operation.
        *  These revisions require slave to be enabled for registers to be
        *  written.
        */
        i2c_CFG_REG |= i2c_CFG_EN_SLAVE;

        /* 8 LSB bits of the 10-bit are written with the divide factor */
		i2c_CLKDIV1_REG = LO8(i2c_DIVIDE_FACTOR);

        /* 2 MSB bits of the 10-bit are written with the divide factor */
		i2c_CLKDIV2_REG = HI8(i2c_DIVIDE_FACTOR);

        /* Define clock rate */
        if(i2c_BUS_SPEED <= i2c_BUS_SPEED_50KHZ)
        {   /* 50 kHz - 32 samples/bit */
            i2c_CFG_REG |= i2c_CFG_CLK_RATE;
        }
        else
        {   /* 100kHz or 400kHz - 16 samples/bit */
            i2c_CFG_REG &= ~i2c_CFG_CLK_RATE;
        }

        /* if I2C block will be used as wake up source, this availabe only for
        * PSoC3 and PSoC5A.
        */
        #if(1u == i2c_ENABLE_WAKEUP)

            /* Configure I2C address match to act as wake-up source */
           i2c_XCFG_REG |= i2c_XCFG_I2C_ON;

            /* Process sio_select and pselect */
            #if(i2c_ADDRESSES == i2c_ONE_ADDRESS)
                if(i2c__ANY != i2c_BUS_PORT)
                {
                    /* SCL and SDA lines get their inputs from SIO block */
                    i2c_CFG_REG |= i2c_CFG_PSELECT;

                    if(i2c__I2C0 == i2c_BUS_PORT)
                    {
                        /* SCL and SDA lines get their inputs from SIO1 */
                        i2c_CFG_REG &= ~i2c_CFG_SIO_SELECT;
                    }
                    else /* SIO2 */
                    {
                        /* SCL and SDA lines get their inputs from SIO2 */
                        i2c_CFG_REG |= i2c_CFG_SIO_SELECT;
                    }
                }
                else    /* GPIO is used */
                {
                    /* SCL and SDA lines get their inputs from GPIO module. */
                    i2c_CFG_REG &= ~i2c_CFG_PSELECT;
                }
            #endif  /* (i2c_ADDRESSES == i2c_ONE_ADDRESS) */

        #endif /* (1u == i2c_ENABLE_WAKEUP) */

    #endif  /* (CY_PSOC5A) */


    #if(i2c_ADDRESSES == i2c_ONE_ADDRESS)

        /* Set default slave address */
        i2c_ADDR_REG  = i2c_DEFAULT_ADDR1;

        /* Turn on hardware address detection */
        i2c_XCFG_REG  |= i2c_XCFG_HDWR_ADDR_EN;

    #else   /* Two devices */

        /* Set default slave addresses */
        i2c_addrS1  = i2c_DEFAULT_ADDR1;
        i2c_addrS2  = i2c_DEFAULT_ADDR2;

    #endif  /* End of (i2c_ADDRESSES == i2c_ONE_ADDRESS) */

    /* Reset offsets and pointers */
    i2c_dataPtrS1 = (uint8 *)0x0000u;
    i2c_rwOffsetS1 = 0u;
    i2c_rwIndexS1 = 0u;
    i2c_wrProtectS1 = 0u;
    i2c_bufSizeS1 = 0u;

    /* Reset offsets and pointers for 2nd slave address */
    #if(i2c_ADDRESSES == i2c_TWO_ADDRESSES)
        i2c_dataPtrS2 = (uint8 *)0x0000u;
        i2c_rwOffsetS2 = 0u;
        i2c_rwIndexS2 = 0u;
        i2c_wrProtectS2 = 0u;
        i2c_bufSizeS2 = 0u;
    #endif  /* End of (i2c_ADDRESSES == i2c_TWO_ADDRESSES) */

    /* Enable the I2C block clock */
    i2c_XCFG_REG  |= i2c_XCFG_CLK_EN;
}


/*******************************************************************************
* Function Name: i2c_Enable
********************************************************************************
*
* Summary:
*  Enables the I2C block operation, sets interrupt priority, sets
*  interrupt vector, clears pending interrupts and enables interrupts. Clears
*  status variables and reset state machine variable.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global variables:
*  i2c_curStatus - this global variable are cleared, it stores the
*  current component status.
*
* i2c_curState - global variable are cleared, it stores the current
*  state of the state machine.
*
* Reentrant:
*  No
*
*******************************************************************************/
void i2c_Enable(void) 
{
    uint8 interruptState;

    /* Enter critical section */
    interruptState = CyEnterCriticalSection();

    /* Enable I2C block in Active mode template */
    i2c_PM_ACT_CFG_REG |= i2c_ACT_PWR_EN;

    /* Enable I2C block in Alternate Active (Standby) mode template */
    i2c_PM_STBY_CFG_REG |= i2c_STBY_PWR_EN;

    /* Exit critical section */
    CyExitCriticalSection(interruptState);

    /* Set the interrupt priority */
    CyIntSetPriority(i2c_ISR_NUMBER, i2c_ISR_PRIORITY);

    /* Set the interrupt vector */
    CyIntSetVector(i2c_ISR_NUMBER, i2c_ISR);

    /* Clear any pending interrupt */
    CyIntClearPending(i2c_ISR_NUMBER);

    /* Reset State Machine to IDLE */
    i2c_curState = i2c_SM_IDLE;

    /* Clear Status variable */
    i2c_curStatus = 0x00u;

    /* Enable the interrupt */
    i2c_EnableInt();
}


/* [] END OF FILE */
