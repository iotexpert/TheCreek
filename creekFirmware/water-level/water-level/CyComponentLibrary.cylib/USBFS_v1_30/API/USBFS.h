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


#if !defined(`$INSTANCE_NAME`_H)
#define `$INSTANCE_NAME`_H

#include "cytypes.h"

#if(CYDEV_CHIP_DIE_EXPECT == CYDEV_CHIP_DIE_LEOPARD)     
    #if(CYDEV_CHIP_REV_EXPECT <= CYDEV_CHIP_REV_LEOPARD_ES2)      
        #include <intrins.h>
        #define `$INSTANCE_NAME`_ISR_PATCH() _nop_(); _nop_(); _nop_(); _nop_(); _nop_(); _nop_(); _nop_(); _nop_();
    #endif
#endif    

/************************************************
 *  General defines 
 ************************************************/
#if defined(__C51__) || defined(__CX51__)
#ifndef `$INSTANCE_NAME`_CODE
#define `$INSTANCE_NAME`_CODE code
#define `$INSTANCE_NAME`_DATA data
#define `$INSTANCE_NAME`_XDATA xdata
#define `$INSTANCE_NAME`_FAR far
#define `$INSTANCE_NAME`_NULL  ((void *) 0)
#endif
#else
#ifndef `$INSTANCE_NAME`_CODE
#define `$INSTANCE_NAME`_CODE
#define `$INSTANCE_NAME`_DATA
#define `$INSTANCE_NAME`_XDATA
#define `$INSTANCE_NAME`_FAR
#define `$INSTANCE_NAME`_NULL  ((void *) 0)
#endif
#endif

/************************************************
 *  Customizer Defines 
 ************************************************/
`$APIGEN_DEFINES`
#define `$INSTANCE_NAME`_MON_VBUS                   `$mon_vbus`
#define `$INSTANCE_NAME`_EP0_ISR_REMOVE             `$rm_ep_isr_0`
#define `$INSTANCE_NAME`_EP1_ISR_REMOVE             `$rm_ep_isr_1`
#define `$INSTANCE_NAME`_EP2_ISR_REMOVE             `$rm_ep_isr_2`
#define `$INSTANCE_NAME`_EP3_ISR_REMOVE             `$rm_ep_isr_3`
#define `$INSTANCE_NAME`_EP4_ISR_REMOVE             `$rm_ep_isr_4`
#define `$INSTANCE_NAME`_EP5_ISR_REMOVE             `$rm_ep_isr_5`
#define `$INSTANCE_NAME`_EP6_ISR_REMOVE             `$rm_ep_isr_6`
#define `$INSTANCE_NAME`_EP7_ISR_REMOVE             `$rm_ep_isr_7`
#define `$INSTANCE_NAME`_EP8_ISR_REMOVE             `$rm_ep_isr_8`
#if (`$INSTANCE_NAME`_MON_VBUS == 1)
#define `$INSTANCE_NAME`_VBUS_DR                    ((reg8 *) `$INSTANCE_NAME`_VBUS__DR)
#define `$INSTANCE_NAME`_VBUS_PS                    ((reg8 *) `$INSTANCE_NAME`_VBUS__PS)
#define `$INSTANCE_NAME`_VBUS_MASK                  `$INSTANCE_NAME`_VBUS__MASK
#endif

/************************************************
 *  Constants for `@INSTANCE_NAME` API. 
 ************************************************/
#define `$INSTANCE_NAME`_TRUE                       1
#define `$INSTANCE_NAME`_FALSE                      0

#define `$INSTANCE_NAME`_NO_EVENT_ALLOWED           2
#define `$INSTANCE_NAME`_EVENT_PENDING              1
#define `$INSTANCE_NAME`_NO_EVENT_PENDING           0

#define `$INSTANCE_NAME`_IN_BUFFER_FULL             `$INSTANCE_NAME`_NO_EVENT_PENDING
#define `$INSTANCE_NAME`_IN_BUFFER_EMPTY            `$INSTANCE_NAME`_EVENT_PENDING
#define `$INSTANCE_NAME`_OUT_BUFFER_FULL            `$INSTANCE_NAME`_EVENT_PENDING
#define `$INSTANCE_NAME`_OUT_BUFFER_EMPTY           `$INSTANCE_NAME`_NO_EVENT_PENDING

#define `$INSTANCE_NAME`_FORCE_J                    0xA0
#define `$INSTANCE_NAME`_FORCE_K                    0x80
#define `$INSTANCE_NAME`_FORCE_SE0                  0xC0
#define `$INSTANCE_NAME`_FORCE_NONE                 0x00

#define `$INSTANCE_NAME`_IDLE_TIMER_RUNNING         0x02
#define `$INSTANCE_NAME`_IDLE_TIMER_EXPIRED         0x01
#define `$INSTANCE_NAME`_IDLE_TIMER_INDEFINITE      0x00

#define `$INSTANCE_NAME`_DEVICE_STATUS_BUS_POWERED  0x00
#define `$INSTANCE_NAME`_DEVICE_STATUS_SELF_POWERED 0x01

#define `$INSTANCE_NAME`_3V_OPERATION               0x02
#define `$INSTANCE_NAME`_5V_OPERATION               0x03

