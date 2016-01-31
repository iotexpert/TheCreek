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
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(`$INSTANCE_NAME`_H) /* FILT Header File */
#define `$INSTANCE_NAME`_H

#include "cyfitter.h"
#include "CyLib.h"


/***************************************
*     Data Struct Definition
***************************************/

/* Low power Mode API Support */
typedef struct _`$INSTANCE_NAME`_backupStruct 
{
    uint8 enableState;
    uint8 cr;
    uint8 sr;
    uint8 sema;
}   `$INSTANCE_NAME`_BACKUP_STRUCT;


/***************************************
* FILT component function prototypes.
****************************************/

void   `$INSTANCE_NAME`_Start(void) `=ReentrantKeil($INSTANCE_NAME . "_Start")`;
void   `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`;
uint8 `$INSTANCE_NAME`_Read8(uint8 channel) `=ReentrantKeil($INSTANCE_NAME . "_Read8")`;
uint16 `$INSTANCE_NAME`_Read16(uint8 channel) `=ReentrantKeil($INSTANCE_NAME . "_Read16")`;
uint32 `$INSTANCE_NAME`_Read24(uint8 channel) `=ReentrantKeil($INSTANCE_NAME . "_Read24")`;
void `$INSTANCE_NAME`_Write8(uint8 channel, uint8 sample) `=ReentrantKeil($INSTANCE_NAME . "_Write8")`;
void `$INSTANCE_NAME`_Write16(uint8 channel, uint16 sample) `=ReentrantKeil($INSTANCE_NAME . "_Write16")`;
void `$INSTANCE_NAME`_Write24(uint8 channel, uint32 sample) `=ReentrantKeil($INSTANCE_NAME . "_Write24")`;
void `$INSTANCE_NAME`_Sleep(void) `=ReentrantKeil($INSTANCE_NAME . "_Sleep")`;
void `$INSTANCE_NAME`_Wakeup(void) `=ReentrantKeil($INSTANCE_NAME . "_Wakeup")`;
void `$INSTANCE_NAME`_SaveConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_SaveConfig")`;
void `$INSTANCE_NAME`_RestoreConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_RestoreConfig")`;
void `$INSTANCE_NAME`_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_Init")`;
void `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`;
void `$INSTANCE_NAME`_SetCoherency(uint8 channel, uint8 byteSelect) `=ReentrantKeil($INSTANCE_NAME . "_SetCoherency")`;
void `$INSTANCE_NAME`_RestoreACURam(void) `=ReentrantKeil($INSTANCE_NAME . "_RestoreACURam")`;


/*****************************************
* FILT component API Constants.
******************************************/

/* Channel Definitions */
#define `$INSTANCE_NAME`_CHANNEL_A             (0u)
#define `$INSTANCE_NAME`_CHANNEL_B             (1u)

#define `$INSTANCE_NAME`_CHANNEL_A_INTR        (0x08u)
#define `$INSTANCE_NAME`_CHANNEL_B_INTR        (0x10u)

#define `$INSTANCE_NAME`_ALL_INTR              (0xf8u)

#define `$INSTANCE_NAME`_SIGN_BIT              (0x00800000u)
#define `$INSTANCE_NAME`_SIGN_BYTE             (0xFF000000u)

/* Component's enable/disable state */
#define `$INSTANCE_NAME`_ENABLED               (0x01u)
#define `$INSTANCE_NAME`_DISABLED              (0x00u)

/* SetCoherency API constants */
#define `$INSTANCE_NAME`_KEY_LOW               (0x00u)
#define `$INSTANCE_NAME`_KEY_MID               (0x01u)
#define `$INSTANCE_NAME`_KEY_HIGH              (0x02u)


/*******************************************************************************
* FILT component macros.
*******************************************************************************/

#define `$INSTANCE_NAME`_ClearInterruptSource() \
   (`$INSTANCE_NAME`_SR_REG = `$INSTANCE_NAME`_ALL_INTR )

#define `$INSTANCE_NAME`_IsInterruptChannelA() \
    (`$INSTANCE_NAME`_SR_REG & `$INSTANCE_NAME`_CHANNEL_A_INTR)

#define `$INSTANCE_NAME`_IsInterruptChannelB() \
    (`$INSTANCE_NAME`_SR_REG & `$INSTANCE_NAME`_CHANNEL_B_INTR)


/*******************************************************************************
* FILT component DFB registers.
*******************************************************************************/

/* DFB Status register */
#define `$INSTANCE_NAME`_SR_REG             (* (reg8 *) `$INSTANCE_NAME`_DFB__SR)
#define `$INSTANCE_NAME`_SR_PTR             (  (reg8 *) `$INSTANCE_NAME`_DFB__SR)

/* DFB Control register */
#define `$INSTANCE_NAME`_CR_REG             (* (reg8 *) `$INSTANCE_NAME`_DFB__CR)
#define `$INSTANCE_NAME`_CR_PTR             (  (reg8 *) `$INSTANCE_NAME`_DFB__CR)


/*******************************************************************************
* DFB.COHER bit field defines.
*******************************************************************************/

/* STAGEA key coherency mask */
#define `$INSTANCE_NAME`_STAGEA_COHER_MASK    0x03u

/* HOLDA key coherency mask */
#define `$INSTANCE_NAME`_HOLDA_COHER_MASK    (0x03u << 4)

/* STAGEB key coherency mask */
#define `$INSTANCE_NAME`_STAGEB_COHER_MASK    0x0Cu

/* HOLDB key coherency mask */
#define `$INSTANCE_NAME`_HOLDB_COHER_MASK    (0x0Cu << 4)

#endif /* End FILT Header File */


/* [] END OF FILE */
