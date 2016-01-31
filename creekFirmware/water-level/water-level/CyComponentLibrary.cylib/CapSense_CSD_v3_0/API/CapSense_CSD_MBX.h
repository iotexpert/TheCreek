/*******************************************************************************
* File Name: `$INSTANCE_NAME`_MBX.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides constants and structure declarations for the tuner 
*  communication the for CapSense CSD component.
*
* Note:
*
********************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(CY_CAPSENSE_CSD_MBX_`$INSTANCE_NAME`_H)
#define CY_CAPSENSE_CSD_MBX_`$INSTANCE_NAME`_H

#include "`$INSTANCE_NAME`.h"
#include "`$INSTANCE_NAME`_CSHL.h"


/***************************************
*   Condition compilation parameters
***************************************/

/* Check Sum to identify data */
#define `$INSTANCE_NAME`_CHECK_SUM      (`$CheckSum`u)

#define `$INSTANCE_NAME`_TOTAL_SENSOR_MASK_COUNT     (((`$INSTANCE_NAME`_TOTAL_SENSOR_COUNT - 1u) / 8u) + 1u)

/* Do nothing from big endian compilers (__C51__), for ARM compilers need to swap bytes */
#if defined(__C51__) || defined(__CX51__)
    #define `$INSTANCE_NAME`_SWAP_ENDIAN16(x) (x)
    #define CYPACKED
#else
    #define `$INSTANCE_NAME`_SWAP_ENDIAN16(x) ((uint16)(((x) << 8) | ((x) >> 8)))
    #define CYPACKED __attribute__ ((packed))
#endif  /* End (defined(__C51__)) */

#define `$INSTANCE_NAME`_WIDGET_CSHL_PARAMETERS_COUNT           (`$INSTANCE_NAME`_TOTAL_WIDGET_COUNT + \
                                                                 `$INSTANCE_NAME`_TOTAL_TOUCH_PADS_COUNT + \
                                                                 `$INSTANCE_NAME`_TOTAL_MATRIX_BUTTONS_COUNT)

#define `$INSTANCE_NAME`_WIDGET_RESOLUTION_PARAMETERS_COUNT     (`$INSTANCE_NAME`_WIDGET_CSHL_PARAMETERS_COUNT + \
                                                                 `$INSTANCE_NAME`_TOTAL_GENERICS_COUNT)


/***************************************
*      Type defines for mailboxes
***************************************/

/* Outbox structure definition */
typedef struct _`$INSTANCE_NAME`_outbox
{       
    #if (`$INSTANCE_NAME`_TOTAL_CENTROIDS_COUNT)
        uint16   position[`$INSTANCE_NAME`_TOTAL_CENTROIDS_COUNT];
    #endif  /* End (`$INSTANCE_NAME`_TOTAL_CENTROIDS_COUNT) */

    #if (`$INSTANCE_NAME`_TOTAL_MATRIX_BUTTONS_COUNT)
        uint8   mb_position[`$INSTANCE_NAME`_TOTAL_MATRIX_BUTTONS_COUNT * 2u];
    #endif

    uint16   rawData[`$INSTANCE_NAME`_TOTAL_SENSOR_COUNT];
    uint16   baseLine[`$INSTANCE_NAME`_TOTAL_SENSOR_COUNT];
    `$SignalSizeReplacementString`    signal[`$INSTANCE_NAME`_TOTAL_SENSOR_COUNT];
    uint8    onMask[`$INSTANCE_NAME`_TOTAL_SENSOR_MASK_COUNT];
    
    #if (`$INSTANCE_NAME`_TUNING_METHOD == `$INSTANCE_NAME`_AUTO_TUNING)
        uint8   fingerThreshold[`$INSTANCE_NAME`_WIDGET_CSHL_PARAMETERS_COUNT];
        uint8   noiseThreshold[`$INSTANCE_NAME`_WIDGET_CSHL_PARAMETERS_COUNT];
        uint8   scanResolution[`$INSTANCE_NAME`_WIDGET_RESOLUTION_PARAMETERS_COUNT];
        uint8   idacValue[`$INSTANCE_NAME`_TOTAL_SENSOR_COUNT];
		uint8   analogSwitchDivider[`$INSTANCE_NAME`_TOTAL_SCANSLOT_COUNT];
    #endif  /* End (`$INSTANCE_NAME`_TUNING_METHOD == `$INSTANCE_NAME`_MANUAL_TUNING) */

    #if(`$INSTANCE_NAME`_TUNING_METHOD == `$INSTANCE_NAME`_MANUAL_TUNING)
        uint8   noReadMsg;
    #endif  /* End (`$INSTANCE_NAME`_TUNING_METHOD == `$INSTANCE_NAME`_MANUAL_TUNING) */
    uint16  checkSum;
    
} CYPACKED `$INSTANCE_NAME`_OUTBOX;