#define `$INSTANCE_NAME`_MODE_DISABLE               0x00
#define	`$INSTANCE_NAME`_MODE_NAK_IN_OUT            0x01
#define	`$INSTANCE_NAME`_MODE_STATUS_OUT_ONLY       0x02
#define	`$INSTANCE_NAME`_MODE_STALL_IN_OUT          0x03
#define	`$INSTANCE_NAME`_MODE_RESERVED_0100         0x04
#define	`$INSTANCE_NAME`_MODE_ISO_OUT               0x05
#define	`$INSTANCE_NAME`_MODE_STATUS_IN_ONLY        0x06
#define	`$INSTANCE_NAME`_MODE_ISO_IN                0x07
#define	`$INSTANCE_NAME`_MODE_NAK_OUT               0x08
#define	`$INSTANCE_NAME`_MODE_ACK_OUT               0x09
#define	`$INSTANCE_NAME`_MODE_RESERVED_1010         0x0A
#define	`$INSTANCE_NAME`_MODE_ACK_OUT_STATUS_IN     0x0B
#define	`$INSTANCE_NAME`_MODE_NAK_IN                0x0C
#define	`$INSTANCE_NAME`_MODE_ACK_IN                0x0D
#define	`$INSTANCE_NAME`_MODE_RESERVED_1110         0x0E
#define	`$INSTANCE_NAME`_MODE_ACK_IN_STATUS_OUT     0x0F
#define	`$INSTANCE_NAME`_MODE_STALL_DATA_EP         0x80

#define `$INSTANCE_NAME`_MODE_ACKD                  0x10
#define `$INSTANCE_NAME`_MODE_OUT_RCVD              0x20
#define `$INSTANCE_NAME`_MODE_IN_RCVD               0x40
#define `$INSTANCE_NAME`_MODE_SETUP_RCVD            0x80

#define `$INSTANCE_NAME`_RQST_TYPE_MASK             0x60
#define `$INSTANCE_NAME`_RQST_TYPE_STD              0x00
#define `$INSTANCE_NAME`_RQST_TYPE_CLS              0x20
#define `$INSTANCE_NAME`_RQST_TYPE_VND              0x40
#define `$INSTANCE_NAME`_RQST_DIR_MASK              0x80
#define `$INSTANCE_NAME`_RQST_DIR_D2H               0x80
#define `$INSTANCE_NAME`_RQST_DIR_H2D               0x00
#define `$INSTANCE_NAME`_RQST_RCPT_MASK             0x03
#define `$INSTANCE_NAME`_RQST_RCPT_DEV              0x00
#define `$INSTANCE_NAME`_RQST_RCPT_IFC              0x01
#define `$INSTANCE_NAME`_RQST_RCPT_EP               0x02
#define `$INSTANCE_NAME`_RQST_RCPT_OTHER            0x03

/************************************************
 *  Standard Request Types (Table 9-4) 
 ************************************************/
#define `$INSTANCE_NAME`_GET_STATUS           0x00
#define `$INSTANCE_NAME`_CLEAR_FEATURE        0x01
#define `$INSTANCE_NAME`_SET_FEATURE          0x03
#define `$INSTANCE_NAME`_SET_ADDRESS          0x05
#define `$INSTANCE_NAME`_GET_DESCRIPTOR       0x06
#define `$INSTANCE_NAME`_SET_DESCRIPTOR       0x07
#define `$INSTANCE_NAME`_GET_CONFIGURATION    0x08
#define `$INSTANCE_NAME`_SET_CONFIGURATION    0x09
#define `$INSTANCE_NAME`_GET_INTERFACE        0x0A
#define `$INSTANCE_NAME`_SET_INTERFACE        0x0B
#define `$INSTANCE_NAME`_SYNCH_FRAME          0x0C

/************************************************
 *  Vendor Specific Request Types 
 ************************************************/
 /*Request for Microsoft OS String Descriptor*/
#define `$INSTANCE_NAME`_GET_CONFIG_DESCRIPTOR 0x01
/************************************************
 *  Descriptor Types (Table 9-5) 
 ************************************************/

#define `$INSTANCE_NAME`_DESCR_DEVICE           1
#define `$INSTANCE_NAME`_DESCR_CONFIG           2
#define `$INSTANCE_NAME`_DESCR_STRING           3
#define `$INSTANCE_NAME`_DESCR_INTERFACE        4
#define `$INSTANCE_NAME`_DESCR_ENDPOINT         5
#define `$INSTANCE_NAME`_DESCR_DEVICE_QUALIFIER 6
#define `$INSTANCE_NAME`_DESCR_OTHER_SPEED      7
#define `$INSTANCE_NAME`_DESCR_INTERFACE_POWER  8

/************************************************
 *  Device Descriptor Shifts 
 ************************************************/
#define `$INSTANCE_NAME`_DEVICE_DESCR_SN_SHIFT	16

/************************************************
 *  Feature Selectors (Table 9-6)
 ************************************************/
#define `$INSTANCE_NAME`_DEVICE_REMOTE_WAKEUP    0x01
#define `$INSTANCE_NAME`_ENDPOINT_HALT           0x00
#define `$INSTANCE_NAME`_TEST_MODE               0x02

/************************************************
 *  USB Device Status (Figure 9-4)
 ************************************************/
#define `$INSTANCE_NAME`_DEVICE_STATUS_BUS_POWERED   0x00
#define `$INSTANCE_NAME`_DEVICE_STATUS_SELF_POWERED  0x01
#define `$INSTANCE_NAME`_DEVICE_STATUS_REMOTE_WAKEUP 0x02

/************************************************
 *  USB Endpoint Status (Figure 9-4)
 ************************************************/
#define `$INSTANCE_NAME`_ENDPOINT_STATUS_HALT        0x01

/************************************************
 *  USB Endpoint Directions 
 ************************************************/
