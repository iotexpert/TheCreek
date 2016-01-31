/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*    This file contains the function prototypes and constants used in
*    the 8-bit Current DAC (IDAC8) User Module.
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#include "cytypes.h"
#include "cyfitter.h"

#if !defined(CY_IDAC8_`$INSTANCE_NAME`_H)  
#define CY_IDAC8_`$INSTANCE_NAME`_H

#define `$INSTANCE_NAME`_FIRST_SILICON 


/***************************************
*        Function Prototypes 
***************************************/

void    `$INSTANCE_NAME`_Start(void);
void    `$INSTANCE_NAME`_Stop(void);
void    `$INSTANCE_NAME`_SetSpeed(uint8 speed);
void    `$INSTANCE_NAME`_SetPolarity(uint8 polarity);
void    `$INSTANCE_NAME`_SetRange(uint8 iRange);
void    `$INSTANCE_NAME`_SetValue(uint8 value);
void    `$INSTANCE_NAME`_DacTrim(void);
  

/***************************************
*       Initialization Values
***************************************/

#define `$INSTANCE_NAME`_DEFAULT_RANGE    `$IDAC_Range`      /* Default DAC range */
#define `$INSTANCE_NAME`_DEFAULT_SPEED    ((`$IDAC_Speed` ? 1:0)*2)  /* Default DAC speed */
#define `$INSTANCE_NAME`_DEFAULT_CNTL     0x00u             /* Default Control */
#define `$INSTANCE_NAME`_DEFAULT_STRB     `$Strobe_Mode`    /* Default Strobe mode */
#define `$INSTANCE_NAME`_DEFAULT_DATA     `$Initial_Value`  /* Initial DAC value */
#define `$INSTANCE_NAME`_DEFAULT_POLARITY `$Polarity`       /* Default Sink or Source */
#define `$INSTANCE_NAME`_DEFAULT_DATA_SRC `$Data_Source`    /* Default Data Source */

/***************************************
*              Constants        
***************************************/

/* SetRange constants */
`#cy_declare_enum iDacRange`
#define `$INSTANCE_NAME`_RANGE_32uA     0x00u
#define `$INSTANCE_NAME`_RANGE_255uA    0x04u
#define `$INSTANCE_NAME`_RANGE_2mA      0x08u

/* SetPolarity constants */
#define `$INSTANCE_NAME`_SOURCE         0x00u
#define `$INSTANCE_NAME`_SINK           0x04u

/* Power setting for SetSpeed API  */
#define `$INSTANCE_NAME`_LOWSPEED       0x00u
#define `$INSTANCE_NAME`_HIGHSPEED      0x02u


/* CR0 iDAC Control Register 0 definitions */  

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
#define `$INSTANCE_NAME`_RANGE_2        0x08u
#define `$INSTANCE_NAME`_RANGE_3        0x0Cu

/* CR1 iDAC Control Register 1 definitions */

/* Bit Field  DAC_MX_DATA                  */
#define `$INSTANCE_NAME`_SRC_MASK       0x20u
#define `$INSTANCE_NAME`_SRC_REG        0x00u
#define `$INSTANCE_NAME`_SRC_UDB        0x20u

/* This bit enable reset from UDB array      */
#define `$INSTANCE_NAME`_RESET_MASK     0x10u
#define `$INSTANCE_NAME`_RESET_ENABLE   0x10u
#define `$INSTANCE_NAME`_RESET_DISABLE  0x00u

/* Bit Field  DAC_MX_IDIR_SRC              */
#define `$INSTANCE_NAME`_IDIR_SRC_MASK  0x08u
#define `$INSTANCE_NAME`_IDIR_SRC_REG   0x00u
#define `$INSTANCE_NAME`_IDIR_SRC_UDB   0x08u

/* Bit Field  DAC_I_DIR                  */
/* Register control of current direction      */
#define `$INSTANCE_NAME`_IDIR_MASK      0x04u   
#define `$INSTANCE_NAME`_IDIR_SRC       0x00u
#define `$INSTANCE_NAME`_IDIR_SINK      0x04u

/* Bit Field  DAC_MX_IOFF_SRC                  */
/* Selects source of IOFF control, reg or UDB  */
#define `$INSTANCE_NAME`_IDIR_CTL_MASK  0x02u
#define `$INSTANCE_NAME`_IDIR_CTL_REG   0x00u
#define `$INSTANCE_NAME`_IDIR_CTL_UDB   0x02u

