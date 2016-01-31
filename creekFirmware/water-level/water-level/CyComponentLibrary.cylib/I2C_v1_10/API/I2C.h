/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*   This is the header file for the I2C user module.  It contains function
*   prototypes and constants for the users convenience. 
*
*   Note:
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#include "cytypes.h"
#include "cyfitter.h"
#include "CyLib.h"

#if !defined(`$INSTANCE_NAME`_H)
#define `$INSTANCE_NAME`_H

`#cy_declare_enum I2C__Implementation`

/***************************************
* M/S Combined Function Prototypes.
***************************************/

void `$INSTANCE_NAME`_Start(void);
void `$INSTANCE_NAME`_Stop(void);
void `$INSTANCE_NAME`_EnableInt(void);
void `$INSTANCE_NAME`_DisableInt(void);
CY_ISR_PROTO( `$INSTANCE_NAME`_ISR );

#if(CYDEV_CHIP_DIE_EXPECT == CYDEV_CHIP_DIE_LEOPARD)     
    #if((CYDEV_CHIP_REV_EXPECT <= CYDEV_CHIP_REV_LEOPARD_ES2) && (`$INSTANCE_NAME`_I2C_IRQ__ES2_PATCH))      
        #include <intrins.h>
        #define `$INSTANCE_NAME`_ISR_PATCH() _nop_(); _nop_(); _nop_(); _nop_(); _nop_(); _nop_(); _nop_(); _nop_();
    #endif
#endif    

/* Number of the RTC_isr interrupt. */
#define `$INSTANCE_NAME`_ISR_NUMBER           `$INSTANCE_NAME``[I2C_IRQ]`_INTC_NUMBER

/* Priority of the RTC_isr interrupt. */
#define `$INSTANCE_NAME`_ISR_PRIORITY         `$INSTANCE_NAME``[I2C_IRQ]`_INTC_PRIOR_NUM


/***************************************
*   Initial Parameter Constants 
***************************************/

#define `$INSTANCE_NAME`_FIXED_FUNCTION             `$Implementation`
#define `$INSTANCE_NAME`_DEFAULT_ADDR               `$Slave_Address`   
#define `$INSTANCE_NAME`_ADDR_DECODE                `$Address_Decode`   
#define `$INSTANCE_NAME`_SW_DECODE                  0x00u   
#define `$INSTANCE_NAME`_HDWR_DECODE                0x01u    

#define `$INSTANCE_NAME`_ENABLE_WAKEUP              `$EnableWakeup`   
#define `$INSTANCE_NAME`_BUS_SPEED                  `$BusSpeed_kHz`   
#define `$INSTANCE_NAME`_MSTR_TIMEOUT               50  /* Time out in mSec */

#define `$INSTANCE_NAME`_DEFAULT_CLKDIV             (BCLK__BUS_CLK__KHZ/(`$INSTANCE_NAME`_BUS_SPEED*16))

#define `$INSTANCE_NAME`_MODE                       `$I2C_Mode`  /* I2C Mode */
#define `$INSTANCE_NAME`_MODE_SLAVE                 0x01u  /* I2C Slave Mode */
#define `$INSTANCE_NAME`_MODE_MASTER                0x02u  /* I2C Master Mode */
#define `$INSTANCE_NAME`_MODE_MULTI_MASTER          0x06u  /* I2C Multi-Master Mode */
#define `$INSTANCE_NAME`_MODE_MULTI_MASTER_SLAVE    0x07u  /* I2C Multi-Master Slave Mode */


/***************************************
* I2C state machine constants 
***************************************/

/* Default slave address states */
#define  `$INSTANCE_NAME`_DEV_MASK                  0xF0   /* Wait for sub-address */
#define  `$INSTANCE_NAME`_SM_IDLE                   0x10   /* Idle I2C state */
#define  `$INSTANCE_NAME`_DEV_MASTER_XFER           0x40   /* Wait for sub-address */

/* Default slave address states */
#define  `$INSTANCE_NAME`_SM_SL_WR_IDLE             0x10   /* Slave Idle, waiting for start */
#define  `$INSTANCE_NAME`_SM_SL_WR_DATA             0x11   /* Slave waiting for master to write data */
#define  `$INSTANCE_NAME`_SM_SL_RD_DATA             0x12   /* Slave waiting for master to read data */
#define  `$INSTANCE_NAME`_SM_SL_STOP                0x14   /* Slave waiting for stop */

/* Master mode states */
#define  `$INSTANCE_NAME`_SM_MASTER                 0x40   /* Master or Multi-Master mode is set */
#define  `$INSTANCE_NAME`_SM_MASTER_IDLE            0x40   /* Hardware in Master mode and sitting idle  */

#define  `$INSTANCE_NAME`_SM_MSTR_ADDR              0x43   /* Master has sent Start/Address */
#define  `$INSTANCE_NAME`_SM_MSTR_WR_ADDR           0x42   /* Master has sent a Start/Address/WR */
#define  `$INSTANCE_NAME`_SM_MSTR_RD_ADDR           0x43   /* Master has sent a Start/Address/RD */

#define  `$INSTANCE_NAME`_SM_MSTR_DATA              0x44   /* Master is writing data to external slave */
#define  `$INSTANCE_NAME`_SM_MSTR_WR_DATA           0x44   /* Master is writing data to external slave */
#define  `$INSTANCE_NAME`_SM_MSTR_RD_DATA           0x45   /* Master is receiving data from external slave */

#define  `$INSTANCE_NAME`_SM_MSTR_WAIT_STOP         0x48   /* Master Send Stop */
#define  `$INSTANCE_NAME`_SM_MSTR_HALT              0x60   /* Master Halt state */

/* mstrControl bit definitions */
#define  `$INSTANCE_NAME`_MSTR_GEN_STOP             0x01   /* Generate a stop after a data transfer */ 
#define  `$INSTANCE_NAME`_MSTR_NO_STOP              0x01   /* Do not generate a stop after a data transfer */ 


