/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*    This file contains the function prototypes and constants used in
*    the 8-bit Voltage DAC (vDAC8) User Module.
*
*   Note:
*     
*
********************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(CY_VDAC8_`$INSTANCE_NAME`_H) 
#define CY_VDAC8_`$INSTANCE_NAME`_H

#include "cytypes.h"
#include "cyfitter.h"



/***************************************
*       Type defines
***************************************/

/* Sleep Mode API Support */
typedef struct `$INSTANCE_NAME`_backupStruct
{
    uint8 enableState; 
    uint8 data_value;
}`$INSTANCE_NAME`_backupStruct;


/***************************************
*        Function Prototypes 
***************************************/

void `$INSTANCE_NAME`_Start(void);
void `$INSTANCE_NAME`_Stop(void)            `=ReentrantKeil($INSTANCE_NAME . "_Stop")`;
void `$INSTANCE_NAME`_SetSpeed(uint8 speed) `=ReentrantKeil($INSTANCE_NAME . "_SetSpeed")`;
void `$INSTANCE_NAME`_SetRange(uint8 range) `=ReentrantKeil($INSTANCE_NAME . "_SetRange")`;
void `$INSTANCE_NAME`_SetValue(uint8 value) `=ReentrantKeil($INSTANCE_NAME . "_SetValue")`;
void `$INSTANCE_NAME`_DacTrim(void)         `=ReentrantKeil($INSTANCE_NAME . "_DacTrim")`;
void `$INSTANCE_NAME`_Init(void)            `=ReentrantKeil($INSTANCE_NAME . "_Init")`;
void `$INSTANCE_NAME`_Enable(void)          `=ReentrantKeil($INSTANCE_NAME . "_Enable")`;
void `$INSTANCE_NAME`_SaveConfig(void);
void `$INSTANCE_NAME`_RestoreConfig(void);
void `$INSTANCE_NAME`_Sleep(void);
void `$INSTANCE_NAME`_Wakeup(void)          `=ReentrantKeil($INSTANCE_NAME . "_Wakeup")`;
  
  
/***************************************
*            API Constants
***************************************/

/* SetRange constants */
`#cy_declare_enum vDacRange`
#define `$INSTANCE_NAME`_RANGE_1V       0x00u
#define `$INSTANCE_NAME`_RANGE_4V       0x04u


/* Power setting for Start API  */
#define `$INSTANCE_NAME`_LOWSPEED       0x00u
#define `$INSTANCE_NAME`_HIGHSPEED      0x02u


/***************************************
*  Initialization Parameter Constants
***************************************/

 /* Default DAC range */
#define `$INSTANCE_NAME`_DEFAULT_RANGE    `$VDAC_Range`
 /* Default DAC speed */
#define `$INSTANCE_NAME`_DEFAULT_SPEED    `$VDAC_Speed`
 /* Default Control */
#define `$INSTANCE_NAME`_DEFAULT_CNTL      0x00u
/* Default Strobe mode */
#define `$INSTANCE_NAME`_DEFAULT_STRB     `$Strobe_Mode`
 /* Initial DAC value */
#define `$INSTANCE_NAME`_DEFAULT_DATA     `$Initial_Value`
 /* Default Data Source */
#define `$INSTANCE_NAME`_DEFAULT_DATA_SRC `$Data_Source`


/***************************************
*              Registers        
***************************************/

#define `$INSTANCE_NAME`_CR0         (* (reg8 *) `$INSTANCE_NAME`_viDAC8__CR0 )
#define `$INSTANCE_NAME`_CR1         (* (reg8 *) `$INSTANCE_NAME`_viDAC8__CR1 )
#define `$INSTANCE_NAME`_Data        (* (reg8 *) `$INSTANCE_NAME`_viDAC8__D )
#define `$INSTANCE_NAME`_Data_PTR    (  (reg8 *) `$INSTANCE_NAME`_viDAC8__D )
#define `$INSTANCE_NAME`_Strobe      (* (reg8 *) `$INSTANCE_NAME`_viDAC8__STROBE )
#define `$INSTANCE_NAME`_SW0         (* (reg8 *) `$INSTANCE_NAME`_viDAC8__SW0 )
#define `$INSTANCE_NAME`_SW2         (* (reg8 *) `$INSTANCE_NAME`_viDAC8__SW2 )
#define `$INSTANCE_NAME`_SW3         (* (reg8 *) `$INSTANCE_NAME`_viDAC8__SW3 )
#define `$INSTANCE_NAME`_SW4         (* (reg8 *) `$INSTANCE_NAME`_viDAC8__SW4 )
#define `$INSTANCE_NAME`_TR          (* (reg8 *) `$INSTANCE_NAME`_viDAC8__TR )
/* Power manager */
#define `$INSTANCE_NAME`_PWRMGR      (* (reg8 *) `$INSTANCE_NAME`_viDAC8__PM_ACT_CFG )
  /* Standby Power manager */
