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
uint8   `$INSTANCE_NAME`_ZeroCal(void);
uint8   `$INSTANCE_NAME`_GetCompare(void);

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
*       Register Constants        
**************************************/


/* CR (Comp Control Register)             */
#define `$INSTANCE_NAME`_CFG_MODE_MASK 0x78u
#define `$INSTANCE_NAME`_FILTER_ON     0x40u
#define `$INSTANCE_NAME`_HYST_ON       0x20u
#define `$INSTANCE_NAME`_CAL_ON        0x10u
#define `$INSTANCE_NAME`_PWRDWN_OVRD   0x08u

#define `$INSTANCE_NAME`_PWR_MODE_MASK 0x06u
#define `$INSTANCE_NAME`_PWR_MODE_SLOW 0x00u
#define `$INSTANCE_NAME`_PWR_MODE_FAST 0x02u
#define `$INSTANCE_NAME`_PWR_MODE_ULOW 0x04u
#define `$INSTANCE_NAME`_ALL_OFF       0x00u

/* CLK (Comp Clock Control Register)      */
#define `$INSTANCE_NAME`_BYPASS_SYNC   0x10u
#define `$INSTANCE_NAME`_SYNC_CLK_EN   0x08u
#define `$INSTANCE_NAME`_CLKSYNC_MASK  0x18u
#define `$INSTANCE_NAME`_MX_CLK_MASK   0x07u
#define `$INSTANCE_NAME`_MX_CLK_A0     0x00u /* Select analog clk0 */
#define `$INSTANCE_NAME`_MX_CLK_A1     0x01u /* Select analog clk1 */
#define `$INSTANCE_NAME`_MX_CLK_A2     0x02u /* Select analog clk2 */
#define `$INSTANCE_NAME`_MX_CLK_A3     0x03u /* Select analog clk3 */
#define `$INSTANCE_NAME`_MX_CLK_DSI    0x04u /* Select UDB clock   */

/* WRK (Comp Output working Register)     */
#define `$INSTANCE_NAME`_CMP_EN        0x08u /* CHECK, mask must be set by fitter */

/* TR (Comp Trim Register)     */
#define `$INSTANCE_NAME`_CMP_TRIM1_DIR  0x08u /* Trim direction for N-type load for offset calibration */
#define `$INSTANCE_NAME`_CMP_TRIM1_MASK 0x07u /* Trim for N-type load for offset calibration */
#define `$INSTANCE_NAME`_CMP_TRIM2_DIR  0x80u /* Trim direction for P-type load for offset calibration */
#define `$INSTANCE_NAME`_CMP_TRIM2_MASK 0x70u /* Trim for P-type load for offset calibration */

/* WRK (Comp output working register)     */ 
#define `$INSTANCE_NAME`_CMP_OUT_MASK   `$INSTANCE_NAME`_ctComp__WRK_MASK /* Specific comparator out mask */

/* PM_ACT_CFG (Active Power Mode CFG Register)     */ 
#define `$INSTANCE_NAME`_ACT_PWR_EN     `$INSTANCE_NAME`_ctComp__PM_ACT_MSK /* Power enable mask */

/**************************************
*             Registers        
**************************************/

#define `$INSTANCE_NAME`_CR     (* (reg8 *) `$INSTANCE_NAME`_ctComp__CR ) /* Config register   */
#define `$INSTANCE_NAME`_CLK    (* (reg8 *) `$INSTANCE_NAME`_ctComp__CLK ) /* Comp clock comtrol register */
#define `$INSTANCE_NAME`_SW0    (* (reg8 *) `$INSTANCE_NAME`_ctComp__SW0 ) /* Routing registers */
#define `$INSTANCE_NAME`_SW2    (* (reg8 *) `$INSTANCE_NAME`_ctComp__SW2 )
#define `$INSTANCE_NAME`_SW3    (* (reg8 *) `$INSTANCE_NAME`_ctComp__SW3 )
#define `$INSTANCE_NAME`_SW4    (* (reg8 *) `$INSTANCE_NAME`_ctComp__SW4 )
#define `$INSTANCE_NAME`_SW6    (* (reg8 *) `$INSTANCE_NAME`_ctComp__SW6 )

#define `$INSTANCE_NAME`_TR     (* (reg8 *) `$INSTANCE_NAME`_ctComp__TR )
#define `$INSTANCE_NAME`_WRK    (* (reg8 *) `$INSTANCE_NAME`_ctComp__WRK )

#define `$INSTANCE_NAME`_PWRMGR (* (reg8 *) `$INSTANCE_NAME`_ctComp__PM_ACT_CFG )  /* Power manager */



#endif /* CY_COMP_`$INSTANCE_NAME`_H */
/* [] END OF FILE  */
