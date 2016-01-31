/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*    This file contains the setup, control and status commands for the EzI2C
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
#include "`$INSTANCE_NAME`.h"  
#include "`$INSTANCE_NAME`_I2C_IRQ.h"  


extern uint8 `$INSTANCE_NAME`_State;       /* Current state of I2C state machine */
extern uint8 `$INSTANCE_NAME`_Status;      /* Status byte */                     

extern uint8 * `$INSTANCE_NAME`_DataPtr;   /* Pointer to data exposed to I2C Master */       

#if (`$INSTANCE_NAME`_SUBADDR_WIDTH == `$INSTANCE_NAME`_SUBADDR_8BIT)
extern uint8   `$INSTANCE_NAME`_RwOffset1;  /* Offset for read and write operations, set at each write sequence */
extern uint8   `$INSTANCE_NAME`_RwIndex1;   /* Points to next value to be read or written */
extern uint8   `$INSTANCE_NAME`_WrProtect1; /* Offset where data is read only */
extern uint8   `$INSTANCE_NAME`_BufSize1;   /* Size of array between 1 and 255 */
#else
extern uint16  `$INSTANCE_NAME`_RwOffset1;  /* Offset for read and write operations, set at each write sequence */
extern uint16  `$INSTANCE_NAME`_RwIndex1;   /* Points to next value to be read or written */
extern uint16  `$INSTANCE_NAME`_WrProtect1; /* Offset where data is read only */
extern uint16  `$INSTANCE_NAME`_BufSize1;   /* Size of array between 1 and 65535 */
#endif

#if (`$INSTANCE_NAME`_ADDRESSES == `$INSTANCE_NAME`_TWO_ADDRESSES)
extern uint8 * `$INSTANCE_NAME`_DataPtr2;   /* Pointer to data exposed to I2C Master */       

extern uint8   `$INSTANCE_NAME`_Address1;   /* Software address compare 1 */
extern uint8   `$INSTANCE_NAME`_Address2;   /* Software address compare 2 */

#if (`$INSTANCE_NAME`_SUBADDR_WIDTH == `$INSTANCE_NAME`_SUBADDR_8BIT)
extern uint8   `$INSTANCE_NAME`_RwOffset2;  /* Offset for read and write operations, set at each write sequence */
extern uint8   `$INSTANCE_NAME`_RwIndex2;   /* Points to next value to be read or written */
extern uint8   `$INSTANCE_NAME`_WrProtect2; /* Offset where data is read only */
extern uint8   `$INSTANCE_NAME`_BufSize2;   /* Size of array between 1 and 255 */
#else
extern uint16  `$INSTANCE_NAME`_RwOffset2;  /* Offset for read and write operations, set at each write sequence */
extern uint16  `$INSTANCE_NAME`_RwIndex2;   /* Points to next value to be read or written */
extern uint16  `$INSTANCE_NAME`_WrProtect2; /* Offset where data is read only */
extern uint16  `$INSTANCE_NAME`_BufSize2;   /* Size of array between 1 and 65535 */
#endif
#endif

