/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file contains the function prototypes and constants used in
*  the OpAmp (Analog Buffer) Component.
*
* Note:
*     
********************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

#if !defined(CY_OPAMP_`$INSTANCE_NAME`_H) 
#define CY_OPAMP_`$INSTANCE_NAME`_H 

#include "cytypes.h"
#include "cyfitter.h"


/***************************************  
* Conditional Compilation Parameters
***************************************/

/* PSoC3 ES2 or early */
#define `$INSTANCE_NAME`_PSOC3_ES2  ((CYDEV_CHIP_MEMBER_USED == CYDEV_CHIP_MEMBER_3A)    && \
                                    (CYDEV_CHIP_REVISION_USED <= CYDEV_CHIP_REVISION_3A_ES2))
/* PSoC5 ES1 or early */
#define `$INSTANCE_NAME`_PSOC5_ES1  ((CYDEV_CHIP_MEMBER_USED == CYDEV_CHIP_MEMBER_5A)    && \
                                    (CYDEV_CHIP_REVISION_USED <= CYDEV_CHIP_REVISION_5A_ES1))
/* PSoC3 ES3 or later */
#define `$INSTANCE_NAME`_PSOC3_ES3  ((CYDEV_CHIP_MEMBER_USED == CYDEV_CHIP_MEMBER_3A)    && \
                                    (CYDEV_CHIP_REVISION_USED > CYDEV_CHIP_REVISION_3A_ES2))
/* PSoC5 ES2 or later */
#define `$INSTANCE_NAME`_PSOC5_ES2  ((CYDEV_CHIP_MEMBER_USED == CYDEV_CHIP_MEMBER_5A)    && \
                                    (CYDEV_CHIP_REVISION_USED > CYDEV_CHIP_REVISION_5A_ES1))


/***************************************
*   Data Struct Definition
***************************************/

/* Low power Mode API Support */
typedef struct _`$INSTANCE_NAME`_backupStruct
{
    uint8   enableState;
}   `$INSTANCE_NAME`_BACKUP_STRUCT;


/**************************************
*        Function Prototypes 
**************************************/

void `$INSTANCE_NAME`_Start(void);
void `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`;
void `$INSTANCE_NAME`_SetPower(uint8 power) `=ReentrantKeil($INSTANCE_NAME . "_SetPower")`;
void `$INSTANCE_NAME`_Sleep(void);
void `$INSTANCE_NAME`_Wakeup(void) `=ReentrantKeil($INSTANCE_NAME . "_Wakeup")`;
void `$INSTANCE_NAME`_SaveConfig(void);
void `$INSTANCE_NAME`_RestoreConfig(void);
void `$INSTANCE_NAME`_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_Init")`;
void `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`;


/**************************************
*           API Constants        
**************************************/

/* Power constants for SetPower() function */
#if (`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2)
    #define `$INSTANCE_NAME`_LPOCPOWER (0x00u)
    #define `$INSTANCE_NAME`_LOWPOWER  (0x01u)
    #define `$INSTANCE_NAME`_MEDPOWER  (0x02u)
    #define `$INSTANCE_NAME`_HIGHPOWER (0x03u)
#else
    #define `$INSTANCE_NAME`_LOWPOWER  (0x01u)
    #define `$INSTANCE_NAME`_MEDPOWER  (0x02u)
    #define `$INSTANCE_NAME`_HIGHPOWER (0x00u)
#endif

/**************************************
*           Parameter Defaults        
**************************************/
#if (`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2)
    #define `$INSTANCE_NAME`_DEFAULT_POWER    (`$Power`u)
#endif

#if (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
    #if (`$Power`u == 0)
        #error "Low Power Over Compensated Mode is not supported on ES2 silicon. Please select another default power setting"
    #elif (`$Power`u == 3)
        #define `$INSTANCE_NAME`_DEFAULT_POWER (0u)
    #else
        #define `$INSTANCE_NAME`_DEFAULT_POWER (`$Power`u)
    #endif
#endif

#define `$INSTANCE_NAME`_DEFAULT_MODE     (`$Mode`u)


/**************************************
*             Registers        
**************************************/

#define `$INSTANCE_NAME`_CR_REG        (* (reg8 *) `$INSTANCE_NAME`_ABuf__CR)
#define `$INSTANCE_NAME`_CR_PTR        (  (reg8 *) `$INSTANCE_NAME`_ABuf__CR)

#define `$INSTANCE_NAME`_MX_REG        (* (reg8 *) `$INSTANCE_NAME`_ABuf__MX)
#define `$INSTANCE_NAME`_MX_PTR        (  (reg8 *) `$INSTANCE_NAME`_ABuf__MX)

#define `$INSTANCE_NAME`_SW_REG        (* (reg8 *) `$INSTANCE_NAME`_ABuf__SW)
#define `$INSTANCE_NAME`_SW_PTR        (  (reg8 *) `$INSTANCE_NAME`_ABuf__SW)

/* Active mode power manager register */
#define `$INSTANCE_NAME`_PM_ACT_CFG_REG    (* (reg8 *) `$INSTANCE_NAME`_ABuf__PM_ACT_CFG)
#define `$INSTANCE_NAME`_PM_ACT_CFG_PTR    (  (reg8 *) `$INSTANCE_NAME`_ABuf__PM_ACT_CFG)

