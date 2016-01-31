/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*    This file contains the function prototypes and constants used in
*    the Analog Buffer User Module.
*
*   Note:
*     
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/




#include "cytypes.h"
#include "cyfitter.h"

#if !defined(CY_ABUF_`$INSTANCE_NAME`_H) 
#define CY_ABUF_`$INSTANCE_NAME`_H 

/**************************************
*        Function Prototypes 
**************************************/

void    `$INSTANCE_NAME`_Start(void);
void    `$INSTANCE_NAME`_Stop(void);
void    `$INSTANCE_NAME`_SetPower(uint8 power);


/**************************************
*           API Constants        
**************************************/

/* Power constants for SetPower() function */
#define `$INSTANCE_NAME`_LOWPOWER    0x01u
#define `$INSTANCE_NAME`_MEDPOWER    0x02u
#define `$INSTANCE_NAME`_HIGHPOWER   0x03u


/**************************************
*           Parameter Defaults        
**************************************/

#define `$INSTANCE_NAME`_DEFAULT_POWER    `$Power` 
#define `$INSTANCE_NAME`_DEFAULT_MODE     `$Mode` 

/**************************************
*       Register Constants        
**************************************/

/* CX Analog Buffer Input Selection Register */

/* Power mode defines.                  */
#define `$INSTANCE_NAME`_PWR_MASK    0x03u 
#define `$INSTANCE_NAME`_PWR_SLOW    0x01u
#define `$INSTANCE_NAME`_PWR_MEDIUM  0x02u
#define `$INSTANCE_NAME`_PWR_FAST    0x03u

/* MX Analog Buffer Input Selection Register */

/* Bit Field  MX_VN                     */
#define `$INSTANCE_NAME`_MX_VN_MASK  0x30u 
#define `$INSTANCE_NAME`_MX_VN_NC    0x00u 
#define `$INSTANCE_NAME`_MX_VN_AG4   0x20u 
#define `$INSTANCE_NAME`_MX_VN_AG6   0x30u 

/* Bit Field  MX_VP                     */
#define `$INSTANCE_NAME`_MX_VP_MASK  0x0Fu 
#define `$INSTANCE_NAME`_MX_VP_NC    0x00u 
#define `$INSTANCE_NAME`_MX_VP_VREF  0x07u 
#define `$INSTANCE_NAME`_MX_VP_AG4   0x08u 
#define `$INSTANCE_NAME`_MX_VP_AG5   0x09u 
#define `$INSTANCE_NAME`_MX_VP_AG6   0x0Au 
#define `$INSTANCE_NAME`_MX_VP_AG7   0x08u 
#define `$INSTANCE_NAME`_MX_VP_ABUS0 0x0Cu 
#define `$INSTANCE_NAME`_MX_VP_ABUS1 0x0Du 
#define `$INSTANCE_NAME`_MX_VP_ABUS2 0x0Eu 
#define `$INSTANCE_NAME`_MX_VP_ABUS3 0x0Fu 

/* SW Analog Buffer Routing Switch Reg  */

/* Bit Field  SW                        */
#define `$INSTANCE_NAME`_SW_MASK     0x07u 
#define `$INSTANCE_NAME`_SW_SWINP    0x04u  /* Enable positive input */
#define `$INSTANCE_NAME`_SW_SWINN    0x02u  /* Enable negative input */
#define `$INSTANCE_NAME`_SW_SWFOL    0x01u  /* Enable follower mode  */

/* PM_ACT_CFG (Active Power Mode CFG Register)     */ 
#define `$INSTANCE_NAME`_ACT_PWR_EN   `$INSTANCE_NAME`_ABuf__PM_ACT_MSK /* Power enable mask */

/***********************************************/
/* ANIF.PUMP.CR1 Pump Configuration Register 1 */
/***********************************************/
#define `$INSTANCE_NAME`_PUMP_CR1  (* (reg8 *) CYDEV_ANAIF_CFG_PUMP_CR1)

#define `$INSTANCE_NAME`_PUMP_CR1_CLKSEL  0x40
#define `$INSTANCE_NAME`_PUMP_CR1_FORCE   0x20
#define `$INSTANCE_NAME`_PUMP_CR1_AUTO    0x10

/**************************************
*             Registers        
**************************************/

#define `$INSTANCE_NAME`_CR    	(* (reg8 *) `$INSTANCE_NAME`_ABuf__CR )
#define `$INSTANCE_NAME`_MX    	(* (reg8 *) `$INSTANCE_NAME`_ABuf__MX )
#define `$INSTANCE_NAME`_SW    	(* (reg8 *) `$INSTANCE_NAME`_ABuf__SW )
#define `$INSTANCE_NAME`_PWRMGR  (* (reg8 *) `$INSTANCE_NAME`_ABuf__PM_ACT_CFG )  /* Power manager */

#endif /* CY_ABUF_`$INSTANCE_NAME`_H */

/* [] END OF FILE */
