/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*    Header File for the USFS component. Contains prototypes and constant values. 
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/


#if !defined(`$INSTANCE_NAME`_hid_H)
#define `$INSTANCE_NAME`_hid_H

#include "cytypes.h"
/************************************************
 *  Prototypes of the `@INSTANCE_NAME` API. 
 ************************************************/
extern uint8 USBFS_UpdateHIDTimer(uint8 bInterface);
extern uint8 USBFS_bGetProtocol(uint8 bInterface);

/************************************************
 *  Constants for `@INSTANCE_NAME` API. 
 ************************************************/

#define `$INSTANCE_NAME`_IDLE_TIMER_RUNNING         0x02
#define `$INSTANCE_NAME`_IDLE_TIMER_EXPIRED         0x01
#define `$INSTANCE_NAME`_IDLE_TIMER_INDEFINITE      0x00

#define `$INSTANCE_NAME`_PROTOCOL_BOOT              0x00
#define `$INSTANCE_NAME`_PROTOCOL_REPORT            0x01

/************************************************
 *  Request Types (HID Chapter 7.2) 
 ************************************************/

#define `$INSTANCE_NAME`_HID_GET_REPORT       0x01
#define `$INSTANCE_NAME`_HID_GET_IDLE         0x02
#define `$INSTANCE_NAME`_HID_GET_PROTOCOL     0x03
#define `$INSTANCE_NAME`_HID_SET_REPORT       0x09
#define `$INSTANCE_NAME`_HID_SET_IDLE         0x0A
#define `$INSTANCE_NAME`_HID_SET_PROTOCOL     0x0B

/************************************************
 *  Descriptor Types (HID Chapter 7.1) 
 ************************************************/

#define `$INSTANCE_NAME`_DESCR_HID_CLASS         0x21
#define `$INSTANCE_NAME`_DESCR_HID_REPORT        0x22
#define `$INSTANCE_NAME`_DESCR_HID_PHYSICAL      0x23

#endif