/* Bit Field  DAC_MX_IOFF                   */
/* Register control of IDAC                 */
/* Only valid if IOFF CTL is set to Reg     */
#define `$INSTANCE_NAME`_I_OFF_MASK     0x01u   
#define `$INSTANCE_NAME`_I_OFF          0x00u
#define `$INSTANCE_NAME`_I_ON           0x01u

/* This bit enables data from DAC bus      */
#define `$INSTANCE_NAME`_DACBUS_MASK     0x20u
#define `$INSTANCE_NAME`_DACBUS_ENABLE   0x20u
#define `$INSTANCE_NAME`_DACBUS_DISABLE  0x00u

/* DAC STROBE Strobe Control Register definitions */

/* Bit Field  DAC_MX_STROBE                  */
#define `$INSTANCE_NAME`_STRB_MASK      0x08u
#define `$INSTANCE_NAME`_STRB_EN        0x08u
#define `$INSTANCE_NAME`_STRB_DIS       0x00u

/* PM_ACT_CFG (Active Power Mode CFG Register)     */ 
#if !defined(`$INSTANCE_NAME`_FIRST_SILICON)
#define `$INSTANCE_NAME`_ACT_PWR_EN   `$INSTANCE_NAME`_viDAC8__PM_ACT_MSK /* Power enable mask */ 
#else
#define `$INSTANCE_NAME`_ACT_PWR_EN   0xFF /* TODO: Work around for incorrect power enable */
#endif

/***************************************
*              Registers        
***************************************/

#define `$INSTANCE_NAME`_CR0    (* (reg8 *) `$INSTANCE_NAME`_viDAC8__CR0 )
#define `$INSTANCE_NAME`_CR1    (* (reg8 *) `$INSTANCE_NAME`_viDAC8__CR1 )
#define `$INSTANCE_NAME`_Data   (* (reg8 *) `$INSTANCE_NAME`_viDAC8__D )
#define `$INSTANCE_NAME`_Strobe (* (reg8 *) `$INSTANCE_NAME`_viDAC8__STROBE )
#define `$INSTANCE_NAME`_SW0    (* (reg8 *) `$INSTANCE_NAME`_viDAC8__SW0 )
#define `$INSTANCE_NAME`_SW2    (* (reg8 *) `$INSTANCE_NAME`_viDAC8__SW2 )
#define `$INSTANCE_NAME`_SW3    (* (reg8 *) `$INSTANCE_NAME`_viDAC8__SW3 )
#define `$INSTANCE_NAME`_SW4    (* (reg8 *) `$INSTANCE_NAME`_viDAC8__SW4 )
#define `$INSTANCE_NAME`_TR     (* (reg8 *) `$INSTANCE_NAME`_viDAC8__TR )
#define `$INSTANCE_NAME`_PWRMGR (* (reg8 *) `$INSTANCE_NAME`_viDAC8__PM_ACT_CFG )  /* Power manager */

/***************************************
*              Trim    
*
* Note - VIDAC trim values are stored in the "Customer Table" area in 
* Row 1 of the Hidden Flash.  There are 8 bytes of trim data for each VIDAC block.
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
* The data set for the 4 VIDACs are arranged using a left side/right side approach:
*   Left 0, Left 1, Right 0, Right 1.
* When mapped to the VIDAC0 thru VIDAC3 as:
*   VIDAC 0, VIDAC 2, VIDAC 1, VIDAC 3
***************************************/

#define `$INSTANCE_NAME`_DAC_POS  (`$INSTANCE_NAME`_viDAC8__D - CYDEV_ANAIF_WRK_DAC0_BASE)

#if(`$INSTANCE_NAME`_DAC_POS == 0)
#define `$INSTANCE_NAME`_DAC_TRIM_BASE  0x0C011Cu
#endif

#if(`$INSTANCE_NAME`_DAC_POS == 1)
#define `$INSTANCE_NAME`_DAC_TRIM_BASE  0x0C012Cu
#endif

#if(`$INSTANCE_NAME`_DAC_POS == 2)
#define `$INSTANCE_NAME`_DAC_TRIM_BASE  0x0C0124u
#endif

#if(`$INSTANCE_NAME`_DAC_POS == 3)
#define `$INSTANCE_NAME`_DAC_TRIM_BASE  0x0C0134u
#endif

#endif /* CY_IDAC8_`$INSTANCE_NAME`_H  */
/*  [] END OF FILE  */
