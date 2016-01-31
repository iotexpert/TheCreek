/*******************************************************************************
* File Name: `$INSTANCE_NAME`_cmd.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides constants and parameter values and API definition related
*  to SM/PM bus command handling.
*
********************************************************************************
* Copyright 2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/

#include "cytypes.h"
#include "`$INSTANCE_NAME`.h"


/***************************************
* Data Type Definition
***************************************/
typedef struct
{
    uint8       command;
    uint8       dataLength;
    uint8 *     dataPtr;
    uint8       cmdProp;               /* command properties (read/write/page-indexed) */
} `$INSTANCE_NAME`_CMD_TABLE_ENTRY;


/***************************************
* Global Variable Declaration
***************************************/
extern volatile `$INSTANCE_NAME`_CMD_TABLE_ENTRY `$INSTANCE_NAME`_commands[];

/* Stores the code of last received command */
extern volatile uint8 `$INSTANCE_NAME`_lastReceivedCmd;

/* Last received command properties */
extern volatile uint8 `$INSTANCE_NAME`_cmdProperties;

 /* Current page value */
extern volatile uint8 `$INSTANCE_NAME`_cmdPage;


/***************************************
*        Function Prototypes
***************************************/
uint8 `$INSTANCE_NAME`_CheckCommand(uint8 command) CYREENTRANT;
uint8 `$INSTANCE_NAME`_SearchCommand(uint8 command) CYREENTRANT;
void `$INSTANCE_NAME`_ReadManualHandler(void) CYREENTRANT;
void `$INSTANCE_NAME`_ReadAutoHandler(void) CYREENTRANT;
void `$INSTANCE_NAME`_WriteHandler(void) CYREENTRANT;


/***************************************
*           API Constants
***************************************/

/**********************************
*      Generated Code
**********************************/
`$CommandsDefines`

/**********************************
*     End Of Generated Code
**********************************/

/* These are command priperties */
#define `$INSTANCE_NAME`_CMD_PROP_WRITE_NONE            (0x00u)
#define `$INSTANCE_NAME`_CMD_PROP_WRITE_MANUAL          (0x01u)
#define `$INSTANCE_NAME`_CMD_PROP_WRITE_AUTO            (0x02u)
#define `$INSTANCE_NAME`_CMD_PROP_READ_NONE             (0x00u)
#define `$INSTANCE_NAME`_CMD_PROP_READ_MANUAL           (0x04u)
#define `$INSTANCE_NAME`_CMD_PROP_READ_AUTO             (0x08u)
#define `$INSTANCE_NAME`_CMD_PROP_PAGE_NONE             (0x00u)
#define `$INSTANCE_NAME`_CMD_PROP_PAGE_INDEXED          (0x10u)
#define `$INSTANCE_NAME`_CMD_PROP_FORMAT_NON_NUMERIC    (0x00u)
#define `$INSTANCE_NAME`_CMD_PROP_FORMAT_LINEAR         (0x20u)
#define `$INSTANCE_NAME`_CMD_PROP_FORMAT_SIGNED         (0x40u)
#define `$INSTANCE_NAME`_CMD_PROP_FORMAT_DIRECT         (0x60u)
#define `$INSTANCE_NAME`_CMD_PROP_FORMAT_UNSIGNED       (0x80u)
#define `$INSTANCE_NAME`_CMD_PROP_FORMAT_VID_MODE       (0xA0u)

/* These are masks for command priperties */
#define `$INSTANCE_NAME`_CMD_PROP_WRITE_MASK            (0x03u)
#define `$INSTANCE_NAME`_CMD_PROP_READ_MASK             (0x0Cu)
#define `$INSTANCE_NAME`_CMD_PROP_PAGE_MASK             (0x10u)
#define `$INSTANCE_NAME`_CMD_PROP_FORMAT_MASK           (0xE0u)


/* Command validness indicators */
#define `$INSTANCE_NAME`_COMMAND_VALID                  (0x01u)
#define `$INSTANCE_NAME`_COMMAND_INVALID                (0x00u)

/* Command validness indicators */
#define `$INSTANCE_NAME`_CMD_ALL_PAGES                  (0xFFu)

/* A flag to indicate that in current transaction a command was receved */
#define `$INSTANCE_NAME`_COMMAND_RECEIVED               (0x01u)

#define `$INSTANCE_NAME`_CMD_UNDEFINED                  (0xFFu)

/* Defines used for QUERY command */
#define `$INSTANCE_NAME`_QRY_CMD_SUPPORTED              (0x80u)
#define `$INSTANCE_NAME`_QRY_CMD_WR_SUPPORTED           (0x40u)
#define `$INSTANCE_NAME`_QRY_CMD_RD_SUPPORTED           (0x20u)
#define `$INSTANCE_NAME`_QRY_FORMAT_LINEAR              (0x00u)
#define `$INSTANCE_NAME`_QRY_FORMAT_SIGNED16            (0x04u)
#define `$INSTANCE_NAME`_QRY_FORMAT_RESERVED            (0x08u)
#define `$INSTANCE_NAME`_QRY_FORMAT_DIRECT              (0x0Cu)
#define `$INSTANCE_NAME`_QRY_FORMAT_UNSIGNED8           (0x10u)
#define `$INSTANCE_NAME`_QRY_FORMAT_VID_MODE            (0x14u)
#define `$INSTANCE_NAME`_QRY_FORMAT_SPECIFIC            (0x18u)
#define `$INSTANCE_NAME`_QRY_FORMAT_NON_NUMERIC         (0x1Cu)

#define `$INSTANCE_NAME`_QRY_FORMAT_MASK                (0x1Cu)


/* [] END OF FILE */