#define `$INSTANCE_NAME`_DIR_IN                          0x80
#define `$INSTANCE_NAME`_DIR_OUT                         0x00
#define `$INSTANCE_NAME`_DIR_UNUSED                      0x7F
/************************************************
 *  USB Endpoint Attributes 
 ************************************************/
#define `$INSTANCE_NAME`_EP_TYPE_CTRL                    0x00
#define `$INSTANCE_NAME`_EP_TYPE_ISOC                    0x01
#define `$INSTANCE_NAME`_EP_TYPE_BULK                    0x02
#define `$INSTANCE_NAME`_EP_TYPE_INT                     0x03
#define `$INSTANCE_NAME`_EP_TYPE_MASK                    0x03

#define `$INSTANCE_NAME`_EP_SYNC_TYPE_NO_SYNC            0x00
#define `$INSTANCE_NAME`_EP_SYNC_TYPE_ASYNC              0x04
#define `$INSTANCE_NAME`_EP_SYNC_TYPE_ADAPTIVE           0x08
#define `$INSTANCE_NAME`_EP_SYNC_TYPE_SYNCHRONOUS        0x0C
#define `$INSTANCE_NAME`_EP_SYNC_TYPE_MASK               0x0C

#define `$INSTANCE_NAME`_EP_USAGE_TYPE_DATA              0x00
#define `$INSTANCE_NAME`_EP_USAGE_TYPE_FEEDBACK          0x10
#define `$INSTANCE_NAME`_EP_USAGE_TYPE_IMPLICIT          0x20
#define `$INSTANCE_NAME`_EP_USAGE_TYPE_RESERVED          0x30
#define `$INSTANCE_NAME`_EP_USAGE_TYPE_MASK              0x30

/************************************************
 *  Transfer Completion Notification 
 ************************************************/
#define `$INSTANCE_NAME`_XFER_IDLE                       0x00
#define `$INSTANCE_NAME`_XFER_STATUS_ACK                 0x01
#define `$INSTANCE_NAME`_XFER_PREMATURE                  0x02
#define `$INSTANCE_NAME`_XFER_ERROR                      0x03

/*******************************************************************************
* Driver State defines
********************************************************************************/
#define `$INSTANCE_NAME`_TRANS_STATE_IDLE             0x00
#define `$INSTANCE_NAME`_TRANS_STATE_CONTROL_READ     0x02
#define `$INSTANCE_NAME`_TRANS_STATE_CONTROL_WRITE    0x04
#define `$INSTANCE_NAME`_TRANS_STATE_NO_DATA_CONTROL  0x06

/************************************************
 *  USB, ARB, SIE register definitions 
 ************************************************/
#define `$INSTANCE_NAME`_ARB_CFG        ((reg8 *) `$INSTANCE_NAME`_USB__ARB_CFG)

#define `$INSTANCE_NAME`_ARB_EP1_CFG    ((reg8 *) `$INSTANCE_NAME`_USB__ARB_EP1_CFG)
#define `$INSTANCE_NAME`_ARB_EP1_INT_EN ((reg8 *) `$INSTANCE_NAME`_USB__ARB_EP1_INT_EN)
#define `$INSTANCE_NAME`_ARB_EP1_SR     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_EP1_SR)

#define `$INSTANCE_NAME`_ARB_EP2_CFG    ((reg8 *) `$INSTANCE_NAME`_USB__ARB_EP2_CFG)
#define `$INSTANCE_NAME`_ARB_EP2_INT_EN ((reg8 *) `$INSTANCE_NAME`_USB__ARB_EP2_INT_EN)
#define `$INSTANCE_NAME`_ARB_EP2_SR     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_EP2_SR)

#define `$INSTANCE_NAME`_ARB_EP3_CFG    ((reg8 *) `$INSTANCE_NAME`_USB__ARB_EP3_CFG)
#define `$INSTANCE_NAME`_ARB_EP3_INT_EN ((reg8 *) `$INSTANCE_NAME`_USB__ARB_EP3_INT_EN)
#define `$INSTANCE_NAME`_ARB_EP3_SR     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_EP3_SR)

#define `$INSTANCE_NAME`_ARB_EP4_CFG    ((reg8 *) `$INSTANCE_NAME`_USB__ARB_EP4_CFG)
#define `$INSTANCE_NAME`_ARB_EP4_INT_EN ((reg8 *) `$INSTANCE_NAME`_USB__ARB_EP4_INT_EN)
#define `$INSTANCE_NAME`_ARB_EP4_SR     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_EP4_SR)

#define `$INSTANCE_NAME`_ARB_EP5_CFG    ((reg8 *) `$INSTANCE_NAME`_USB__ARB_EP5_CFG)
#define `$INSTANCE_NAME`_ARB_EP5_INT_EN ((reg8 *) `$INSTANCE_NAME`_USB__ARB_EP5_INT_EN)
#define `$INSTANCE_NAME`_ARB_EP5_SR     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_EP5_SR)

#define `$INSTANCE_NAME`_ARB_EP6_CFG    ((reg8 *) `$INSTANCE_NAME`_USB__ARB_EP6_CFG)
#define `$INSTANCE_NAME`_ARB_EP6_INT_EN ((reg8 *) `$INSTANCE_NAME`_USB__ARB_EP6_INT_EN)
#define `$INSTANCE_NAME`_ARB_EP6_SR     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_EP6_SR)

#define `$INSTANCE_NAME`_ARB_EP7_CFG    ((reg8 *) `$INSTANCE_NAME`_USB__ARB_EP7_CFG)
#define `$INSTANCE_NAME`_ARB_EP7_INT_EN ((reg8 *) `$INSTANCE_NAME`_USB__ARB_EP7_INT_EN)
#define `$INSTANCE_NAME`_ARB_EP7_SR     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_EP7_SR)