/* Inbox structure definition */
#if (`$INSTANCE_NAME`_TUNING_METHOD == `$INSTANCE_NAME`_MANUAL_TUNING)
    typedef struct _`$INSTANCE_NAME`_inbox
    {
        uint8   sensorIndex;
        #if (`$INSTANCE_NAME`_CURRENT_SOURCE)
            uint8   idacSettings;
        #endif /* End `$INSTANCE_NAME`_CURRENT_SOURCE */
        uint8   resolution;
        uint8   fingerThreshold;
        uint8   noiseThreshold;
        uint8   hysteresis;
        uint8   debounce;
        uint8   analogSwitchDivider;
    } CYPACKED `$INSTANCE_NAME`_INBOX;
    
#endif  /* End (`$INSTANCE_NAME`_TUNING_METHOD == `$INSTANCE_NAME`_MANUAL_TUNING) */

/* Mailbox structure definition */
typedef struct _`$INSTANCE_NAME`_mailBox
{
    uint8   type;               /* Must be the first byte with values ranging from 0-119 */
    uint16  size;               /* Must be the size of this data structure. Range between 0-127 */
    `$INSTANCE_NAME`_OUTBOX  outbox;
    #if (`$INSTANCE_NAME`_TUNING_METHOD == `$INSTANCE_NAME`_MANUAL_TUNING)
        `$INSTANCE_NAME`_INBOX inbox;
    #endif  /* End (`$INSTANCE_NAME`_TUNING_METHOD == `$INSTANCE_NAME`_MANUAL_TUNING) */
    
} CYPACKED `$INSTANCE_NAME`_MAILBOX;


/* Mailboxes structure definition */
typedef struct _`$INSTANCE_NAME`_mailboxes
{
    uint8   numMailBoxes;       /* This must always be the first. Represents # of mailboxes */
    `$INSTANCE_NAME`_MAILBOX    csdMailbox;
    
} CYPACKED `$INSTANCE_NAME`_MAILBOXES;


/***************************************
*        Function Prototypes
***************************************/

void `$INSTANCE_NAME`_InitMailbox(volatile `$INSTANCE_NAME`_MAILBOX *mbx);
void `$INSTANCE_NAME`_PostMessage(volatile `$INSTANCE_NAME`_MAILBOX *mbx);
#if (`$INSTANCE_NAME`_TUNING_METHOD == `$INSTANCE_NAME`_MANUAL_TUNING)
    void `$INSTANCE_NAME`_ReadMessage(volatile `$INSTANCE_NAME`_MAILBOX *mbx);
#endif  /* End (`$INSTANCE_NAME`_TUNING_METHOD == `$INSTANCE_NAME`_MANUAL_TUNING) */


/***************************************
*           API Constants        
***************************************/

/* The selected ID for this version of Tuner */
#define `$INSTANCE_NAME`_TYPE_ID        (0x04u)

#define `$INSTANCE_NAME`_BUSY_FLAG      (`$INSTANCE_NAME`_TYPE_ID | 0x80u)
#define `$INSTANCE_NAME`_HAVE_MSG       (sizeof(`$INSTANCE_NAME`_MAILBOX) | 0x8000u)


#endif  /* End (CY_CAPSENSE_CSD_MBX_`$INSTANCE_NAME`_H) */


/* [] END OF FILE */
