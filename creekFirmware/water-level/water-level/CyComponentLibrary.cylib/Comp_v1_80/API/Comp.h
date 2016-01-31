/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*    This file contains the function prototypes and constants used in
*    the Analog Comparator User Module.
*
*  Note:
*     
*
********************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

#if !defined(CY_COMP_`$INSTANCE_NAME`_H) 
#define CY_COMP_`$INSTANCE_NAME`_H

#include "cytypes.h"
#include "CyLib.h"
#include "cyfitter.h" 

#define `$INSTANCE_NAME`_RECALMODE 0u


/***************************************
*       Type defines
***************************************/

/* Sleep Mode API Support */
typedef struct `$INSTANCE_NAME`_backupStruct
{
    uint8 enableState;
    uint8 compCRReg;
}`$INSTANCE_NAME`_backupStruct;


/**************************************
*        Function Prototypes 
**************************************/

void    `$INSTANCE_NAME`_Start(void);
void    `$INSTANCE_NAME`_Stop(void)               `=ReentrantKeil($INSTANCE_NAME . "_Stop")`;
void    `$INSTANCE_NAME`_SetSpeed(uint8 speed)    `=ReentrantKeil($INSTANCE_NAME . "_SetSpeed")`;
uint8   `$INSTANCE_NAME`_GetCompare(void)         `=ReentrantKeil($INSTANCE_NAME . "_GetCompare")`;
uint16  `$INSTANCE_NAME`_ZeroCal(void)            `=ReentrantKeil($INSTANCE_NAME . "_ZeroCal")`;
void    `$INSTANCE_NAME`_LoadTrim(uint16 trimVal) `=ReentrantKeil($INSTANCE_NAME . "_LoadTrim")`;
void `$INSTANCE_NAME`_Init(void)                  `=ReentrantKeil($INSTANCE_NAME . "_Init")`; 
void `$INSTANCE_NAME`_Enable(void)                `=ReentrantKeil($INSTANCE_NAME . "_Enable")`;
void `$INSTANCE_NAME`_SaveConfig(void);
void `$INSTANCE_NAME`_RestoreConfig(void);
void `$INSTANCE_NAME`_Sleep(void);
void `$INSTANCE_NAME`_Wakeup(void)                `=ReentrantKeil($INSTANCE_NAME . "_Wakeup")`;
/* Below APIs are valid only for PSoC3 silicon.*/
#if (CY_PSOC3) 
    void `$INSTANCE_NAME`_PwrDwnOverrideEnable(void) `=ReentrantKeil($INSTANCE_NAME . "_PwrDwnOverrideEnable")`;
    void `$INSTANCE_NAME`_PwrDwnOverrideDisable(void) `=ReentrantKeil($INSTANCE_NAME . "_PwrDwnOverrideDisable")`;
#endif /* CY_PSOC3 */


/**************************************
*           API Constants        
**************************************/

/* Power constants for SetSpeed() function */
#define `$INSTANCE_NAME`_SLOWSPEED   0x00u
#define `$INSTANCE_NAME`_HIGHSPEED   0x01u
#define `$INSTANCE_NAME`_LOWPOWER    0x02u


/***************************************
*         Trim Locations               
****************************************/

/* High speed trim values */
#define `$INSTANCE_NAME`_HS_TRIM_TR0        (CY_GET_XTND_REG8(`$INSTANCE_NAME`_ctComp__TRIM__TR0_HS))

#if (CY_PSOC3_ES3 || CY_PSOC5_ES2)
    #define `$INSTANCE_NAME`_HS_TRIM_TR1    (CY_GET_XTND_REG8(`$INSTANCE_NAME`_ctComp__TRIM__TR1_HS))
#endif /* (CY_PSOC3_ES3 || CY_PSOC5_ES2) */

/* Low speed trim values */
#define `$INSTANCE_NAME`_LS_TRIM_TR0        (CY_GET_XTND_REG8(`$INSTANCE_NAME`_ctComp__TRIM__TR0 + 1))

#if (CY_PSOC3_ES3 || CY_PSOC5_ES2)
    #define `$INSTANCE_NAME`_LS_TRIM_TR1    (CY_GET_XTND_REG8(`$INSTANCE_NAME`_ctComp__TRIM__TR1 + 1))
