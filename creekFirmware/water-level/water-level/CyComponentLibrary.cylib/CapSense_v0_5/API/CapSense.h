/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h  
* Version `$VERSION_MAJOR`.`$VERSION_MINOR`
*
*  Description:
*     Contains the function prototypes and constants available to the RTC
*     Component.
*
*   Note:
*     None
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#if !defined(CY_CAPSENSE_`$INSTANCE_NAME`_H)
#define CY_CAPSENSE_`$INSTANCE_NAME`_H
#include "cytypes.h"
#include "cyfitter.h"
#include "cydevice.h"
`$IncludeLeft`
`$IncludeRight`
/***************************************
 *  Types definition
 ***************************************/      
/*------- SCAN SLOT -------------------------------------------------------*/
typedef struct _`$INSTANCE_NAME`_ScanSlot
{
	uint8 RawIndex;		/* Entry in SlotResult */
	uint8 IndexOffset;	/* Offset in IndezTable */
	uint8 SnsCnt; 		/* Number of Sensors in currect Slot */
	uint8 WidgetNumber; /* Number of Widget this slot belongs */
	uint8 DebounceCount;/* Helps to define if slot Active */
} `$INSTANCE_NAME`_ScanSlot;

/*------- SETTINGS -------------------------------------------------------*/
`$StructSettingsPrototypes`

/*------- PORT MASK -------------------------------------------------------*/
typedef struct _`$INSTANCE_NAME`_PortMask
{
	uint8 port;
	uint8 mask;
} `$INSTANCE_NAME`_PortMask;

/***************************************
 *  Function Prototypes
 ***************************************/
`$ParalelFunctionPrototypes`

`$FunctionPrototypesLeft`
`$FunctionPrototypesRight`

/***************************************
*  Constants
***************************************/
/* Scan Speed Type */
#define `$INSTANCE_NAME`_SCAN_SPEED_ULTRA_FAST			0x01u
#define `$INSTANCE_NAME`_SCAN_SPEED_FAST				0x03u
#define `$INSTANCE_NAME`_SCAN_SPEED_NORMAL				0x07u
#define `$INSTANCE_NAME`_SCAN_SPEED_SLOW				0x0Fu

/* Idac SetRange constants */
#define `$INSTANCE_NAME`_IDAC_RANGE_MASK			    0x0Cu
#define `$INSTANCE_NAME`_IDAC_RANGE_32uA     			0x00u
#define `$INSTANCE_NAME`_IDAC_RANGE_255uA    			0x04u
#define `$INSTANCE_NAME`_IDAC_RANGE_2mA      			0x08u

/* PWM Resolution */
#define `$INSTANCE_NAME`_PWM_RESOLUTION_8_BITS		    0x00u
#define `$INSTANCE_NAME`_PWM_RESOLUTION_9_BITS		    0x01u
#define `$INSTANCE_NAME`_PWM_RESOLUTION_10_BITS		    0x03u
#define `$INSTANCE_NAME`_PWM_RESOLUTION_11_BITS		    0x07u
#define `$INSTANCE_NAME`_PWM_RESOLUTION_12_BITS		    0x0Fu
#define `$INSTANCE_NAME`_PWM_RESOLUTION_13_BITS		    0x1Fu
#define `$INSTANCE_NAME`_PWM_RESOLUTION_14_BITS		    0x3Fu
#define `$INSTANCE_NAME`_PWM_RESOLUTION_15_BITS		    0x7Fu
#define `$INSTANCE_NAME`_PWM_RESOLUTION_16_BITS		    0xFFu

/* Starts CapSensing */
#define `$INSTANCE_NAME`_CSBUF_CONNECT			  		0x01u

/* Enable csBuffer */
#define `$INSTANCE_NAME`_START_CAPSENSING				0x01u
#define `$INSTANCE_NAME`_RESET_PWM_CNTR					0x02u

/* Rbleed */
#define `$INSTANCE_NAME`_MAX_RB_NUMBER      3
#define `$INSTANCE_NAME`_RBLEED1			0
#define `$INSTANCE_NAME`_RBLEED2			1
#define `$INSTANCE_NAME`_RBLEED3			2

/* Register to work with port */
#define `$INSTANCE_NAME`_BASE_PRT_DM0			( (reg8 *) CYDEV_IO_PRT_PRT0_DM0 )
#define `$INSTANCE_NAME`_BASE_PRT_DM1			( (reg8 *) CYDEV_IO_PRT_PRT0_DM1 )
#define `$INSTANCE_NAME`_BASE_PRT_DM2			( (reg8 *) CYDEV_IO_PRT_PRT0_DM2 )
#define `$INSTANCE_NAME`_BASE_PRT_AMUX			( (reg8 *) CYDEV_IO_PRT_PRT0_AMUX )
#define `$INSTANCE_NAME`_BASE_PRT_BYP			( (reg8 *) CYDEV_IO_PRT_PRT0_BYP )
#define `$INSTANCE_NAME`_BASE_PRT_DR			( (reg8 *) CYDEV_IO_PRT_PRT0_DR )
#define `$INSTANCE_NAME`_BASE_PRTDSI_CAPS		( (reg8 *) CYDEV_PRTDSI_PRT0_CAPS_SEL )

#define `$INSTANCE_NAME`_PRT_OFFSET				CYDEV_IO_PRT_PRT0_SIZE
#define `$INSTANCE_NAME`_PRTDSI_OFFSET			CYDEV_PRTDSI_PRT0_SIZE

/* Mask to work with all Port */
#define `$INSTANCE_NAME`_PRT_BYP_ENABLE			0x80
#define `$INSTANCE_NAME`_PRT_SET_TO_HIGH    	0x01
#define max(a,b)           						(( a > b ) ?  a : b )

`$DefineLeft`
`$DefineRight`
`$Define`
`$DefineIdacEnable`

#endif /* CY_CAPSENSE_`$INSTANCE_NAME`_H */