/* Alternative mode power manager register */
#define `$INSTANCE_NAME`_PM_STBY_CFG_REG    (* (reg8 *) `$INSTANCE_NAME`_ABuf__PM_STBY_CFG)
#define `$INSTANCE_NAME`_PM_STBY_CFG_PTR    (  (reg8 *) `$INSTANCE_NAME`_ABuf__PM_STBY_CFG)

/* ANIF.PUMP.CR1 Pump Configuration Register 1 */
#define `$INSTANCE_NAME`_PUMP_CR1_REG  (* (reg8 *) CYDEV_ANAIF_CFG_PUMP_CR1)
#define `$INSTANCE_NAME`_PUMP_CR1_PTR  (  (reg8 *) CYDEV_ANAIF_CFG_PUMP_CR1)

/* Trim register defines */
#define `$INSTANCE_NAME`_TRO_REG       (* (reg8 *) `$INSTANCE_NAME`_ABuf__TR0)
#define `$INSTANCE_NAME`_TRO_PTR       (  (reg8 *) `$INSTANCE_NAME`_ABuf__TR0)

#define `$INSTANCE_NAME`_TR1_REG       (* (reg8 *) `$INSTANCE_NAME`_ABuf__TR1)
#define `$INSTANCE_NAME`_TR1_PTR       (  (reg8 *) `$INSTANCE_NAME`_ABuf__TR1)

/* PM_ACT_CFG (Active Power Mode CFG Register) mask */ 
#define `$INSTANCE_NAME`_ACT_PWR_EN   `$INSTANCE_NAME`_ABuf__PM_ACT_MSK 

/* PM_STBY_CFG (Alternative Active Power Mode CFG Register) mask */ 
#define `$INSTANCE_NAME`_STBY_PWR_EN   `$INSTANCE_NAME`_ABuf__PM_STBY_MSK 


/**************************************
*       Register Constants        
**************************************/

/* CX Analog Buffer Input Selection Register */

/* Power mode defines */
#define `$INSTANCE_NAME`_PWR_MASK           (0x03u)
#if (`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2)
    #define `$INSTANCE_NAME`_PWR_LPOC        (0x00u)
    #define `$INSTANCE_NAME`_PWR_LOW         (0x01u)
    #define `$INSTANCE_NAME`_PWR_MEDIUM      (0x02u)
    #define `$INSTANCE_NAME`_PWR_HIGH        (0x03u)
#else
    #define `$INSTANCE_NAME`_PWR_LOW         (0x01u)
    #define `$INSTANCE_NAME`_PWR_MEDIUM      (0x02u)
    #define `$INSTANCE_NAME`_PWR_HIGH        (0x00u)
#endif

/* MX Analog Buffer Input Selection Register */

/* Bit Field  MX_VN */
#define `$INSTANCE_NAME`_MX_VN_MASK       (0x30u)
#define `$INSTANCE_NAME`_MX_VN_NC         (0x00u)
#define `$INSTANCE_NAME`_MX_VN_AG4        (0x20u)
#define `$INSTANCE_NAME`_MX_VN_AG6        (0x30u)

/* Bit Field  MX_VP */
#define `$INSTANCE_NAME`_MX_VP_MASK       (0x0Fu)
#define `$INSTANCE_NAME`_MX_VP_NC         (0x00u)
#define `$INSTANCE_NAME`_MX_VP_VREF       (0x07u)
#define `$INSTANCE_NAME`_MX_VP_AG4        (0x08u)
#define `$INSTANCE_NAME`_MX_VP_AG5        (0x09u)
#define `$INSTANCE_NAME`_MX_VP_AG6        (0x0Au)
#define `$INSTANCE_NAME`_MX_VP_AG7        (0x08u)
#define `$INSTANCE_NAME`_MX_VP_ABUS0      (0x0Cu)
#define `$INSTANCE_NAME`_MX_VP_ABUS1      (0x0Du)
#define `$INSTANCE_NAME`_MX_VP_ABUS2      (0x0Eu)
#define `$INSTANCE_NAME`_MX_VP_ABUS3      (0x0Fu)

/* SW Analog Buffer Routing Switch Reg */

/* Bit Field  SW */
#define `$INSTANCE_NAME`_SW_MASK          (0x07u) 
#define `$INSTANCE_NAME`_SW_SWINP         (0x04u)  /* Enable positive input */
#define `$INSTANCE_NAME`_SW_SWINN         (0x02u)  /* Enable negative input */
#define `$INSTANCE_NAME`_SW_SWFOL         (0x01u)  /* Enable follower mode  */

/* Pump configuration register masks */
#define `$INSTANCE_NAME`_PUMP_CR1_CLKSEL  (0x40u)
#define `$INSTANCE_NAME`_PUMP_CR1_FORCE   (0x20u)
#define `$INSTANCE_NAME`_PUMP_CR1_AUTO    (0x10u)

#endif /* CY_OPAMP_`$INSTANCE_NAME`_H */



/* [] END OF FILE */
