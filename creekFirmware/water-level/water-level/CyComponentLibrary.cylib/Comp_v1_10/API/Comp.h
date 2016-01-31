/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*    This file contains the function prototypes and constants used in
*    the Analog Comparitor User Module.
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

#if !defined(CY_COMP_`$INSTANCE_NAME`_H) 
#define CY_COMP_`$INSTANCE_NAME`_H 

#define `$INSTANCE_NAME`_RECALMODE 0

/**************************************
*        Function Prototypes 
**************************************/

void    `$INSTANCE_NAME`_Start(void);
void    `$INSTANCE_NAME`_Stop(void);
void    `$INSTANCE_NAME`_SetSpeed(uint8 speed);
uint8   `$INSTANCE_NAME`_GetCompare(void);
uint8   `$INSTANCE_NAME`_ZeroCal(void);
void    `$INSTANCE_NAME`_LoadTrim(uint8 trimVal); 


/**************************************
*           API Constants        
**************************************/
/* Power constants for SetSpeed() function */
#define `$INSTANCE_NAME`_SLOWSPEED   0x00u
#define `$INSTANCE_NAME`_HIGHSPEED   0x01u
#define `$INSTANCE_NAME`_LOWPOWER    0x02u


/**************************************
*           Parameter Defaults        
**************************************/

#define `$INSTANCE_NAME`_DEFAULT_SPEED       `$Speed` 
#define `$INSTANCE_NAME`_DEFAULT_HYSTERESIS  `$Hysteresis` 
#define `$INSTANCE_NAME`_DEFAULT_POLARITY    `$Polarity` 
#define `$INSTANCE_NAME`_DEFAULT_BYPASS_SYNC `$Sync` 

/**************************************
*             Registers        
**************************************/

#define `$INSTANCE_NAME`_CR     (* (reg8 *) `$INSTANCE_NAME`_ctComp__CR )   /* Config register   */
#define `$INSTANCE_NAME`_CLK    (* (reg8 *) `$INSTANCE_NAME`_ctComp__CLK )  /* Comp clock comtrol register */
#define `$INSTANCE_NAME`_SW0    (* (reg8 *) `$INSTANCE_NAME`_ctComp__SW0 )  /* Routing registers */
#define `$INSTANCE_NAME`_SW2    (* (reg8 *) `$INSTANCE_NAME`_ctComp__SW2 )
#define `$INSTANCE_NAME`_SW3    (* (reg8 *) `$INSTANCE_NAME`_ctComp__SW3 )
#define `$INSTANCE_NAME`_SW4    (* (reg8 *) `$INSTANCE_NAME`_ctComp__SW4 )
#define `$INSTANCE_NAME`_SW6    (* (reg8 *) `$INSTANCE_NAME`_ctComp__SW6 )
#define `$INSTANCE_NAME`_TR     (* (reg8 *) `$INSTANCE_NAME`_ctComp__TR )   /* Trim registers */
#define `$INSTANCE_NAME`_WRK    (* (reg8 *) `$INSTANCE_NAME`_ctComp__WRK )  /* Working register - output */
#define `$INSTANCE_NAME`_PWRMGR (* (reg8 *) `$INSTANCE_NAME`_ctComp__PM_ACT_CFG )  /* Power manager */


/**************************************
*       Register Constants        
**************************************/

/* CR (Comp Control Register)             */
#define `$INSTANCE_NAME`_CFG_MODE_MASK  0x78u
#define `$INSTANCE_NAME`_FILTER_ON      0x40u
#define `$INSTANCE_NAME`_HYST_OFF       0x20u
#define `$INSTANCE_NAME`_CAL_ON         0x10u
#define `$INSTANCE_NAME`_MX_AO          0x08u
#define `$INSTANCE_NAME`_PWRDWN_OVRD    0x04u

#define `$INSTANCE_NAME`_PWR_MODE_SHIFT 0x00u
#define `$INSTANCE_NAME`_PWR_MODE_MASK  (0x03u << `$INSTANCE_NAME`_PWR_MODE_SHIFT)
#define `$INSTANCE_NAME`_PWR_MODE_SLOW  (0x00u << `$INSTANCE_NAME`_PWR_MODE_SHIFT)
#define `$INSTANCE_NAME`_PWR_MODE_FAST  (0x01u << `$INSTANCE_NAME`_PWR_MODE_SHIFT)
#define `$INSTANCE_NAME`_PWR_MODE_ULOW  (0x02u << `$INSTANCE_NAME`_PWR_MODE_SHIFT)

/* CLK (Comp Clock Control Register)      */
#define `$INSTANCE_NAME`_BYPASS_SYNC    0x10u
#define `$INSTANCE_NAME`_SYNC_CLK_EN    0x08u
#define `$INSTANCE_NAME`_SYNCCLK_MASK   (`$INSTANCE_NAME`_BYPASS_SYNC | `$INSTANCE_NAME`_SYNC_CLK_EN)

/* SW3Routing Register definitions */
#define `$INSTANCE_NAME`_CMP_SW3_INPCTL_MASK    0x09u   /* SW3 bits for inP routing control */

/* TR (Comp Trim Register)     */
#define `$INSTANCE_NAME`_CMP_TRIM1_DIR  0x08u   /* Trim direction for N-type load for offset calibration */
#define `$INSTANCE_NAME`_CMP_TRIM1_MASK 0x07u   /* Trim for N-type load for offset calibration */
#define `$INSTANCE_NAME`_CMP_TRIM2_DIR  0x80u   /* Trim direction for P-type load for offset calibration */
#define `$INSTANCE_NAME`_CMP_TRIM2_MASK 0x70u   /* Trim for P-type load for offset calibration */

/* WRK (Comp output working register)     */ 
#define `$INSTANCE_NAME`_CMP_OUT_MASK   `$INSTANCE_NAME`_ctComp__WRK_MASK /* Specific comparator out mask */

/* PM_ACT_CFG (Active Power Mode CFG Register)     */ 
#define `$INSTANCE_NAME`_ACT_PWR_EN     `$INSTANCE_NAME`_ctComp__PM_ACT_MSK /* Power enable mask */

#endif /* CY_COMP_`$INSTANCE_NAME`_H */
/* [] END OF FILE  */