#define `$INSTANCE_NAME`_ARB_EP8_CFG    ((reg8 *) `$INSTANCE_NAME`_USB__ARB_EP8_CFG)
#define `$INSTANCE_NAME`_ARB_EP8_INT_EN ((reg8 *) `$INSTANCE_NAME`_USB__ARB_EP8_INT_EN)
#define `$INSTANCE_NAME`_ARB_EP8_SR     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_EP8_SR)

#define `$INSTANCE_NAME`_ARB_INT_EN     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_INT_EN)
#define `$INSTANCE_NAME`_ARB_INT_SR     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_INT_SR)

#define `$INSTANCE_NAME`_ARB_RW1_DR     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW1_DR)
#define `$INSTANCE_NAME`_ARB_RW1_RA     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW1_RA)
#define `$INSTANCE_NAME`_ARB_RW1_RA_MSB ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW1_RA_MSB)
#define `$INSTANCE_NAME`_ARB_RW1_WA     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW1_WA)
#define `$INSTANCE_NAME`_ARB_RW1_WA_MSB ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW1_WA_MSB)

#define `$INSTANCE_NAME`_ARB_RW2_DR     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW2_DR)
#define `$INSTANCE_NAME`_ARB_RW2_RA     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW2_RA)
#define `$INSTANCE_NAME`_ARB_RW2_RA_MSB ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW2_RA_MSB)
#define `$INSTANCE_NAME`_ARB_RW2_WA     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW2_WA)
#define `$INSTANCE_NAME`_ARB_RW2_WA_MSB ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW2_WA_MSB)

#define `$INSTANCE_NAME`_ARB_RW3_DR     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW3_DR)
#define `$INSTANCE_NAME`_ARB_RW3_RA     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW3_RA)
#define `$INSTANCE_NAME`_ARB_RW3_RA_MSB ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW3_RA_MSB)
#define `$INSTANCE_NAME`_ARB_RW3_WA     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW3_WA)
#define `$INSTANCE_NAME`_ARB_RW3_WA_MSB ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW3_WA_MSB)

#define `$INSTANCE_NAME`_ARB_RW4_DR     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW4_DR)
#define `$INSTANCE_NAME`_ARB_RW4_RA     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW4_RA)
#define `$INSTANCE_NAME`_ARB_RW4_RA_MSB ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW4_RA_MSB)
#define `$INSTANCE_NAME`_ARB_RW4_WA     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW4_WA)
#define `$INSTANCE_NAME`_ARB_RW4_WA_MSB ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW4_WA_MSB)

#define `$INSTANCE_NAME`_ARB_RW5_DR     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW5_DR)
#define `$INSTANCE_NAME`_ARB_RW5_RA     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW5_RA)
#define `$INSTANCE_NAME`_ARB_RW5_RA_MSB ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW5_RA_MSB)
#define `$INSTANCE_NAME`_ARB_RW5_WA     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW5_WA)
#define `$INSTANCE_NAME`_ARB_RW5_WA_MSB ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW5_WA_MSB)

#define `$INSTANCE_NAME`_ARB_RW6_DR     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW6_DR)
#define `$INSTANCE_NAME`_ARB_RW6_RA     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW6_RA)
#define `$INSTANCE_NAME`_ARB_RW6_RA_MSB ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW6_RA_MSB)
#define `$INSTANCE_NAME`_ARB_RW6_WA     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW6_WA)
#define `$INSTANCE_NAME`_ARB_RW6_WA_MSB ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW6_WA_MSB)

#define `$INSTANCE_NAME`_ARB_RW7_DR     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW7_DR)
#define `$INSTANCE_NAME`_ARB_RW7_RA     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW7_RA)
#define `$INSTANCE_NAME`_ARB_RW7_RA_MSB ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW7_RA_MSB)
#define `$INSTANCE_NAME`_ARB_RW7_WA     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW7_WA)
#define `$INSTANCE_NAME`_ARB_RW7_WA_MSB ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW7_WA_MSB)

#define `$INSTANCE_NAME`_ARB_RW8_DR     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW8_DR)
#define `$INSTANCE_NAME`_ARB_RW8_RA     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW8_RA)
#define `$INSTANCE_NAME`_ARB_RW8_RA_MSB ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW8_RA_MSB)
#define `$INSTANCE_NAME`_ARB_RW8_WA     ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW8_WA)
#define `$INSTANCE_NAME`_ARB_RW8_WA_MSB ((reg8 *) `$INSTANCE_NAME`_USB__ARB_RW8_WA_MSB)

#define `$INSTANCE_NAME`_BUF_SIZE       ((reg8 *) `$INSTANCE_NAME`_USB__BUF_SIZE)
#define `$INSTANCE_NAME`_BUS_RST_CNT    ((reg8 *) `$INSTANCE_NAME`_USB__BUS_RST_CNT)
#define `$INSTANCE_NAME`_CR0            ((reg8 *) `$INSTANCE_NAME`_USB__CR0)
#define `$INSTANCE_NAME`_CR1            ((reg8 *) `$INSTANCE_NAME`_USB__CR1)
#define `$INSTANCE_NAME`_CWA            ((reg8 *) `$INSTANCE_NAME`_USB__CWA)
#define `$INSTANCE_NAME`_CWA_MSB        ((reg8 *) `$INSTANCE_NAME`_USB__CWA_MSB)

