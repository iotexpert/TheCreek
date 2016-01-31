/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file contains the function prototypes and constants used in
*  the TIA User Module.
*
* Note:
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(CY_TIA_`$INSTANCE_NAME`_H) 
#define CY_TIA_`$INSTANCE_NAME`_H 

#include "cytypes.h"
#include "cyfitter.h"

/* Check to see if required defines such as CY_PSOC5LP are available */
/* They are defined starting with cy_boot v3.0 */
#if !defined (CY_PSOC5LP)
    #error Component `$CY_COMPONENT_NAME` requires cy_boot v3.0 or later
#endif /* (CY_ PSOC5LP) */


/***************************************
*   Data Struct Definition
***************************************/

/* Low power Mode API Support */
typedef struct _`$INSTANCE_NAME`_backupStruct
{
    uint8   enableState;
    uint8   scCr1Reg;
    uint8   scCr2Reg;
}   `$INSTANCE_NAME`_BACKUP_STRUCT;


/***************************************
*        Function Prototypes
***************************************/

void `$INSTANCE_NAME`_Start(void)                  `=ReentrantKeil($INSTANCE_NAME . "_Start")`;
void `$INSTANCE_NAME`_Stop(void)                   `=ReentrantKeil($INSTANCE_NAME . "_Stop")`;
void `$INSTANCE_NAME`_SetPower(uint8 power)        `=ReentrantKeil($INSTANCE_NAME . "_SetPower")`;
void `$INSTANCE_NAME`_SetResFB(uint8 res_feedback) `=ReentrantKeil($INSTANCE_NAME . "_SetResFB")`;
void `$INSTANCE_NAME`_SetCapFB(uint8 cap_feedback) `=ReentrantKeil($INSTANCE_NAME . "_SetCapFB")`;
void `$INSTANCE_NAME`_Sleep(void)                  `=ReentrantKeil($INSTANCE_NAME . "_Sleep")`;
void `$INSTANCE_NAME`_Wakeup(void)                 `=ReentrantKeil($INSTANCE_NAME . "_Wakeup")`;
void `$INSTANCE_NAME`_SaveConfig(void)             `=ReentrantKeil($INSTANCE_NAME . "_SaveConfig")`;
void `$INSTANCE_NAME`_RestoreConfig(void)          `=ReentrantKeil($INSTANCE_NAME . "_RestoreConfig")`;
void `$INSTANCE_NAME`_Init(void)                   `=ReentrantKeil($INSTANCE_NAME . "_Init")`;
void `$INSTANCE_NAME`_Enable(void)                 `=ReentrantKeil($INSTANCE_NAME . "_Enable")`;


/***************************************
*           API Constants
***************************************/

/* Power constants for SetPower function */
#define `$INSTANCE_NAME`_MINPOWER               (0x00u)
#define `$INSTANCE_NAME`_LOWPOWER               (0x01u)
#define `$INSTANCE_NAME`_MEDPOWER               (0x02u)
#define `$INSTANCE_NAME`_HIGHPOWER              (0x03u)

/* Constants for SetResFB function */
#define `$INSTANCE_NAME`_RES_FEEDBACK_20K       (0x00u)
#define `$INSTANCE_NAME`_RES_FEEDBACK_30K       (0x01u)
#define `$INSTANCE_NAME`_RES_FEEDBACK_40K       (0x02u)
#define `$INSTANCE_NAME`_RES_FEEDBACK_80K       (0x03u)
#define `$INSTANCE_NAME`_RES_FEEDBACK_120K      (0x04u)
#define `$INSTANCE_NAME`_RES_FEEDBACK_250K      (0x05u)
#define `$INSTANCE_NAME`_RES_FEEDBACK_500K      (0x06u)
#define `$INSTANCE_NAME`_RES_FEEDBACK_1000K     (0x07u)
#define `$INSTANCE_NAME`_RES_FEEDBACK_MAX       (0x07u)

/* Constants for SetCapFB function */
#define `$INSTANCE_NAME`_CAP_FEEDBACK_NONE      (0x00u)
#define `$INSTANCE_NAME`_CAP_FEEDBACK_1_3PF     (0x01u)
#define `$INSTANCE_NAME`_CAP_FEEDBACK_3_3PF     (0x02u)
#define `$INSTANCE_NAME`_CAP_FEEDBACK_4_6PF     (0x03u)
#define `$INSTANCE_NAME`_CAP_FEEDBACK_MAX       (0x03u)

/* Constant for VDDA Threshold */
#define `$INSTANCE_NAME`_CYDEV_VDDA_MV          (CYDEV_VDDA_MV)
#define `$INSTANCE_NAME`_VDDA_THRESHOLD_MV      (3500u)


/***************************************
*       Initial Paramater Values
***************************************/