#endif /* CY_PSOC3_ES3 || CY_PSOC5_ES2 */


/**************************************
*           Parameter Defaults        
**************************************/

#define `$INSTANCE_NAME`_DEFAULT_SPEED       `$Speed`u 
#define `$INSTANCE_NAME`_DEFAULT_HYSTERESIS  `$Hysteresis`u
#define `$INSTANCE_NAME`_DEFAULT_POLARITY    `$Polarity`u
#define `$INSTANCE_NAME`_DEFAULT_BYPASS_SYNC `$Sync`u
#define `$INSTANCE_NAME`_DEFAULT_PWRDWN_OVRD `$Pd_Override`u


/**************************************
*             Registers        
**************************************/

#define `$INSTANCE_NAME`_CR      (* (reg8 *) `$INSTANCE_NAME`_ctComp__CR )   /* Config register   */
#define `$INSTANCE_NAME`_CR_PTR  (  (reg8 *) `$INSTANCE_NAME`_ctComp__CR )
#define `$INSTANCE_NAME`_CLK     (* (reg8 *) `$INSTANCE_NAME`_ctComp__CLK )  /* Comp clock control register */
#define `$INSTANCE_NAME`_CLK_PTR (  (reg8 *) `$INSTANCE_NAME`_ctComp__CLK )
#define `$INSTANCE_NAME`_SW0     (* (reg8 *) `$INSTANCE_NAME`_ctComp__SW0 )  /* Routing registers */
#define `$INSTANCE_NAME`_SW0_PTR (  (reg8 *) `$INSTANCE_NAME`_ctComp__SW0 )
#define `$INSTANCE_NAME`_SW2     (* (reg8 *) `$INSTANCE_NAME`_ctComp__SW2 )
#define `$INSTANCE_NAME`_SW2_PTR (  (reg8 *) `$INSTANCE_NAME`_ctComp__SW2 )
#define `$INSTANCE_NAME`_SW3     (* (reg8 *) `$INSTANCE_NAME`_ctComp__SW3 )
#define `$INSTANCE_NAME`_SW3_PTR (  (reg8 *) `$INSTANCE_NAME`_ctComp__SW3 )
#define `$INSTANCE_NAME`_SW4     (* (reg8 *) `$INSTANCE_NAME`_ctComp__SW4 )
#define `$INSTANCE_NAME`_SW4_PTR (  (reg8 *) `$INSTANCE_NAME`_ctComp__SW4 )
#define `$INSTANCE_NAME`_SW6     (* (reg8 *) `$INSTANCE_NAME`_ctComp__SW6 )
#define `$INSTANCE_NAME`_SW6_PTR (  (reg8 *) `$INSTANCE_NAME`_ctComp__SW6 )

/* Trim registers */
/* PSoC3 ES2 or early, PSoC5 ES1 or early */
#if (CY_PSOC3_ES2 || CY_PSOC5_ES1)
    #define `$INSTANCE_NAME`_TR      (* (reg8 *) `$INSTANCE_NAME`_ctComp__TR )   /* Trim registers */
    #define `$INSTANCE_NAME`_TR_PTR  (  (reg8 *) `$INSTANCE_NAME`_ctComp__TR )
#endif /* CY_PSOC3_ES2 || CY_PSOC5_ES1 */

/* PSoC3 ES3 or later, PSoC5 ES2 or later */
#if (CY_PSOC3_ES3 || CY_PSOC5_ES2) 
    #define `$INSTANCE_NAME`_TR0         (* (reg8 *) `$INSTANCE_NAME`_ctComp__TR0 ) /* Trim register for P-type load */
    #define `$INSTANCE_NAME`_TR0_PTR     (  (reg8 *) `$INSTANCE_NAME`_ctComp__TR0 ) 
    #define `$INSTANCE_NAME`_TR1         (* (reg8 *) `$INSTANCE_NAME`_ctComp__TR1 ) /* Trim register for N-type load */
    #define `$INSTANCE_NAME`_TR1_PTR     (  (reg8 *) `$INSTANCE_NAME`_ctComp__TR1 ) 