#define `$INSTANCE_NAME`_DMA_THRES      ((reg8 *) `$INSTANCE_NAME`_USB__DMA_THRES)
#define `$INSTANCE_NAME`_DMA_THRES_MSB  ((reg8 *) `$INSTANCE_NAME`_USB__DMA_THRES_MSB)

#define `$INSTANCE_NAME`_EP_ACTIVE      ((reg8 *) `$INSTANCE_NAME`_USB__EP_ACTIVE)
#define `$INSTANCE_NAME`_EP_TYPE        ((reg8 *) `$INSTANCE_NAME`_USB__EP_TYPE)

#define `$INSTANCE_NAME`_EP0_CNT        ((reg8 *) `$INSTANCE_NAME`_USB__EP0_CNT)
#define `$INSTANCE_NAME`_EP0_CR         ((reg8 *) `$INSTANCE_NAME`_USB__EP0_CR)
#define `$INSTANCE_NAME`_EP0_DR0        ((reg8 *) `$INSTANCE_NAME`_USB__EP0_DR0)
#define `$INSTANCE_NAME`_EP0_DR1        ((reg8 *) `$INSTANCE_NAME`_USB__EP0_DR1)
#define `$INSTANCE_NAME`_EP0_DR2        ((reg8 *) `$INSTANCE_NAME`_USB__EP0_DR2)
#define `$INSTANCE_NAME`_EP0_DR3        ((reg8 *) `$INSTANCE_NAME`_USB__EP0_DR3)
#define `$INSTANCE_NAME`_EP0_DR4        ((reg8 *) `$INSTANCE_NAME`_USB__EP0_DR4)
#define `$INSTANCE_NAME`_EP0_DR5        ((reg8 *) `$INSTANCE_NAME`_USB__EP0_DR5)
#define `$INSTANCE_NAME`_EP0_DR6        ((reg8 *) `$INSTANCE_NAME`_USB__EP0_DR6)
#define `$INSTANCE_NAME`_EP0_DR7        ((reg8 *) `$INSTANCE_NAME`_USB__EP0_DR7)

#define `$INSTANCE_NAME`_OSCLK_DR0      ((reg8 *) `$INSTANCE_NAME`_USB__OSCLK_DR0)
#define `$INSTANCE_NAME`_OSCLK_DR1      ((reg8 *) `$INSTANCE_NAME`_USB__OSCLK_DR1)

#define `$INSTANCE_NAME`_PM_ACT_CFG     ((reg8 *) `$INSTANCE_NAME`_USB__PM_ACT_CFG)
#define `$INSTANCE_NAME`_PM_ACT_MSK     ((reg8 *) `$INSTANCE_NAME`_USB__PM_ACT_MSK)
#define `$INSTANCE_NAME`_PM_STBY_CFG    ((reg8 *) `$INSTANCE_NAME`_USB__PM_STBY_CFG)
#define `$INSTANCE_NAME`_PM_STBY_MSK    ((reg8 *) `$INSTANCE_NAME`_USB__PM_STBY_MSK)

#define `$INSTANCE_NAME`_SIE_EP_INT_EN  ((reg8 *) `$INSTANCE_NAME`_USB__SIE_EP_INT_EN)
#define `$INSTANCE_NAME`_SIE_EP_INT_SR  ((reg8 *) `$INSTANCE_NAME`_USB__SIE_EP_INT_SR)

#define `$INSTANCE_NAME`_SIE_EP1_CNT0   ((reg8 *) `$INSTANCE_NAME`_USB__SIE_EP1_CNT0)
#define `$INSTANCE_NAME`_SIE_EP1_CNT1   ((reg8 *) `$INSTANCE_NAME`_USB__SIE_EP1_CNT1)
#define `$INSTANCE_NAME`_SIE_EP1_CR0    ((reg8 *) `$INSTANCE_NAME`_USB__SIE_EP1_CR0)

#define `$INSTANCE_NAME`_SIE_EP2_CNT0   ((reg8 *) `$INSTANCE_NAME`_USB__SIE_EP2_CNT0)
#define `$INSTANCE_NAME`_SIE_EP2_CNT1   ((reg8 *) `$INSTANCE_NAME`_USB__SIE_EP2_CNT1)
#define `$INSTANCE_NAME`_SIE_EP2_CR0    ((reg8 *) `$INSTANCE_NAME`_USB__SIE_EP2_CR0)

#define `$INSTANCE_NAME`_SIE_EP3_CNT0   ((reg8 *) `$INSTANCE_NAME`_USB__SIE_EP3_CNT0)
#define `$INSTANCE_NAME`_SIE_EP3_CNT1   ((reg8 *) `$INSTANCE_NAME`_USB__SIE_EP3_CNT1)
#define `$INSTANCE_NAME`_SIE_EP3_CR0    ((reg8 *) `$INSTANCE_NAME`_USB__SIE_EP3_CR0)

#define `$INSTANCE_NAME`_SIE_EP4_CNT0   ((reg8 *) `$INSTANCE_NAME`_USB__SIE_EP4_CNT0)
#define `$INSTANCE_NAME`_SIE_EP4_CNT1   ((reg8 *) `$INSTANCE_NAME`_USB__SIE_EP4_CNT1)
#define `$INSTANCE_NAME`_SIE_EP4_CR0    ((reg8 *) `$INSTANCE_NAME`_USB__SIE_EP4_CR0)