#define `$INSTANCE_NAME`_INIT_POWER            (`$Power`)
#define `$INSTANCE_NAME`_INIT_RES_FEEDBACK     (`$Resistive_Feedback`)
#define `$INSTANCE_NAME`_INIT_CAP_FEEDBACK     (`$Capacitive_Feedback`)


/***************************************
*              Registers
***************************************/

#define `$INSTANCE_NAME`_CR0_REG            (* (reg8 *) `$INSTANCE_NAME`_SC__CR0 )
#define `$INSTANCE_NAME`_CR0_PTR            (  (reg8 *) `$INSTANCE_NAME`_SC__CR0 )
#define `$INSTANCE_NAME`_CR1_REG            (* (reg8 *) `$INSTANCE_NAME`_SC__CR1 )
#define `$INSTANCE_NAME`_CR1_PTR            (  (reg8 *) `$INSTANCE_NAME`_SC__CR1 )
#define `$INSTANCE_NAME`_CR2_REG            (* (reg8 *) `$INSTANCE_NAME`_SC__CR2 )
#define `$INSTANCE_NAME`_CR2_PTR            (  (reg8 *) `$INSTANCE_NAME`_SC__CR2 )
/* Power manager */
#define `$INSTANCE_NAME`_PM_ACT_CFG_REG     (* (reg8 *) `$INSTANCE_NAME`_SC__PM_ACT_CFG )
#define `$INSTANCE_NAME`_PM_ACT_CFG_PTR     (  (reg8 *) `$INSTANCE_NAME`_SC__PM_ACT_CFG )
/* Power manager */
#define `$INSTANCE_NAME`_PM_STBY_CFG_REG    (* (reg8 *) `$INSTANCE_NAME`_SC__PM_STBY_CFG )
#define `$INSTANCE_NAME`_PM_STBY_CFG_PTR    (  (reg8 *) `$INSTANCE_NAME`_SC__PM_STBY_CFG )
/* SC Boost Clk Control */
#define `$INSTANCE_NAME`_BSTCLK_REG         (* (reg8 *) `$INSTANCE_NAME`_SC__BST )
#define `$INSTANCE_NAME`_BSTCLK_PTR         (  (reg8 *) `$INSTANCE_NAME`_SC__BST )
/* Pump Register for SC block */
#define `$INSTANCE_NAME`_SC_MISC_REG        (* (reg8 *) CYDEV_ANAIF_RT_SC_MISC)
#define `$INSTANCE_NAME`_SC_MISC_PTR        (  (reg8 *) CYDEV_ANAIF_RT_SC_MISC)
/* Pump clock selectin register */
#define `$INSTANCE_NAME`_PUMP_CR1_REG       (* (reg8 *) CYDEV_ANAIF_CFG_PUMP_CR1)
#define `$INSTANCE_NAME`_PUMP_CR1_PTR       (  (reg8 *) CYDEV_ANAIF_CFG_PUMP_CR1)

/* PM_ACT_CFG (Active Power Mode CFG Register) mask */ 
#define `$INSTANCE_NAME`_ACT_PWR_EN     `$INSTANCE_NAME`_SC__PM_ACT_MSK 

/* PM_STBY_CFG (Alternative Active Power Mode CFG Register) mask */ 
#define `$INSTANCE_NAME`_STBY_PWR_EN     `$INSTANCE_NAME`_SC__PM_STBY_MSK 

/*Backward Compatibility*/
#define `$INSTANCE_NAME`_CR0             `$INSTANCE_NAME`_CR0_REG
#define `$INSTANCE_NAME`_CR1             `$INSTANCE_NAME`_CR1_REG
#define `$INSTANCE_NAME`_CR2             `$INSTANCE_NAME`_CR2_REG
#define `$INSTANCE_NAME`_BSTCLK          `$INSTANCE_NAME`_BSTCLK_REG
#define `$INSTANCE_NAME`_PWRMGR          `$INSTANCE_NAME`_PM_ACT_CFG_REG
#define `$INSTANCE_NAME`_SC_MISC         `$INSTANCE_NAME`_SC_MISC_REG


/***************************************
*         Register constants        
***************************************/

/* SC_MISC constants */
#define `$INSTANCE_NAME`_PUMP_FORCE             (0x20u)
#define `$INSTANCE_NAME`_PUMP_AUTO              (0x10u)
#define `$INSTANCE_NAME`_DIFF_PGA_1_3           (0x02u)
#define `$INSTANCE_NAME`_DIFF_PGA_0_2           (0x01u)

/* ANIF.PUMP.CR1 Constants */
#define `$INSTANCE_NAME`_PUMP_CR1_SC_CLKSEL     (0x80u)

/* CR0 SC/CT Control Register 0 definitions */

/* Bit Field SC_MODE_ENUM - SCxx_CR0[3:1], TIA Mode = 3b'001' */
#define `$INSTANCE_NAME`_MODE_TIA               (0x01u << 1)

/* CR1 SC/CT Control Register 1 definitions */

