/*******************************************************************************
* File Name: .h
* Version 1.10
*
* Description:
*  This private file provides constants and parameter values for the
*  SCB Component in I2C mode.
*  Please do not use this file or its content in your project.
*
* Note:
*
********************************************************************************
* Copyright 2013, Cypress Semiconductor Corporation. All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(CY_SCB_I2C_PVT_i2c_H)
#define CY_SCB_I2C_PVT_i2c_H

#include "i2c_I2C.h"


/***************************************
*     Private Global Vars
***************************************/

extern volatile uint8 i2c_state; /* Current state of I2C FSM */

#if(i2c_I2C_SLAVE_CONST)
    extern volatile uint8 i2c_slStatus;          /* Slave Status */

    /* Receive buffer variables */
    extern volatile uint8 * i2c_slWrBufPtr;      /* Pointer to Receive buffer  */
    extern volatile uint32  i2c_slWrBufSize;     /* Slave Receive buffer size  */
    extern volatile uint32  i2c_slWrBufIndex;    /* Slave Receive buffer Index */

    /* Transmit buffer variables */
    extern volatile uint8 * i2c_slRdBufPtr;      /* Pointer to Transmit buffer  */
    extern volatile uint32  i2c_slRdBufSize;     /* Slave Transmit buffer size  */
    extern volatile uint32  i2c_slRdBufIndex;    /* Slave Transmit buffer Index */
    extern volatile uint32  i2c_slRdBufIndexTmp; /* Slave Transmit buffer Index Tmp */
    extern volatile uint8   i2c_slOverFlowCount; /* Slave Transmit Overflow counter */
#endif /* (i2c_I2C_SLAVE_CONST) */

#if(i2c_I2C_MASTER_CONST)
    extern volatile uint16 i2c_mstrStatus;      /* Master Status byte  */
    extern volatile uint8  i2c_mstrControl;     /* Master Control byte */

    /* Receive buffer variables */
    extern volatile uint8 * i2c_mstrRdBufPtr;   /* Pointer to Master Read buffer */
    extern volatile uint32  i2c_mstrRdBufSize;  /* Master Read buffer size       */
    extern volatile uint32  i2c_mstrRdBufIndex; /* Master Read buffer Index      */

    /* Transmit buffer variables */
    extern volatile uint8 * i2c_mstrWrBufPtr;   /* Pointer to Master Write buffer */
    extern volatile uint32  i2c_mstrWrBufSize;  /* Master Write buffer size       */
    extern volatile uint32  i2c_mstrWrBufIndex; /* Master Write buffer Index      */
    extern volatile uint32  i2c_mstrWrBufIndexTmp; /* Master Write buffer Index Tmp */
#endif /* (i2c_I2C_MASTER_CONST) */


/***************************************
*     Private Function Prototypes
***************************************/

#if(i2c_SCB_MODE_I2C_CONST_CFG)
    void i2c_I2CInit(void);
#endif /* (i2c_SCB_MODE_I2C_CONST_CFG) */

void i2c_I2CStop(void);
void i2c_I2CSaveConfig(void);
void i2c_I2CRestoreConfig(void);

#if(i2c_I2C_MASTER_CONST)
    void i2c_I2CReStartGeneration(void);
#endif /* (i2c_I2C_MASTER_CONST) */

#endif /* (CY_SCB_I2C_PVT_i2c_H) */


/* [] END OF FILE */