#define `$INSTANCE_NAME`_SIE_EP5_CNT0   ((reg8 *) `$INSTANCE_NAME`_USB__SIE_EP5_CNT0)
#define `$INSTANCE_NAME`_SIE_EP5_CNT1   ((reg8 *) `$INSTANCE_NAME`_USB__SIE_EP5_CNT1)
#define `$INSTANCE_NAME`_SIE_EP5_CR0    ((reg8 *) `$INSTANCE_NAME`_USB__SIE_EP5_CR0)

#define `$INSTANCE_NAME`_SIE_EP6_CNT0   ((reg8 *) `$INSTANCE_NAME`_USB__SIE_EP6_CNT0)
#define `$INSTANCE_NAME`_SIE_EP6_CNT1   ((reg8 *) `$INSTANCE_NAME`_USB__SIE_EP6_CNT1)
#define `$INSTANCE_NAME`_SIE_EP6_CR0    ((reg8 *) `$INSTANCE_NAME`_USB__SIE_EP6_CR0)

#define `$INSTANCE_NAME`_SIE_EP7_CNT0   ((reg8 *) `$INSTANCE_NAME`_USB__SIE_EP7_CNT0)
#define `$INSTANCE_NAME`_SIE_EP7_CNT1   ((reg8 *) `$INSTANCE_NAME`_USB__SIE_EP7_CNT1)
#define `$INSTANCE_NAME`_SIE_EP7_CR0    ((reg8 *) `$INSTANCE_NAME`_USB__SIE_EP7_CR0)

#define `$INSTANCE_NAME`_SIE_EP8_CNT0   ((reg8 *) `$INSTANCE_NAME`_USB__SIE_EP8_CNT0)
#define `$INSTANCE_NAME`_SIE_EP8_CNT1   ((reg8 *) `$INSTANCE_NAME`_USB__SIE_EP8_CNT1)
#define `$INSTANCE_NAME`_SIE_EP8_CR0    ((reg8 *) `$INSTANCE_NAME`_USB__SIE_EP8_CR0)

#define `$INSTANCE_NAME`_SOF0           ((reg8 *) `$INSTANCE_NAME`_USB__SOF0)
#define `$INSTANCE_NAME`_SOF1           ((reg8 *) `$INSTANCE_NAME`_USB__SOF1)

#define `$INSTANCE_NAME`_USB_CLK_EN     ((reg8 *) `$INSTANCE_NAME`_USB__USB_CLK_EN)

#define `$INSTANCE_NAME`_USBIO_CR0      ((reg8 *) `$INSTANCE_NAME`_USB__USBIO_CR0)
#define `$INSTANCE_NAME`_USBIO_CR1      ((reg8 *) `$INSTANCE_NAME`_USB__USBIO_CR1)
#define `$INSTANCE_NAME`_USBIO_CR2      ((reg8 *) `$INSTANCE_NAME`_USB__USBIO_CR2)

#define `$INSTANCE_NAME`_USB_MEM        ((reg8 *) CYDEV_USB_MEM_BASE)

#if(CYDEV_CHIP_DIE_EXPECT == CYDEV_CHIP_DIE_LEOPARD)
    /* PSoC3 interrupt registers*/
    #define `$INSTANCE_NAME`_USB_ISR_PRIOR  ((reg8 *) CYDEV_INTC_PRIOR0)
    #define `$INSTANCE_NAME`_USB_ISR_SET_EN ((reg8 *) CYDEV_INTC_SET_EN0)
    #define `$INSTANCE_NAME`_USB_ISR_CLR_EN ((reg8 *) CYDEV_INTC_CLR_EN0)
    #define `$INSTANCE_NAME`_USB_ISR_VECT   ((cyisraddress *) CYDEV_INTC_VECT_MBASE)
#else
#if(CYDEV_CHIP_DIE_EXPECT == CYDEV_CHIP_DIE_PANTHER)
    /* PSoC5 interrupt registers*/ 
    #define `$INSTANCE_NAME`_USB_ISR_PRIOR  ((reg8 *) CYDEV_NVIC_PRI_0)
    #define `$INSTANCE_NAME`_USB_ISR_SET_EN ((reg8 *) CYDEV_NVIC_SETENA0)
    #define `$INSTANCE_NAME`_USB_ISR_CLR_EN ((reg8 *) CYDEV_NVIC_CLRENA0)
    #define `$INSTANCE_NAME`_USB_ISR_VECT   ((cyisraddress *) CYDEV_NVIC_VECT_OFFSET)
#endif
#endif

/************************************************
 *  Interrupt vectors, masks and priorities 
 ************************************************/
#define `$INSTANCE_NAME`_BUS_RESET_PRIOR     `$INSTANCE_NAME`_bus_reset__INTC_PRIOR_NUM
#define `$INSTANCE_NAME`_BUS_RESET_MASK      `$INSTANCE_NAME`_bus_reset__INTC_MASK
#define `$INSTANCE_NAME`_BUS_RESET_VECT_NUM  `$INSTANCE_NAME`_bus_reset__INTC_NUMBER
 
#define `$INSTANCE_NAME`_SOF_PRIOR     `$INSTANCE_NAME`_sof_int__INTC_PRIOR_NUM 
#define `$INSTANCE_NAME`_SOF_MASK      `$INSTANCE_NAME`_sof_int__INTC_MASK
#define `$INSTANCE_NAME`_SOF_VECT_NUM  `$INSTANCE_NAME`_sof_int__INTC_NUMBER
 
#define `$INSTANCE_NAME`_EP_0_PRIOR     `$INSTANCE_NAME`_ep_0__INTC_PRIOR_NUM
#define `$INSTANCE_NAME`_EP_0_MASK      `$INSTANCE_NAME`_ep_0__INTC_MASK
#define `$INSTANCE_NAME`_EP_0_VECT_NUM  `$INSTANCE_NAME`_ep_0__INTC_NUMBER
 
