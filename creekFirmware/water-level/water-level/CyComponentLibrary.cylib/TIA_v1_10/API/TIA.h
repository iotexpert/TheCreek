/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*    This file contains the function prototypes and constants used in
*    the TIA User Module.
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

#if !defined(CY_TIA_`$INSTANCE_NAME`_H) 
#define CY_TIA_`$INSTANCE_NAME`_H 

/***************************************
*        Function Prototypes 
***************************************/

void    `$INSTANCE_NAME`_Start(void);
void    `$INSTANCE_NAME`_Stop(void);
void    `$INSTANCE_NAME`_SetPower(uint8 power);
void    `$INSTANCE_NAME`_SetResFB(uint8 res_feedback);
void    `$INSTANCE_NAME`_SetCapFB(uint8 cap_feedback);


/***************************************
*       Initial Paramater Values
***************************************/

#define `$INSTANCE_NAME`_INIT_POWER         `$Power`
#define `$INSTANCE_NAME`_INIT_RES_FEEDBACK    `$Resistive_Feedback`
#define `$INSTANCE_NAME`_INIT_CAP_FEEDBACK    `$Capacitive_Feedback`
#define `$INSTANCE_NAME`_MIN_VDDA           `$Minimum_Vdda`

/***************************************
*              Constants        
***************************************/
/* Constants for Minimum Vdda paramater */
#define  `$INSTANCE_NAME`_MIN_VDDA_GTE_2_7V     0x00u
#define  `$INSTANCE_NAME`_MIN_VDDA_LT_2_7V      0x01u

/* Power constants for SetPower function */
#define `$INSTANCE_NAME`_MINPOWER               0x00u
#define `$INSTANCE_NAME`_LOWPOWER               0x01u
#define `$INSTANCE_NAME`_MEDPOWER               0x02u
#define `$INSTANCE_NAME`_HIGHPOWER              0x03u

/* Constants for SetResFB function */
#define `$INSTANCE_NAME`_RES_FEEDBACK_20K       0x00u
#define `$INSTANCE_NAME`_RES_FEEDBACK_30K       0x01u
#define `$INSTANCE_NAME`_RES_FEEDBACK_40K       0x02u
#define `$INSTANCE_NAME`_RES_FEEDBACK_80K       0x03u
#define `$INSTANCE_NAME`_RES_FEEDBACK_120K      0x04u
#define `$INSTANCE_NAME`_RES_FEEDBACK_250K      0x05u
#define `$INSTANCE_NAME`_RES_FEEDBACK_500K      0x06u
#define `$INSTANCE_NAME`_RES_FEEDBACK_1000K     0x07u
#define `$INSTANCE_NAME`_RES_FEEDBACK_MAX       0x07u

/* Constants for SetCapFB function */
#define `$INSTANCE_NAME`_CAP_FEEDBACK_NONE      0x00u
#define `$INSTANCE_NAME`_CAP_FEEDBACK_1_3PF     0x01u
#define `$INSTANCE_NAME`_CAP_FEEDBACK_3_3PF     0x02u
#define `$INSTANCE_NAME`_CAP_FEEDBACK_4_6PF     0x03u
#define `$INSTANCE_NAME`_CAP_FEEDBACK_MAX       0x03u

/* Constant for VDDA Threshold */
#define `$INSTANCE_NAME`_CYDEV_VDDA_MV  CYDEV_VDDA_MV
#define `$INSTANCE_NAME`_VDDA_THRESHOLD_MV 3500u

/* SC_MISC constants */
#define `$INSTANCE_NAME`_PUMP_FORCE     0x20u
#define `$INSTANCE_NAME`_PUMP_AUTO      0x10u
#define `$INSTANCE_NAME`_DIFF_PGA_1_3   0x02u
#define `$INSTANCE_NAME`_DIFF_PGA_0_2   0x01u

/***************************************
*              Registers        
***************************************/

#define `$INSTANCE_NAME`_CR0    (* (reg8 *) `$INSTANCE_NAME`_SC__CR0 )
#define `$INSTANCE_NAME`_CR1    (* (reg8 *) `$INSTANCE_NAME`_SC__CR1 )
#define `$INSTANCE_NAME`_CR2    (* (reg8 *) `$INSTANCE_NAME`_SC__CR2 )
#define `$INSTANCE_NAME`_PWRMGR (* (reg8 *) `$INSTANCE_NAME`_SC__PM_ACT_CFG )   /* Power manager */
#define `$INSTANCE_NAME`_BSTCLK (* (reg8 *) `$INSTANCE_NAME`_SC__BST )          /* SC Boost Clk Control */

/* CR0 SC/CT Control Register 0 definitions */

/* Bit Field SC_MODE_ENUM - SCxx_CR0[3:1], TIA Mode = 3b'001' */
#define `$INSTANCE_NAME`_MODE_TIA      (0x01u << 1) 

/* CR1 SC/CT Control Register 1 definitions */