/* Bit Field  SC_DRIVE_ENUM - SCxx_CR1[1:0] */
#define `$INSTANCE_NAME`_DRIVE_MASK             (0x03u)
#define `$INSTANCE_NAME`_DRIVE_280UA            (0x00u)
#define `$INSTANCE_NAME`_DRIVE_420UA            (0x01u)
#define `$INSTANCE_NAME`_DRIVE_530UA            (0x02u)
#define `$INSTANCE_NAME`_DRIVE_650UA            (0x03u)

/* Bit Field  SC_COMP_ENUM - SCxx_CR1[3:2] */
#define `$INSTANCE_NAME`_COMP_MASK              (0x03u << 2)
#define `$INSTANCE_NAME`_COMP_3P0PF             (0x00u << 2)
#define `$INSTANCE_NAME`_COMP_3P6PF             (0x01u << 2)
#define `$INSTANCE_NAME`_COMP_4P35PF            (0x02u << 2)
#define `$INSTANCE_NAME`_COMP_5P1PF             (0x03u << 2)

/* Bit Field  SC_DIV2_ENUM - SCxx_CR1[4] - n/a for TIA mode */
#define `$INSTANCE_NAME`_DIV2                   (0x01u << 4)
#define `$INSTANCE_NAME`_DIV2_DISABLE           (0x00u << 4)
#define `$INSTANCE_NAME`_DIV2_ENABLE            (0x01u << 4)

/* Bit Field  SC_GAIN_ENUM - SCxx_CR1[5] - n/a for TIA mode] */
#define `$INSTANCE_NAME`_GAIN                   (0x01u << 5)
#define `$INSTANCE_NAME`_GAIN_0DB               (0x00u << 5)
#define `$INSTANCE_NAME`_GAIN_6DB               (0x01u << 5)

/* CR2 SC/CT Control Register 2 definitions */

/* Bit Field  SC_BIAS_CONTROL_ENUM - SCxx_CR2[0] */
#define `$INSTANCE_NAME`_BIAS                   (0x01u)
#define `$INSTANCE_NAME`_BIAS_NORMAL            (0x00u)
#define `$INSTANCE_NAME`_BIAS_LOW               (0x01u)

/* Bit Field  SC_R20_40B_ENUM - SCxx_CR2[1] - n/a for TIA mode */
#define `$INSTANCE_NAME`_R20_40B_MASK           (0x01u << 1)
#define `$INSTANCE_NAME`_R20_40B_40K            (0x00u << 1)
#define `$INSTANCE_NAME`_R20_40B_20K            (0x01u << 1)

/* Bit Field  SC_REDC_ENUM  - SCxx_CR2[3:2] */
#define `$INSTANCE_NAME`_REDC_MASK              (0x03u << 2)
#define `$INSTANCE_NAME`_REDC_00                (0x00u << 2)
#define `$INSTANCE_NAME`_REDC_01                (0x02u << 2)
#define `$INSTANCE_NAME`_REDC_10                (0x04u << 2)
#define `$INSTANCE_NAME`_REDC_11                (0x03u << 2)

/* Bit Field  SC_RVAL_ENUM  - SCxx_CR2[6:4] */
#define `$INSTANCE_NAME`_RVAL_MASK              (0x07u << 4)
#define `$INSTANCE_NAME`_RVAL_20K               (0x00u << 4)
#define `$INSTANCE_NAME`_RVAL_30K               (0x01u << 4)
#define `$INSTANCE_NAME`_RVAL_40K               (0x02u << 4)
#define `$INSTANCE_NAME`_RVAL_60K               (0x03u << 4)
#define `$INSTANCE_NAME`_RVAL_120K              (0x04u << 4)
#define `$INSTANCE_NAME`_RVAL_250K              (0x05u << 4)
#define `$INSTANCE_NAME`_RVAL_500K              (0x06u << 4)
#define `$INSTANCE_NAME`_RVAL_1000K             (0x07u << 4)

/* Bit Field  SC_PGA_GNDVREF_ENUM - SCxx_CR2[7] - n/a for TIA mode */
#define `$INSTANCE_NAME`_GNDVREF                (0x01u << 7)
#define `$INSTANCE_NAME`_GNDVREF_DI             (0x00u << 7)
#define `$INSTANCE_NAME`_GNDVREF_E              (0x01u << 7)

/* SC Blook Boost Clock Selection Register - Boost Clock Enable  SCxx_BST[3] */
#define `$INSTANCE_NAME`_BST_CLK_EN             (0x01u << 3)

/* Constant for Minimum VDDA Threshold */
#define `$INSTANCE_NAME`_MINIMUM_VDDA_THRESHOLD_MV   (2700u)

#define `$INSTANCE_NAME`_SC_REG_CLR         (0x00u)

#endif /* CY_TIA_`$INSTANCE_NAME`_H */


/* [] END OF FILE */