#define `$INSTANCE_NAME`_EP_1_PRIOR     `$INSTANCE_NAME`_ep_1__INTC_PRIOR_NUM 
#define `$INSTANCE_NAME`_EP_1_MASK      `$INSTANCE_NAME`_ep_1__INTC_MASK
#define `$INSTANCE_NAME`_EP_1_VECT_NUM  `$INSTANCE_NAME`_ep_1__INTC_NUMBER
 
#define `$INSTANCE_NAME`_EP_2_PRIOR     `$INSTANCE_NAME`_ep_2__INTC_PRIOR_NUM 
#define `$INSTANCE_NAME`_EP_2_MASK      `$INSTANCE_NAME`_ep_2__INTC_MASK
#define `$INSTANCE_NAME`_EP_2_VECT_NUM  `$INSTANCE_NAME`_ep_2__INTC_NUMBER
 
#define `$INSTANCE_NAME`_EP_3_PRIOR     `$INSTANCE_NAME`_ep_3__INTC_PRIOR_NUM
#define `$INSTANCE_NAME`_EP_3_MASK      `$INSTANCE_NAME`_ep_3__INTC_MASK
#define `$INSTANCE_NAME`_EP_3_VECT_NUM  `$INSTANCE_NAME`_ep_3__INTC_NUMBER
 
#define `$INSTANCE_NAME`_EP_4_PRIOR     `$INSTANCE_NAME`_ep_4__INTC_PRIOR_NUM 
#define `$INSTANCE_NAME`_EP_4_MASK      `$INSTANCE_NAME`_ep_4__INTC_MASK
#define `$INSTANCE_NAME`_EP_4_VECT_NUM  `$INSTANCE_NAME`_ep_4__INTC_NUMBER
 
#define `$INSTANCE_NAME`_EP_5_PRIOR     `$INSTANCE_NAME`_ep_5__INTC_PRIOR_NUM
#define `$INSTANCE_NAME`_EP_5_MASK      `$INSTANCE_NAME`_ep_5__INTC_MASK
#define `$INSTANCE_NAME`_EP_5_VECT_NUM  `$INSTANCE_NAME`_ep_5__INTC_NUMBER
 
#define `$INSTANCE_NAME`_EP_6_PRIOR     `$INSTANCE_NAME`_ep_6__INTC_PRIOR_NUM
#define `$INSTANCE_NAME`_EP_6_MASK      `$INSTANCE_NAME`_ep_6__INTC_MASK
#define `$INSTANCE_NAME`_EP_6_VECT_NUM  `$INSTANCE_NAME`_ep_6__INTC_NUMBER
 
#define `$INSTANCE_NAME`_EP_7_PRIOR     `$INSTANCE_NAME`_ep_7__INTC_PRIOR_NUM 
#define `$INSTANCE_NAME`_EP_7_MASK      `$INSTANCE_NAME`_ep_7__INTC_MASK
#define `$INSTANCE_NAME`_EP_7_VECT_NUM  `$INSTANCE_NAME`_ep_7__INTC_NUMBER
 
#define `$INSTANCE_NAME`_EP_8_PRIOR     `$INSTANCE_NAME`_ep_8__INTC_PRIOR_NUM
#define `$INSTANCE_NAME`_EP_8_MASK      `$INSTANCE_NAME`_ep_8__INTC_MASK
#define `$INSTANCE_NAME`_EP_8_VECT_NUM  `$INSTANCE_NAME`_ep_8__INTC_NUMBER
 
/************************************************
 *  Register Masks 
 ************************************************/
#define `$INSTANCE_NAME`_CR1_REG_ENABLE             0x01u
#define `$INSTANCE_NAME`_CR1_ENABLE_LOCK            0x02u
#define `$INSTANCE_NAME`_CR1_BUS_ACTIVITY           0x04u
#define `$INSTANCE_NAME`_EP0_CNT_DATA_TOGGLE        0x80u
#define `$INSTANCE_NAME`_EPX_CNT_DATA_TOGGLE        0x80u
#define `$INSTANCE_NAME`_EPX_CNT0_MASK              0x07u
#define `$INSTANCE_NAME`_CR0_ENABLE                 0x80u

#define `$INSTANCE_NAME`_USBIO_CR1_IOMODE           0x80u
#define `$INSTANCE_NAME`_USBIO_CR1_DRIVE_MODE       0x40u
#define `$INSTANCE_NAME`_USBIO_CR1_DPI              0x20u
#define `$INSTANCE_NAME`_USBIO_CR1_DMI              0x10u
#define `$INSTANCE_NAME`_USBIO_CR1_P2PUEN           0x08u
#define `$INSTANCE_NAME`_USBIO_CR1_USBPUEN          0x04u
#define `$INSTANCE_NAME`_USBIO_CR1_DP0              0x02u
#define `$INSTANCE_NAME`_USBIO_CR1_DM0              0x01u

#define `$INSTANCE_NAME`_FASTCLK_IMO_CR_USBCLK_ON   0x40u
#define `$INSTANCE_NAME`_FASTCLK_IMO_CR_XCLKEN      0x20u
#define `$INSTANCE_NAME`_FASTCLK_IMO_CR_FX2ON       0x10u