#define `$INSTANCE_NAME`_STBY_PWRMGR (* (reg8 *) `$INSTANCE_NAME`_viDAC8__PM_STBY_CFG )


/***************************************
*         Register Constants       
***************************************/

/* CR0 vDac Control Register 0 definitions */

/* Bit Field  DAC_HS_MODE                  */
#define `$INSTANCE_NAME`_HS_MASK        0x02u
#define `$INSTANCE_NAME`_HS_LOWPOWER    0x00u
#define `$INSTANCE_NAME`_HS_HIGHSPEED   0x02u

/* Bit Field  DAC_MODE                  */
#define `$INSTANCE_NAME`_MODE_MASK      0x10u
#define `$INSTANCE_NAME`_MODE_V         0x00u
#define `$INSTANCE_NAME`_MODE_I         0x10u

/* Bit Field  DAC_RANGE                  */
#define `$INSTANCE_NAME`_RANGE_MASK     0x0Cu
#define `$INSTANCE_NAME`_RANGE_0        0x00u
#define `$INSTANCE_NAME`_RANGE_1        0x04u

/* CR1 iDac Control Register 1 definitions */

/* Bit Field  DAC_MX_DATA                  */
#define `$INSTANCE_NAME`_SRC_MASK       0x20u
#define `$INSTANCE_NAME`_SRC_REG        0x00u
#define `$INSTANCE_NAME`_SRC_UDB        0x20u

/* This bit enable reset from UDB array      */
#define `$INSTANCE_NAME`_RESET_MASK     0x10u
#define `$INSTANCE_NAME`_RESET_ENABLE   0x10u
#define `$INSTANCE_NAME`_RESET_DISABLE  0x00u

/* This bit enables data from DAC bus      */
#define `$INSTANCE_NAME`_DACBUS_MASK     0x20u
#define `$INSTANCE_NAME`_DACBUS_ENABLE   0x20u
#define `$INSTANCE_NAME`_DACBUS_DISABLE  0x00u

/* DAC STROBE Strobe Control Register definitions */

/* Bit Field  DAC_MX_STROBE                  */
#define `$INSTANCE_NAME`_STRB_MASK     0x08u
#define `$INSTANCE_NAME`_STRB_EN       0x08u
#define `$INSTANCE_NAME`_STRB_DIS      0x00u

/* PM_ACT_CFG (Active Power Mode CFG Register)     */ 
#define `$INSTANCE_NAME`_ACT_PWR_EN   `$INSTANCE_NAME`_viDAC8__PM_ACT_MSK
  /* Standby Power enable mask */
#define `$INSTANCE_NAME`_STBY_PWR_EN  `$INSTANCE_NAME`_viDAC8__PM_STBY_MSK


/*******************************************************************************
*              Trim    
* Note - VDAC trim values are stored in the "Customer Table" area in * Row 1 of
*the Hidden Flash.  There are 8 bytes of trim data for each VDAC block.
* The values are:
*       I Gain offset, min range, Sourcing
*       I Gain offset, min range, Sinking
*       I Gain offset, med range, Sourcing
*       I Gain offset, med range, Sinking
*       I Gain offset, max range, Sourcing
*       I Gain offset, max range, Sinking
*       V Gain offset, 1V range
*       V Gain offset, 4V range
*
* The data set for the 4 VDACs are arranged using a left side/right side
* approach:
*   Left 0, Left 1, Right 0, Right 1.
* When mapped to the VDAC0 thru VDAC3 as:
*   VDAC 0, VDAC 2, VDAC 1, VDAC 3
*******************************************************************************/
#define `$INSTANCE_NAME`_TRIM_M7_1V_RNG_OFFSET  0x06u
#define `$INSTANCE_NAME`_TRIM_M8_4V_RNG_OFFSET  0x07u
/*Constatnt to set DAC in current mode and turnoff output */
#define `$INSTANCE_NAME`_CUR_MODE_OUT_OFF       0x1Eu 
#define `$INSTANCE_NAME`_DAC_TRIM_BASE     (`$INSTANCE_NAME`_viDAC8__TRIM__M1)

#endif /* CY_VDAC8_`$INSTANCE_NAME`_H  */


/* [] END OF FILE */


