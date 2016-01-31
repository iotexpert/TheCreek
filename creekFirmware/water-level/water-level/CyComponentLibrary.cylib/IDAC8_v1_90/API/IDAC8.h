/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file contains the function prototypes and constants used in
*  the 8-bit Current DAC (IDAC8) User Module.
*
* Note:
*  None
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(CY_IDAC8_`$INSTANCE_NAME`_H)  
#define CY_IDAC8_`$INSTANCE_NAME`_H

#include "cytypes.h"
#include "cyfitter.h"

/* Check to see if required defines such as CY_PSOC5LP are available */
/* They are defined starting with cy_boot v3.0 */
#if !defined (CY_PSOC5LP)
    #error Component `$CY_COMPONENT_NAME` requires cy_boot v3.0 or later
#endif /* (CY_PSOC5LP) */


/***************************************
*       Type defines
***************************************/

/* Sleep Mode API Support */
typedef struct `$INSTANCE_NAME`_backupStruct
{
    uint8 enableState;
    uint8 data_value;
}`$INSTANCE_NAME`_backupStruct;


#if (CY_PSOC5A)
    /* Stop API changes for PSoC5A */
    typedef struct _`$INSTANCE_NAME`_lowPowerBackupStruct
    {
        uint8 DACCR0Reg;
    }   `$INSTANCE_NAME`_LOWPOWER_BACKUP_STRUCT;
#endif /* CY_PSOC5A */


/***************************************
*        Function Prototypes 
***************************************/

void    `$INSTANCE_NAME`_Start(void)                 `=ReentrantKeil($INSTANCE_NAME . "_Start")`;
void    `$INSTANCE_NAME`_Stop(void)                  `=ReentrantKeil($INSTANCE_NAME . "_Stop")`;
void    `$INSTANCE_NAME`_SetSpeed(uint8 speed)       `=ReentrantKeil($INSTANCE_NAME . "_SetSpeed")`;
void    `$INSTANCE_NAME`_SetPolarity(uint8 polarity) `=ReentrantKeil($INSTANCE_NAME . "_SetPolarity")`;
void    `$INSTANCE_NAME`_SetRange(uint8 range)      `=ReentrantKeil($INSTANCE_NAME . "_SetRange")`;
void    `$INSTANCE_NAME`_SetValue(uint8 value)       `=ReentrantKeil($INSTANCE_NAME . "_SetValue")`;
void    `$INSTANCE_NAME`_DacTrim(void)               `=ReentrantKeil($INSTANCE_NAME . "_DacTrim")`;

/* Sleep Retention APIs */
void `$INSTANCE_NAME`_Init(void)                     `=ReentrantKeil($INSTANCE_NAME . "_Init")`;
void `$INSTANCE_NAME`_Enable(void)                   `=ReentrantKeil($INSTANCE_NAME . "_Enable")`;
void `$INSTANCE_NAME`_SaveConfig(void)               `=ReentrantKeil($INSTANCE_NAME . "_SaveConfig")`;
void `$INSTANCE_NAME`_RestoreConfig(void)            `=ReentrantKeil($INSTANCE_NAME . "_RestoreConfig")`;
void `$INSTANCE_NAME`_Sleep(void)                    `=ReentrantKeil($INSTANCE_NAME . "_Sleep")`;
void `$INSTANCE_NAME`_Wakeup(void)                   `=ReentrantKeil($INSTANCE_NAME . "_Wakeup")`;


/***************************************
*       Paramater Initial Values
***************************************/

#define `$INSTANCE_NAME`_DEFAULT_RANGE     `$IDAC_Range`u      /* Default DAC range */
#define `$INSTANCE_NAME`_DEFAULT_SPEED     ((`$IDAC_Speed`u ? 1u:0u)*2)  /* Default DAC speed */
#define `$INSTANCE_NAME`_DEFAULT_CNTL      0x00u             /* Default Control */
#define `$INSTANCE_NAME`_DEFAULT_STRB     `$Strobe_Mode`u    /* Default Strobe mode */
#define `$INSTANCE_NAME`_DEFAULT_DATA     `$Initial_Value`u          /* Initial DAC value */
#define `$INSTANCE_NAME`_DEFAULT_POLARITY `$Polarity`u       /* Default Sink or Source */
#define `$INSTANCE_NAME`_DEFAULT_DATA_SRC `$Data_Source`u    /* Default Data Source */   
#define `$INSTANCE_NAME`_HARDWARE_ENABLE  `$Hardware_Enable`u /*Hardware Enable */


/***************************************
*              API Constants        
***************************************/

/* SetRange constants */
`#cy_declare_enum iDacRange`
#define `$INSTANCE_NAME`_RANGE_32uA             (0x00u)
#define `$INSTANCE_NAME`_RANGE_255uA            (0x04u)
#define `$INSTANCE_NAME`_RANGE_2mA              (0x08u)

/* SetPolarity constants */
#define `$INSTANCE_NAME`_SOURCE                 (0x00u)
#define `$INSTANCE_NAME`_SINK                   (0x04u)
#define `$INSTANCE_NAME`_HARDWARE_CONTROLLED    (0x02u)