#endif /* CY_PSOC3_ES3 || CY_PSOC5_ES2 */

#define `$INSTANCE_NAME`_WRK             (* (reg8 *) `$INSTANCE_NAME`_ctComp__WRK )  /* Working register - output */
#define `$INSTANCE_NAME`_WRK_PTR         (  (reg8 *) `$INSTANCE_NAME`_ctComp__WRK )
#define `$INSTANCE_NAME`_PWRMGR          (* (reg8 *) `$INSTANCE_NAME`_ctComp__PM_ACT_CFG )  /* Active Power manager */
#define `$INSTANCE_NAME`_PWRMGR_PTR      ( (reg8 *) `$INSTANCE_NAME`_ctComp__PM_ACT_CFG )
#define `$INSTANCE_NAME`_STBY_PWRMGR     (* (reg8 *) `$INSTANCE_NAME`_ctComp__PM_STBY_CFG )  /* Standby Power manager */
#define `$INSTANCE_NAME`_STBY_PWRMGR_PTR ( (reg8 *) `$INSTANCE_NAME`_ctComp__PM_STBY_CFG )


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

/* SW3 Routing Register definitions */
#define `$INSTANCE_NAME`_CMP_SW3_INPCTL_MASK    0x09u   /* SW3 bits for inP routing control */

/* TR (Comp Trim Register)     */
#define `$INSTANCE_NAME`_DEFAULT_CMP_TRIM  0x00u

/* PSoC3 ES2 or early, PSoC5 ES1 or early */
#if (CY_PSOC3_ES2 || CY_PSOC5_ES1)
    #define `$INSTANCE_NAME`_CMP_TRIM1_DIR  0x08u   /* Trim direction for N-type load for offset calibration */
    #define `$INSTANCE_NAME`_CMP_TRIM1_MASK 0x07u   /* Trim for N-type load for offset calibration */
    #define `$INSTANCE_NAME`_CMP_TRIM2_DIR  0x80u   /* Trim direction for P-type load for offset calibration */
    #define `$INSTANCE_NAME`_CMP_TRIM2_MASK 0x70u   /* Trim for P-type load for offset calibration */
#endif /* CY_PSOC3_ES2 || CY_PSOC5_ES1 */

/* PSoC3 ES3 or later, PSoC5 ES2 or later */
#if (CY_PSOC3_ES3 || CY_PSOC5_ES2)
    #define `$INSTANCE_NAME`_CMP_TR0_DIR 0x11u    /* Trim direction for N-type load for offset calibration */
    #define `$INSTANCE_NAME`_CMP_TR0_MASK 0x1Fu   /* Trim for N-type load for offset calibration */
    #define `$INSTANCE_NAME`_CMP_TR1_DIR 0x10u    /* Trim direction for P-type load for offset calibration */
    #define `$INSTANCE_NAME`_CMP_TR1_MASK 0x1Fu   /* Trim for P-type load for offset calibration */ 
#endif /* CY_PSOC3_ES3 || CY_PSOC5_ES2 */


/* WRK (Comp output working register)     */ 
#define `$INSTANCE_NAME`_CMP_OUT_MASK   `$INSTANCE_NAME`_ctComp__WRK_MASK /* Specific comparator out mask */

/* PM_ACT_CFG7 (Active Power Mode CFG Register)     */ 
#define `$INSTANCE_NAME`_ACT_PWR_EN     `$INSTANCE_NAME`_ctComp__PM_ACT_MSK /* Power enable mask */

/* PM_STBY_CFG7 (Standby Power Mode CFG Register)     */ 
#define `$INSTANCE_NAME`_STBY_PWR_EN     `$INSTANCE_NAME`_ctComp__PM_STBY_MSK /* Standby Power enable mask */

#if (CY_PSOC5_ES1)
    /* For stop API changes mask to make the COMP register CR to 0X00  */
    #define `$INSTANCE_NAME`_COMP_REG_CLR             (0x00u)
#endif /* CY_PSOC5_ES1 */

#endif /* CY_COMP_`$INSTANCE_NAME`_H */


/* [] END OF FILE */
