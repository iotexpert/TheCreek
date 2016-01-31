/*******************************************************************************
* File Name: `$INSTANCE_NAME`_hid.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  Header File for the USFS component. Contains prototypes and constant values.
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(`$INSTANCE_NAME`_hid_H)
#define `$INSTANCE_NAME`_hid_H

#include "cytypes.h"


/***************************************
* Prototypes of the $INSTANCE_NAME` API.
***************************************/

uint8 `$INSTANCE_NAME`_UpdateHIDTimer(uint8 interface) `=ReentrantKeil($INSTANCE_NAME . "_UpdateHIDTimer")`;
uint8 `$INSTANCE_NAME`_GetProtocol(uint8 interface) `=ReentrantKeil($INSTANCE_NAME . "_GetProtocol")`;


/***************************************
*Renamed Functions for backward compatible
***************************************/

#define `$INSTANCE_NAME`_bGetProtocol               `$INSTANCE_NAME`_GetProtocol


/***************************************
*  Constants for $INSTANCE_NAME` API.
***************************************/

#define `$INSTANCE_NAME`_PROTOCOL_BOOT              (0x00u)
#define `$INSTANCE_NAME`_PROTOCOL_REPORT            (0x01u)

/* Request Types (HID Chapter 7.2) */
#define `$INSTANCE_NAME`_HID_GET_REPORT             (0x01u)
#define `$INSTANCE_NAME`_HID_GET_IDLE               (0x02u)
#define `$INSTANCE_NAME`_HID_GET_PROTOCOL           (0x03u)
#define `$INSTANCE_NAME`_HID_SET_REPORT             (0x09u)
#define `$INSTANCE_NAME`_HID_SET_IDLE               (0x0Au)
#define `$INSTANCE_NAME`_HID_SET_PROTOCOL           (0x0Bu)

/* Descriptor Types (HID Chapter 7.1) */
#define `$INSTANCE_NAME`_DESCR_HID_CLASS            (0x21u)
#define `$INSTANCE_NAME`_DESCR_HID_REPORT           (0x22u)
#define `$INSTANCE_NAME`_DESCR_HID_PHYSICAL         (0x23u)

/* Report Request Types (HID Chapter 7.2.1) */
#define `$INSTANCE_NAME`_HID_GET_REPORT_INPUT       (0x01u)
#define `$INSTANCE_NAME`_HID_GET_REPORT_OUTPUT      (0x02u)
#define `$INSTANCE_NAME`_HID_GET_REPORT_FEATURE     (0x03u)

#endif /* End `$INSTANCE_NAME`_hid_H */


/* [] END OF FILE */