/* Bit Field  SC_DRIVE_ENUM - SCxx_CR1[1:0] */
#define `$INSTANCE_NAME`_DRIVE_MASK     0x03u
#define `$INSTANCE_NAME`_DRIVE_280UA    0x00u
#define `$INSTANCE_NAME`_DRIVE_420UA    0x01u
#define `$INSTANCE_NAME`_DRIVE_530UA    0x02u
#define `$INSTANCE_NAME`_DRIVE_650UA    0x03u

/* Bit Field  SC_COMP_ENUM - SCxx_CR1[3:2]  */
#define `$INSTANCE_NAME`_COMP_MASK      (0x03u << 2)
#define `$INSTANCE_NAME`_COMP_3P0PF     (0x00u << 2)
#define `$INSTANCE_NAME`_COMP_3P6PF     (0x01u << 2)
#define `$INSTANCE_NAME`_COMP_4P35PF    (0x02u << 2)
#define `$INSTANCE_NAME`_COMP_5P1PF     (0x03u << 2)

/* Bit Field  SC_DIV2_ENUM - SCxx_CR1[4] - n/a for TIA mode */
#define `$INSTANCE_NAME`_DIV2           (0x01u << 4)
#define `$INSTANCE_NAME`_DIV2_DISABLE   (0x00u << 4)
#define `$INSTANCE_NAME`_DIV2_ENABLE    (0x01u << 4)

/* Bit Field  SC_GAIN_ENUM - SCxx_CR1[5] - n/a for TIA mode] */
#define `$INSTANCE_NAME`_GAIN           (0x01u << 5)
#define `$INSTANCE_NAME`_GAIN_0DB       (0x00u << 5)
#define `$INSTANCE_NAME`_GAIN_6DB       (0x01u << 5)

/* CR2 SC/CT Control Register 2 definitions */

/* Bit Field  SC_BIAS_CONTROL_ENUM - SCxx_CR2[0] */
#define `$INSTANCE_NAME`_BIAS           0x01u
#define `$INSTANCE_NAME`_BIAS_NORMAL    0x00u
#define `$INSTANCE_NAME`_BIAS_LOW       0x01u

/* Bit Field  SC_R20_40B_ENUM - SCxx_CR2[1] - n/a for TIA mode  */
#define `$INSTANCE_NAME`_R20_40B_MASK   (0x01u << 1)
#define `$INSTANCE_NAME`_R20_40B_40K    (0x00u << 1)
#define `$INSTANCE_NAME`_R20_40B_20K    (0x01u << 1)

/* Bit Field  SC_REDC_ENUM  - SCxx_CR2[3:2] */
#define `$INSTANCE_NAME`_REDC_MASK      (0x03u << 2)
#define `$INSTANCE_NAME`_REDC_00        (0x00u << 2)
#define `$INSTANCE_NAME`_REDC_01        (0x02u << 2)
#define `$INSTANCE_NAME`_REDC_10        (0x04u << 2)
#define `$INSTANCE_NAME`_REDC_11        (0x03u << 2)

/* Bit Field  SC_RVAL_ENUM  - SCxx_CR2[6:4]  */
#define `$INSTANCE_NAME`_RVAL_MASK      (0x07u << 4)
#define `$INSTANCE_NAME`_RVAL_20K       (0x00u << 4)
#define `$INSTANCE_NAME`_RVAL_30K       (0x01u << 4)
#define `$INSTANCE_NAME`_RVAL_40K       (0x02u << 4)
#define `$INSTANCE_NAME`_RVAL_60K       (0x03u << 4)
#define `$INSTANCE_NAME`_RVAL_120K      (0x04u << 4)
#define `$INSTANCE_NAME`_RVAL_250K      (0x05u << 4)
#define `$INSTANCE_NAME`_RVAL_500K      (0x06u << 4)
#define `$INSTANCE_NAME`_RVAL_1000K     (0x07u << 4)

/* Bit Field  SC_PGA_GNDVREF_ENUM - SCxx_CR2[7] - n/a for TIA mode   */
#define `$INSTANCE_NAME`_GNDVREF        (0x01u << 7)
#define `$INSTANCE_NAME`_GNDVREF_DI     (0x00u << 7)
#define `$INSTANCE_NAME`_GNDVREF_E      (0x01u << 7)

/* SC Blook Boost Clock Selection Register - Boost Clock Enable  SCxx_BST[3]  */
#define `$INSTANCE_NAME`_BST_CLK_EN        (0x01u << 3)

/* PM_ACT_CFG (Active Power Mode CFG Register)     */ 
#define `$INSTANCE_NAME`_ACT_PWR_EN     `$INSTANCE_NAME`_SC__PM_ACT_MSK /* Power enable mask */

/* Pump Register for SC block */
#define `$INSTANCE_NAME`_SC_MISC (* (reg8 *) CYDEV_ANAIF_RT_SC_MISC)

#endif /* CY_TIA_`$INSTANCE_NAME`_H */

/* [] END OF FILE */