/* Power setting for SetSpeed API  */
#define `$INSTANCE_NAME`_LOWSPEED               (0x00u)
#define `$INSTANCE_NAME`_HIGHSPEED              (0x02u)


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
#define `$INSTANCE_NAME`_PWRMGR      (* (reg8 *) `$INSTANCE_NAME`_viDAC8__PM_ACT_CFG )  /* Power manager */
#define `$INSTANCE_NAME`_STBY_PWRMGR (* (reg8 *) `$INSTANCE_NAME`_viDAC8__PM_STBY_CFG )  /* Standby Power manager */


/******************************************************************************
*              Trim    
*
* Note - VIDAC trim values are stored in the "Customer Table" area in 
* Row 1 of the Hidden Flash.  There are 8 bytes of trim data for each VIDAC 
* block.
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
* The data set for the 4 VIDACs are arranged using a left side/right 
* side approach:
*  Left 0, Left 1, Right 0, Right 1.
* When mapped to the VIDAC0 thru VIDAC3 as:
*  VIDAC 0, VIDAC 2, VIDAC 1, VIDAC 3
******************************************************************************/

#define `$INSTANCE_NAME`_DAC_TRIM_BASE   `$INSTANCE_NAME`_viDAC8__TRIM__M1


/***************************************
*         Register Constants       
***************************************/

/* CR0 iDAC Control Register 0 definitions */  

/* Bit Field  DAC_HS_MODE                  */
#define `$INSTANCE_NAME`_HS_MASK        (0x02u)
#define `$INSTANCE_NAME`_HS_LOWPOWER    (0x00u)
#define `$INSTANCE_NAME`_HS_HIGHSPEED   (0x02u)

/* Bit Field  DAC_MODE                  */
#define `$INSTANCE_NAME`_MODE_MASK      (0x10u)
#define `$INSTANCE_NAME`_MODE_V         (0x00u)
#define `$INSTANCE_NAME`_MODE_I         (0x10u)

/* Bit Field  DAC_RANGE                  */
#define `$INSTANCE_NAME`_RANGE_MASK     (0x0Cu)
#define `$INSTANCE_NAME`_RANGE_0        (0x00u)
#define `$INSTANCE_NAME`_RANGE_1        (0x04u)
#define `$INSTANCE_NAME`_RANGE_2        (0x08u)
#define `$INSTANCE_NAME`_RANGE_3        (0x0Cu)

/* CR1 iDAC Control Register 1 definitions */

/* Bit Field  DAC_MX_DATA                  */
#define `$INSTANCE_NAME`_SRC_MASK       (0x20u)
#define `$INSTANCE_NAME`_SRC_REG        (0x00u)
#define `$INSTANCE_NAME`_SRC_UDB        (0x20u)

/* This bit enable reset from UDB array      */
#define `$INSTANCE_NAME`_RESET_MASK     (0x10u)
#define `$INSTANCE_NAME`_RESET_ENABLE   (0x10u)
#define `$INSTANCE_NAME`_RESET_DISABLE  (0x00u)

/* Bit Field  DAC_MX_IDIR_SRC              */
#define `$INSTANCE_NAME`_IDIR_SRC_MASK  (0x08u)
#define `$INSTANCE_NAME`_IDIR_SRC_REG   (0x00u)
#define `$INSTANCE_NAME`_IDIR_SRC_UDB   (0x08u)

/* Bit Field  DAC_I_DIR                  */
/* Register control of current direction      */
#define `$INSTANCE_NAME`_IDIR_MASK      (0x04u)
#define `$INSTANCE_NAME`_IDIR_SRC       (0x00u)
#define `$INSTANCE_NAME`_IDIR_SINK      (0x04u)

/* Bit Field  DAC_MX_IOFF_SRC                  */
/* Selects source of IOFF control, reg or UDB  */
#define `$INSTANCE_NAME`_IDIR_CTL_MASK  (0x02u)
#define `$INSTANCE_NAME`_IDIR_CTL_REG   (0x00u)
#define `$INSTANCE_NAME`_IDIR_CTL_UDB   (0x02u)

/* Bit Field  DAC_MX_IOFF                   */
/* Register control of IDAC                 */
/* Only valid if IOFF CTL is set to Reg     */
#define `$INSTANCE_NAME`_I_OFF_MASK     (0x01u)
#define `$INSTANCE_NAME`_I_OFF          (0x00u)
#define `$INSTANCE_NAME`_I_ON           (0x01u)

/* This bit enables data from DAC bus      */
#define `$INSTANCE_NAME`_DACBUS_MASK    (0x20u)
#define `$INSTANCE_NAME`_DACBUS_ENABLE  (0x20u)
#define `$INSTANCE_NAME`_DACBUS_DISABLE (0x00u)

/* DAC STROBE Strobe Control Register definitions */

/* Bit Field  DAC_MX_STROBE                  */
#define `$INSTANCE_NAME`_STRB_MASK      (0x08u)
#define `$INSTANCE_NAME`_STRB_EN        (0x08u)
#define `$INSTANCE_NAME`_STRB_DIS       (0x00u)

/* PM_ACT_CFG (Active Power Mode CFG Register)     */ 
#define `$INSTANCE_NAME`_ACT_PWR_EN   `$INSTANCE_NAME`_viDAC8__PM_ACT_MSK  /* Power enable mask */ 
#define `$INSTANCE_NAME`_STBY_PWR_EN  `$INSTANCE_NAME`_viDAC8__PM_STBY_MSK  /* Standby Power enable mask */ 

#endif /* CY_IDAC8_`$INSTANCE_NAME`_H */


/* [] END OF FILE */