#define `$INSTANCE_NAME`_ARB_EPX_CFG_RESET_PTR      0x08u
#define `$INSTANCE_NAME`_ARB_EPX_CFG_CRC_BYPASS     0x04u
#define `$INSTANCE_NAME`_ARB_EPX_CFG_DMA_REQ        0x02u
#define `$INSTANCE_NAME`_ARB_EPX_CFG_IN_DATA_RDY    0x01u

/************************************************
 *  Endpoint 0 offsets (Table 9-2) 
 ************************************************/

#define `$INSTANCE_NAME`_bmRequestType   `$INSTANCE_NAME`_EP0_DR0
#define `$INSTANCE_NAME`_bRequest        `$INSTANCE_NAME`_EP0_DR1
#define `$INSTANCE_NAME`_wValue          `$INSTANCE_NAME`_EP0_DR2
#define `$INSTANCE_NAME`_wValueHi        `$INSTANCE_NAME`_EP0_DR3
#define `$INSTANCE_NAME`_wValueLo        `$INSTANCE_NAME`_EP0_DR2
#define `$INSTANCE_NAME`_wIndex          `$INSTANCE_NAME`_EP0_DR4
#define `$INSTANCE_NAME`_wIndexHi        `$INSTANCE_NAME`_EP0_DR5
#define `$INSTANCE_NAME`_wIndexLo        `$INSTANCE_NAME`_EP0_DR4
#define `$INSTANCE_NAME`_wLength         `$INSTANCE_NAME`_EP0_DR6
#define `$INSTANCE_NAME`_wLengthHi       `$INSTANCE_NAME`_EP0_DR7
#define `$INSTANCE_NAME`_wLengthLo       `$INSTANCE_NAME`_EP0_DR6

#define TRUE  1
#define FALSE 0
/************************************************
 *  Optional Features 
 ************************************************/
`$APIGEN_DEFINES`

#if (`$INSTANCE_NAME`_MON_VBUS == 1)
#define `$INSTANCE_NAME`_VBUS_DR        ((reg8 *) `$INSTANCE_NAME`_VBUS__DR)
#define `$INSTANCE_NAME`_VBUS_PS        ((reg8 *) `$INSTANCE_NAME`_VBUS__PS)
#define `$INSTANCE_NAME`_VBUS_MASK      `$INSTANCE_NAME`_VBUS__MASK
#endif

/*******************************************************************************
* typedefs
********************************************************************************/
typedef struct {
    uint8  bAttrib;
    uint8  bAPIEPState;
    uint8  bHWEPState;
    uint8  bEPToggle;
    uint8  bAddr;
    uint8  bEPMode;
    uint16 wBuffOffset;
    uint16 wBufferSize;
} T_`$INSTANCE_NAME`_EP_CTL_BLOCK;

typedef struct {
    uint8  bInterface;
    uint8  bAltSetting;
    uint8  bAddr;
    uint8  bmAttributes;
    uint16 wBufferSize;
	uint8  bMisc;
} T_`$INSTANCE_NAME`_EP_SETTINGS_BLOCK;

typedef struct {
    uint8  bStatus;
    uint16 wLength;
} T_`$INSTANCE_NAME`_XFER_STATUS_BLOCK;

typedef struct {
    uint16  wCount;
    uint8   *pData;
    T_`$INSTANCE_NAME`_XFER_STATUS_BLOCK *pStatusBlock;
} T_`$INSTANCE_NAME`_TD;

typedef struct {
    uint8   c;
    void    *p_list;
} T_`$INSTANCE_NAME`_LUT;

/************************************************
 *  Prototypes of the `@INSTANCE_NAME` API. 
 ************************************************/
extern void   `@INSTANCE_NAME`_Start(uint8 bDevice, uint8 bMode);
extern void   `@INSTANCE_NAME`_Stop(void);
extern uint8  `@INSTANCE_NAME`_bCheckActivity(void);
extern uint8  `@INSTANCE_NAME`_bGetConfiguration(void);
uint8  `@INSTANCE_NAME`_bGetInterfaceSetting(uint8 ifc);
extern uint8  `@INSTANCE_NAME`_bGetEPState(uint8 bEPNumber);
extern uint16 `@INSTANCE_NAME`_wGetEPCount(uint8 bEPNumber);
extern void   `@INSTANCE_NAME`_LoadEP(uint8 bEPNumber, uint8 *pData, uint16 wLength);
extern uint16 `@INSTANCE_NAME`_ReadOutEP(uint8 bEPNumber, uint8 *pData, uint16 wLength);
extern void   `@INSTANCE_NAME`_EnableOutEP(uint8 bEPNumber);
extern void   `@INSTANCE_NAME`_DisableOutEP(uint8 bEPNumber);
extern void   `@INSTANCE_NAME`_Force(uint8 bState);
extern uint8  `@INSTANCE_NAME`_bGetEPAckState(uint8 bEPNumber);
extern void   `@INSTANCE_NAME`_SetPowerStatus(uint8 bPowerStaus);
extern uint8  `@INSTANCE_NAME`_bRWUEnabled(void);
extern void  `$INSTANCE_NAME`_SerialNumString(uint8 *SNstring);


#define `@INSTANCE_NAME`_LoadInEP(e, p, l)        `@INSTANCE_NAME`_LoadEP(e, p, l)
#define `@INSTANCE_NAME`_LoadInISOCEP(e, p, l)    `@INSTANCE_NAME`_LoadEP(e, p, l)
#define `@INSTANCE_NAME`_EnableOutISOCEP(e)       `@INSTANCE_NAME`_EnableOutEP(e)

#if (`$INSTANCE_NAME`_MON_VBUS == 1)
extern uint8  `@INSTANCE_NAME`_bVBusPresent(void);
#endif

#endif
