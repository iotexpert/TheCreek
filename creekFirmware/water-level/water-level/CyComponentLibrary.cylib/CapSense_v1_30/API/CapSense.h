/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides constants and parameter values for the CapSense
*  Component.
*
* Note:
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(CY_CAPSENSE_`$INSTANCE_NAME`_H)
#define CY_CAPSENSE_`$INSTANCE_NAME`_H

#include "cytypes.h"
#include "cyfitter.h"
#include "cydevice_trm.h"
#include "CyLib.h"
`$IncludeLeft``$IncludeRight`

/***************************************
*       Types definition
***************************************/

/*       Scan Slot struct             */
typedef struct _`$INSTANCE_NAME`_Slot
{
    uint8 RawIndex;         /* Entry in SlotResult */
    uint8 IndexOffset;      /* Offset in IndezTable */
    uint8 SnsCnt;           /* Number of Sensors in currect Slot */
    uint8 WidgetNumber;     /* Number of Widget this slot belongs */
    uint8 DebounceCount;    /* Helps to define if slot Active */
} `$INSTANCE_NAME`_Slot;
`$StructSettingsPrototypes`
/*       Port Shift struct            */
typedef struct _`$INSTANCE_NAME`_PortShift
{
    uint8 port;
    uint8 shift;
} `$INSTANCE_NAME`_PortShift;


/***************************************
*        Function Prototypes
***************************************/

`$ParalelFunctionPrototypes``$FunctionPrototypesLeft``$FunctionPrototypesRight`

/***************************************
*           API Constants
***************************************/
`$DefineTotal``$DefineLeft``$DefineRight`
/* Scan Speed Type */
#define `$INSTANCE_NAME`_SCAN_SPEED_ULTRA_FAST      0x01u
#define `$INSTANCE_NAME`_SCAN_SPEED_FAST            0x03u
#define `$INSTANCE_NAME`_SCAN_SPEED_NORMAL          0x07u
#define `$INSTANCE_NAME`_SCAN_SPEED_SLOW            0x0Fu

/* Idac SetRange */
#define `$INSTANCE_NAME`_IDAC_RANGE_MASK            0x0Cu
#define `$INSTANCE_NAME`_IDAC_RANGE_32uA            0x00u
#define `$INSTANCE_NAME`_IDAC_RANGE_255uA           0x04u
#define `$INSTANCE_NAME`_IDAC_RANGE_2mA             0x08u

/* PWM Resolution */
#define `$INSTANCE_NAME`_PWM_WINDOW_SHIFT           8u
#define `$INSTANCE_NAME`_PWM_RESOLUTION_8_BITS      0x00u
#define `$INSTANCE_NAME`_PWM_RESOLUTION_9_BITS      0x01u
#define `$INSTANCE_NAME`_PWM_RESOLUTION_10_BITS     0x03u
#define `$INSTANCE_NAME`_PWM_RESOLUTION_11_BITS     0x07u
#define `$INSTANCE_NAME`_PWM_RESOLUTION_12_BITS     0x0Fu
#define `$INSTANCE_NAME`_PWM_RESOLUTION_13_BITS     0x1Fu
#define `$INSTANCE_NAME`_PWM_RESOLUTION_14_BITS     0x3Fu
#define `$INSTANCE_NAME`_PWM_RESOLUTION_15_BITS     0x7Fu
#define `$INSTANCE_NAME`_PWM_RESOLUTION_16_BITS     0xFFu

/* Enable csBuffer */
#define `$INSTANCE_NAME`_CSBUF_BOOST_ENABLE         0x02u
#define `$INSTANCE_NAME`_CSBUF_ENABLE               0x01u

/* Starts CapSensing */
#define `$INSTANCE_NAME`_START_CAPSENSING           0x01u

/* Resets PWM and Raw Counter */
#define `$INSTANCE_NAME`_RESET_PWM_CNTR             0x02u

/* Overfow of counter while CSA */
#define `$INSTANCE_NAME`_RAW_OVERFLOW               0x04u

/* Turn off IDAC  */
#define `$INSTANCE_NAME`_TURN_OFF_IDAC              0x00u

/* Rbleed */
#define `$INSTANCE_NAME`_MAX_RB_NUMBER      3
#define `$INSTANCE_NAME`_RBLEED1            0
#define `$INSTANCE_NAME`_RBLEED2            1
#define `$INSTANCE_NAME`_RBLEED3            2

/* Diasable States */
#define `$INSTANCE_NAME`_DISABLE_STATE_GND          0
#define `$INSTANCE_NAME`_DISABLE_STATE_HIGHZ        1
#define `$INSTANCE_NAME`_DISABLE_STATE_SHIELD       2
#define `$INSTANCE_NAME`_ALONE_SENSOR               1


/***************************************
*             Registers
***************************************/
`$DefineRegsLeft``$DefineRegsRight`
/* Register to work with port */
#define `$INSTANCE_NAME`_BASE_PRT_PC            ( (reg8 *) CYDEV_IO_PC_PRT0_BASE )
#if((CYDEV_CHIP_DIE_EXPECT == CYDEV_CHIP_DIE_LEOPARD) && (CYDEV_CHIP_REV_EXPECT == CYDEV_CHIP_REV_LEOPARD_ES1))
    #define `$INSTANCE_NAME`_BASE_PRTDSI_CAPS   ( (reg8 *) CYREG_PRTDSI_PRT0_CAPS_SEL )
#else
    #define `$INSTANCE_NAME`_BASE_PRTDSI_CAPS   ( (reg8 *) CYREG_PRT0_CAPS_SEL )
#endif


/***************************************
*       Register Constants
***************************************/

/* PC and CAPS_SEL Registers Offset */
#define `$INSTANCE_NAME`_PRT_PC_OFFSET      CYDEV_IO_PC_PRT0_SIZE
#define `$INSTANCE_NAME`_PRTDSI_OFFSET      (CYDEV_PRTDSI_PRT0_SIZE+1)

/* Masks of PTR PC Register */
#define `$INSTANCE_NAME`_DR_MASK            0x01
#define `$INSTANCE_NAME`_DM0_MASK           0x02
#define `$INSTANCE_NAME`_DM1_MASK           0x04
#define `$INSTANCE_NAME`_DM2_MASK           0x08
#define `$INSTANCE_NAME`_BYP_MASK           0x80

#define `$INSTANCE_NAME`_PRT_PC_GND         `$INSTANCE_NAME`_DM2_MASK
#define `$INSTANCE_NAME`_PRT_PC_HIGHZ       (`$INSTANCE_NAME`_DM2_MASK |`$INSTANCE_NAME`_DR_MASK)
#define `$INSTANCE_NAME`_PRT_PC_SHIELD      (`$INSTANCE_NAME`_DM2_MASK | `$INSTANCE_NAME`_DM1_MASK | `$INSTANCE_NAME`_BYP_MASK)
`$DefineIdacEnable`
#endif /* End CY_CAPSENSE_`$INSTANCE_NAME`_H */


 /* [] END OF FILE */
