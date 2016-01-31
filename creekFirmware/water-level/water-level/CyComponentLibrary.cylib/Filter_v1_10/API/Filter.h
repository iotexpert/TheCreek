/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This header file contains definitions associated with the FILT component.
*
* Note:
* 
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#if !defined(`$INSTANCE_NAME`_H) /* FILT Header File */
#define `$INSTANCE_NAME`_H

#include "cytypes.h"
#include "cyfitter.h"

/*******************************************************************************
* FILT component function prototypes.
*******************************************************************************/

void   `$INSTANCE_NAME`_Start(void);
void   `$INSTANCE_NAME`_Stop(void);
uint8 `$INSTANCE_NAME`_Read8(uint8 channel);
uint16 `$INSTANCE_NAME`_Read16(uint8 channel);
uint32 `$INSTANCE_NAME`_Read24(uint8 channel);
void `$INSTANCE_NAME`_Write8(uint8 channel, uint8 sample);
void `$INSTANCE_NAME`_Write16(uint8 channel, uint16 sample);
void `$INSTANCE_NAME`_Write24(uint8 channel, uint32 sample);

/*******************************************************************************
* FILT component DFB registers.
*******************************************************************************/

#define `$INSTANCE_NAME`_SR             (* (reg8 *) `$INSTANCE_NAME`_DFB__SR)

/*******************************************************************************
* FILT component API Constants.
*******************************************************************************/

/* Channel Definitions */
#define `$INSTANCE_NAME`_CHANNEL_A      0u
#define `$INSTANCE_NAME`_CHANNEL_B      1u

#define `$INSTANCE_NAME`_CHANNEL_A_INTR 0x08u
#define `$INSTANCE_NAME`_CHANNEL_B_INTR 0x10u

#define `$INSTANCE_NAME`_ALL_INTR       0xf8u

#define `$INSTANCE_NAME`_SIGN_BIT       0x00800000u
#define `$INSTANCE_NAME`_SIGN_BYTE      0xFF000000u


/*******************************************************************************
* FILT component macros.
*******************************************************************************/

#define `$INSTANCE_NAME`_ClearInterruptSource() \
    do { \
    `$INSTANCE_NAME`_SR = `$INSTANCE_NAME`_ALL_INTR; \
    } while (0)

#define `$INSTANCE_NAME`_IsInterruptChannelA() \
    (`$INSTANCE_NAME`_SR & `$INSTANCE_NAME`_CHANNEL_A_INTR)

#define `$INSTANCE_NAME`_IsInterruptChannelB() \
    (`$INSTANCE_NAME`_SR & `$INSTANCE_NAME`_CHANNEL_B_INTR)

/*******************************************************************************
* FILT component internal macros.
*******************************************************************************/

// This is a workaround for CDT - 42885 Move CYCODE define into cytypes.h
// After the CDT is fixed, it should be removed.
#if !defined(CYCODE)
#if defined(__C51__) || defined(__CX51__)
#define CYCODE code
#else
#define CYCODE
#endif
#endif

#endif /* End FILT Header File */

/* [] END OF FILE */
