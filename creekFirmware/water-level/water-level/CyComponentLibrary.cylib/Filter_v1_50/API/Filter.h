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
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(`$INSTANCE_NAME`_H) /* FILT Header File */
#define `$INSTANCE_NAME`_H

#include "cyfitter.h"
#include "CyLib.h"


/***************************************
* Conditional Compilation Parameters
***************************************/

/* PSoC3 ES2 or early */
#define `$INSTANCE_NAME`_PSOC3_ES2  ((CYDEV_CHIP_MEMBER_USED == CYDEV_CHIP_MEMBER_3A)    && \
                                    (CYDEV_CHIP_REVISION_USED <= CYDEV_CHIP_REVISION_3A_ES2))
/* PSoC5 ES1 or early */
#define `$INSTANCE_NAME`_PSOC5_ES1  ((CYDEV_CHIP_MEMBER_USED == CYDEV_CHIP_MEMBER_5A)    && \
                                    (CYDEV_CHIP_REVISION_USED <= CYDEV_CHIP_REVISION_5A_ES1))
/* PSoC3 ES3 or later */
#define `$INSTANCE_NAME`_PSOC3_ES3  ((CYDEV_CHIP_MEMBER_USED == CYDEV_CHIP_MEMBER_3A)    && \
                                    (CYDEV_CHIP_REVISION_USED > CYDEV_CHIP_REVISION_3A_ES2))
/* PSoC5 ES2 or later */
#define `$INSTANCE_NAME`_PSOC5_ES2  ((CYDEV_CHIP_MEMBER_USED == CYDEV_CHIP_MEMBER_5A)    && \
                                    (CYDEV_CHIP_REVISION_USED > CYDEV_CHIP_REVISION_5A_ES1))


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
    uint8 acu_ram[64];
}   `$INSTANCE_NAME`_BACKUP_STRUCT;


/***************************************
* FILT component function prototypes.
****************************************/

void   `$INSTANCE_NAME`_Start(void);
void   `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`;
uint8 `$INSTANCE_NAME`_Read8(uint8 channel) `=ReentrantKeil($INSTANCE_NAME . "_Read8")`;
uint16 `$INSTANCE_NAME`_Read16(uint8 channel) `=ReentrantKeil($INSTANCE_NAME . "_Read16")`;
uint32 `$INSTANCE_NAME`_Read24(uint8 channel) `=ReentrantKeil($INSTANCE_NAME . "_Read24")`;
void `$INSTANCE_NAME`_Write8(uint8 channel, uint8 sample) `=ReentrantKeil($INSTANCE_NAME . "_Write8")`;
void `$INSTANCE_NAME`_Write16(uint8 channel, uint16 sample) `=ReentrantKeil($INSTANCE_NAME . "_Write16")`;
void `$INSTANCE_NAME`_Write24(uint8 channel, uint32 sample) `=ReentrantKeil($INSTANCE_NAME . "_Write24")`;
void `$INSTANCE_NAME`_Sleep(void);
void `$INSTANCE_NAME`_Wakeup(void) `=ReentrantKeil($INSTANCE_NAME . "_Wakeup")`;
void `$INSTANCE_NAME`_SaveConfig(void);
void `$INSTANCE_NAME`_RestoreConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_RestoreConfig")`;
void `$INSTANCE_NAME`_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_Init")`;
void `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`;


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


/*******************************************************************************
* FILT component macros.
*******************************************************************************/

#define `$INSTANCE_NAME`_ClearInterruptSource() \
    do { \
    `$INSTANCE_NAME`_SR_REG = `$INSTANCE_NAME`_ALL_INTR; \
    } while (0)

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

#endif /* End FILT Header File */


/* [] END OF FILE */
