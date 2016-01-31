/*******************************************************************************
* File Name: `$INSTANCE_NAME`_cdc.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  Header File for the USFS component.
*  Contains CDC class prototypes and constant values.
*
********************************************************************************
* Copyright 2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(`$INSTANCE_NAME`_cdc_H)
#define `$INSTANCE_NAME`_cdc_H

#include "cytypes.h"


/***************************************
*  Memory Union Definitions
***************************************/
/*Pointer type conversions are restricted to avoid conflicts with compiler
optimizations. */
typedef union _`$INSTANCE_NAME`_ConvertDword
{
    volatile uint8 bytes[4];
    uint32 dword;
} `$INSTANCE_NAME`_CONVERT_DWORD;


/***************************************
* Prototypes of the $INSTANCE_NAME` API.
***************************************/

#if (`$INSTANCE_NAME`_ENABLE_CDC_CLASS_API != 0u)
    void `$INSTANCE_NAME`_CDC_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_CDC_Init")`;
    void `$INSTANCE_NAME`_PutData(uint8* pData, uint16 length) `=ReentrantKeil($INSTANCE_NAME . "_PutData")`;
    void `$INSTANCE_NAME`_PutString(char8* string) `=ReentrantKeil($INSTANCE_NAME . "_PutString")`;
    void `$INSTANCE_NAME`_PutChar(char8 txDataByte) `=ReentrantKeil($INSTANCE_NAME . "_PutChar")`;
    void `$INSTANCE_NAME`_PutCRLF(void) `=ReentrantKeil($INSTANCE_NAME . "_PutCRLF")`;
    uint16 `$INSTANCE_NAME`_GetCount(void) `=ReentrantKeil($INSTANCE_NAME . "_GetCount")`;
    uint8 `$INSTANCE_NAME`_CDCIsReady(void) `=ReentrantKeil($INSTANCE_NAME . "_CDCIsReady")`;
    uint8 `$INSTANCE_NAME`_DataIsReady(void) `=ReentrantKeil($INSTANCE_NAME . "_DataIsReady")`;
    uint16 `$INSTANCE_NAME`_GetData(uint8* pData, uint16 length) `=ReentrantKeil($INSTANCE_NAME . "_GetData")`;
    uint16 `$INSTANCE_NAME`_GetAll(uint8* pData) `=ReentrantKeil($INSTANCE_NAME . "_GetAll")`;
    uint8 `$INSTANCE_NAME`_GetChar(void) `=ReentrantKeil($INSTANCE_NAME . "_GetChar")`;
    uint8 `$INSTANCE_NAME`_IsLineChanged(void) `=ReentrantKeil($INSTANCE_NAME . "_IsLineChanged")`;
    uint32 `$INSTANCE_NAME`_GetDTERate(void) `=ReentrantKeil($INSTANCE_NAME . "_GetDTERate")`;
    uint8 `$INSTANCE_NAME`_GetCharFormat(void) `=ReentrantKeil($INSTANCE_NAME . "_GetCharFormat")`;
    uint8 `$INSTANCE_NAME`_GetParityType(void) `=ReentrantKeil($INSTANCE_NAME . "_GetParityType")`;
    uint8 `$INSTANCE_NAME`_GetDataBits(void) `=ReentrantKeil($INSTANCE_NAME . "_GetDataBits")`;
    uint16 `$INSTANCE_NAME`_GetLineControl(void) `=ReentrantKeil($INSTANCE_NAME . "_GetLineControl")`;
#endif  /* End `$INSTANCE_NAME`_ENABLE_CDC_CLASS_API*/


/***************************************
*  Constants for $INSTANCE_NAME` API.
***************************************/

/* CDC Class-Specific Request Codes (CDC ver 1.2 Table 19) */
#define `$INSTANCE_NAME`_CDC_SET_LINE_CODING        (0x20u)
#define `$INSTANCE_NAME`_CDC_GET_LINE_CODING        (0x21u)
#define `$INSTANCE_NAME`_CDC_SET_CONTROL_LINE_STATE (0x22u)

#define `$INSTANCE_NAME`_LINE_CODING_CHANGED        (0x01u)
#define `$INSTANCE_NAME`_LINE_CONTROL_CHANGED       (0x02u)

#define `$INSTANCE_NAME`_1_STOPBIT                  (0x00u)
#define `$INSTANCE_NAME`_1_5_STOPBITS               (0x01u)
#define `$INSTANCE_NAME`_2_STOPBITS                 (0x02u)

#define `$INSTANCE_NAME`_PARITY_NONE                (0x00u)
#define `$INSTANCE_NAME`_PARITY_ODD                 (0x01u)
#define `$INSTANCE_NAME`_PARITY_EVEN                (0x02u)
#define `$INSTANCE_NAME`_PARITY_MARK                (0x03u)
#define `$INSTANCE_NAME`_PARITY_SPACE               (0x04u)

#define `$INSTANCE_NAME`_LINE_CODING_SIZE           (0x07u)
#define `$INSTANCE_NAME`_LINE_CODING_RATE           (0x00u)
#define `$INSTANCE_NAME`_LINE_CODING_STOP_BITS      (0x04u)
#define `$INSTANCE_NAME`_LINE_CODING_PARITY         (0x05u)
#define `$INSTANCE_NAME`_LINE_CODING_DATA_BITS      (0x06u)

#define `$INSTANCE_NAME`_LINE_CONTROL_DTR           (0x01u)
#define `$INSTANCE_NAME`_LINE_CONTROL_RTS           (0x02u)

#endif /* End `$INSTANCE_NAME`_cdc_H */


/* [] END OF FILE */