uint8 `$INSTANCE_NAME`_initVar = 0;


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

    if(`$INSTANCE_NAME`_initVar == 0) 
    {
        `$INSTANCE_NAME`_initVar = 1;
        /* Component function code  */
        /* Configure registers */
        /* Set the clock divider to the defined value */
        `$INSTANCE_NAME`_CLKDIV  = `$INSTANCE_NAME`_DEFAULT_CLKDIV;

        /* Clear Status register */
        `$INSTANCE_NAME`_CSR  = 0x00;

        /* Set the priority. */
        `$INSTANCE_NAME`_I2C_IRQ_SetPriority(`$INSTANCE_NAME`_I2C_IRQ_INTC_PRIOR_NUMBER);

        /* Set interrupt vector */
        `$INSTANCE_NAME`_I2C_IRQ_SetVector( `$INSTANCE_NAME`_ISR );
        
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

        /* Set Configuration, slave, rate, IE on stop and bus error, Pins **CHECK** */
        /* Set the prescaler to the 400k bps mode, which is really a divide by 4.  This will give the */
        /* Clock divider sufficient range to work bewteen 50 and 400 kbps.   */
        `$INSTANCE_NAME`_CFG  = `$INSTANCE_NAME`_CFG_CLK_RATE_400 | `$INSTANCE_NAME`_CFG_EN_SLAVE \
                           |  `$INSTANCE_NAME`_CFG_BUS_ERR_IE;

        #if (`$INSTANCE_NAME`_ADDRESSES == `$INSTANCE_NAME`_ONE_ADDRESS)

            /* Set default slave address */
            `$INSTANCE_NAME`_ADDR  = `$INSTANCE_NAME`_DEFAULT_ADDR1;

            /* Turn on hardware address detection and enable the clock */
            `$INSTANCE_NAME`_XCFG  = `$INSTANCE_NAME`_XCFG_HDWR_ADDR_EN | `$INSTANCE_NAME`_XCFG_CLK_EN;

        #else   /* Two devices */

            /* Set default slave addresses */
            `$INSTANCE_NAME`_Address1  = `$INSTANCE_NAME`_DEFAULT_ADDR1;
            `$INSTANCE_NAME`_Address2  = `$INSTANCE_NAME`_DEFAULT_ADDR2;

            /* Do not enable hardware address detection, enable the clock */
            `$INSTANCE_NAME`_XCFG  = `$INSTANCE_NAME`_XCFG_CLK_EN;
        #endif


        /* Reset offsets and pointers */
        `$INSTANCE_NAME`_DataPtr = (uint8 *)0x0000;
        `$INSTANCE_NAME`_RwOffset1 = 0;
        `$INSTANCE_NAME`_RwIndex1 = 0;
        `$INSTANCE_NAME`_WrProtect1 = 0;
        `$INSTANCE_NAME`_BufSize1 = 0;

        #if (`$INSTANCE_NAME`_ADDRESSES == `$INSTANCE_NAME`_TWO_ADDRESSES)
            `$INSTANCE_NAME`_DataPtr2 = (uint8 *)0x0000;
            `$INSTANCE_NAME`_RwOffset2 = 0;
            `$INSTANCE_NAME`_RwIndex2 = 0;
            `$INSTANCE_NAME`_WrProtect2 = 0;
            `$INSTANCE_NAME`_BufSize2 = 0;
        #endif


    }

    /* Reset State Machine */
    `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_IDLE;
    `$INSTANCE_NAME`_Status = 0x00;

    /* Enable Slave */
    `$INSTANCE_NAME`_CFG  |= `$INSTANCE_NAME`_CFG_EN_SLAVE;

    /* Clear any pending interrupt */
    `$INSTANCE_NAME`_I2C_IRQ_ClearPending();


    /* Enable Interrupt */
    `$INSTANCE_NAME`_I2C_IRQ_Enable( );

    /* Enable power to I2C Module */
    `$INSTANCE_NAME`_PWRMGR |= `$INSTANCE_NAME`_ACT_PWR_EN;
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
    /* Disable Slave */
    `$INSTANCE_NAME`_CFG  &= ~`$INSTANCE_NAME`_CFG_EN_SLAVE;

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


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetAddress1
********************************************************************************
* Summary:
*  This function sets the main address of this I2C slave device.
*
* Parameters:  
*  address:  The 7-bit slave address between 0x00 and 0x7F
*
* Return: 
*  void 
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetAddress1( uint8 address) 
{
    /* Set slave address */
    #if (`$INSTANCE_NAME`_ADDRESSES == `$INSTANCE_NAME`_TWO_ADDRESSES)
        `$INSTANCE_NAME`_Address1  = address & 0x7F;
    #else
        `$INSTANCE_NAME`_ADDR  = address & 0x7F;
    #endif

}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetAddress1
********************************************************************************
* Summary:
*  Return address of main device.
*
* Parameters:  
*  void    
*
* Return: 
*  The current slave address.
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_GetAddress1(void) 
{
    #if (`$INSTANCE_NAME`_ADDRESSES == `$INSTANCE_NAME`_TWO_ADDRESSES)
        return(`$INSTANCE_NAME`_Address1);
    #else
        return(`$INSTANCE_NAME`_ADDR);
    #endif
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetActivity
********************************************************************************
* Summary:
*  This function returns a nonzero value if the I2C read or write cycle 
*  occurred since the last time this function was called.  The activity
*  flag resets to zero at the end of this function call.
*  The Read and Write busy flags are cleared when read, but the "BUSY" 
*  flag is only cleared by an I2C Stop.
*
* Parameters:  
*  void 
*
* Return: 
*  A non-zero value is returned if activity is detected.
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_GetActivity(void) 
{
    uint8 tmpStatus;

    tmpStatus = `$INSTANCE_NAME`_Status;

    /* Clear Read, Write, and Error status */
    `$INSTANCE_NAME`_Status &= `$INSTANCE_NAME`_STATUS_BUSY;
    return(tmpStatus);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetBuffer1
********************************************************************************
* Summary:
*  This function sets the buffer, size of the buffer, and the R/W boundry
*  for the memory buffer.
*
* Parameters:  
*  size:  Size of the buffer in bytes.  
*
*  rwBoundry: Sets how many bytes are writable in the beginning of the buffer
*             This value must be less than or equal to the buffer size.
*
*  dataPtr:  Pointer to the data buffer.  
*
* Return: 
*  void
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetBuffer1(uint16 bufSize, uint16 rwBoundry, void * dataPtr) 
{
    /* Component function code  */
    `$INSTANCE_NAME`_DataPtr    = dataPtr;
    `$INSTANCE_NAME`_RwOffset1  = 0u;
    `$INSTANCE_NAME`_RwIndex1   = 0u;

    #if (`$INSTANCE_NAME`_SUBADDR_WIDTH == `$INSTANCE_NAME`_SUBADDR_8BIT)
        `$INSTANCE_NAME`_BufSize1   =(uint8) bufSize;
        `$INSTANCE_NAME`_WrProtect1 =(uint8) rwBoundry;
    #else
        `$INSTANCE_NAME`_BufSize1   = bufSize;
        `$INSTANCE_NAME`_WrProtect1 = rwBoundry;
    #endif
}


#if (`$INSTANCE_NAME`_ADDRESSES == `$INSTANCE_NAME`_ONE_ADDRESS)
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SlaveSetSleepMode
********************************************************************************
* Summary:
*  Disables the run time EZI2C and enables the sleep Slave I2C.  Should be
*  called just prior to entering sleep.  Only generated if fixed I2C hardware
*  is used.
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
void `$INSTANCE_NAME`_SlaveSetSleepMode(void) 
{
   /* Component function code  */
   // Look at the following registers
   // PWRSYS.CR1  ( I2C power regulator enable )
   //
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SlaveSetWakeMode
********************************************************************************
* Summary:
*  Disables the sleep EzI2C slave and re-enables the run time I2C.  Should be
*  called just after awaking from sleep.  Must preserve address to continue.  
*  Only generated if fixed I2C hardware is used.
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
void `$INSTANCE_NAME`_SlaveSetWakeMode(void) 
{
    /* Component function code  */
}
#endif


#if (`$INSTANCE_NAME`_ADDRESSES == `$INSTANCE_NAME`_TWO_ADDRESSES)
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetBuffer
********************************************************************************
* Summary:
*  This function sets the buffer, size of the buffer, and the R/W boundry
*  for the memory buffer.
*
* Parameters:  
*  size:  Size of the buffer in bytes.  
*
*  rwBoundry: Sets how many bytes are writable in the beginning of the buffer
*             This value must be less than or equal to the buffer size.
*
*  dataPtr:  Pointer to the data buffer.  
*
* Return: 
*  void
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetBuffer2(uint16 bufSize, uint16 rwBoundry, void * dataPtr) 
{
    /* Component function code  */
    `$INSTANCE_NAME`_DataPtr2   = dataPtr;
    `$INSTANCE_NAME`_RwOffset2  = 0u;
    `$INSTANCE_NAME`_RwIndex2   = 0u;

    #if (`$INSTANCE_NAME`_SUBADDR_WIDTH == `$INSTANCE_NAME`_SUBADDR_8BIT)
        `$INSTANCE_NAME`_BufSize2   =(uint8) bufSize;
        `$INSTANCE_NAME`_WrProtect2 =(uint8) rwBoundry;
    #else
        `$INSTANCE_NAME`_BufSize2   = bufSize;
        `$INSTANCE_NAME`_WrProtect2 = rwBoundry;
    #endif
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetAddress2
********************************************************************************
* Summary:
*  This function sets the main address of this I2C slave device.
*
* Parameters:  
*  address:  The 7-bit slave address between 0x00 and 0x7F
*
* Return: 
*  void 
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetAddress2( uint8 address) 
{
    /* Set slave address */
    `$INSTANCE_NAME`_Address2  = address & 0x7F;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetAddress2
********************************************************************************
* Summary:
*  Return address of main device.
*
* Parameters:  
*  void    
*
* Return: 
*  (uint8) The current slave address.
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_GetAddress2(void) 
{
    return(`$INSTANCE_NAME`_Address2);
}
#endif

/* [] END OF FILE */


