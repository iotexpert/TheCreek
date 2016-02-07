/*******************************************************************************
* File Name: idac.h
* Version 1.0
*
* Description:
*  This file provides constants and parameter values for the IDAC_P4
*  component.
*
********************************************************************************
* Copyright 2013, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(CY_IDAC_idac_H)
#define CY_IDAC_idac_H

#include "cytypes.h"
#include "cyfitter.h"
#include "CyLib.h"


/***************************************
* Internal Type defines
***************************************/

/* Structure to save state before go to sleep */
typedef struct
{
    uint8  enableState;
} idac_BACKUP_STRUCT;


extern uint32 idac_initVar;


/***************************************
*   Conditional Compilation Parameters
****************************************/

#define idac_IDAC_RESOLUTION    (8u)
#define idac_IDAC_RANGE         (0u)
#define idac_IDAC_POLARITY      (0u)


/***************************************
*    Initial Parameter Constants
***************************************/
#define idac_IDAC_INIT_VALUE    (128u)




/***************************************
*        Function Prototypes
***************************************/

void idac_Init(void);
void idac_Enable(void);
void idac_Start(void);
void idac_Stop(void);
void idac_SetValue(uint32  value);
void idac_SaveConfig(void);
void idac_Sleep(void);
void idac_RestoreConfig(void);
void idac_Wakeup(void);


/***************************************
*            API Constants
***************************************/

#define idac_IDAC_EN_MODE              (3u)
#define idac_IDAC_CSD_EN               (1u)
#define idac_IDAC_CSD_EN_POSITION      (31u)
#define idac_IDAC_VALUE_POSITION       (idac_cy_psoc4_idac__CSD_IDAC_SHIFT)
#define idac_IDAC_MODE_SHIFT           (8u)
#define idac_IDAC_POLARITY_POSITION    (idac_cy_psoc4_idac__POLARITY_SHIFT)
#define idac_IDAC_MODE_POSITION        ((uint32)idac_cy_psoc4_idac__CSD_IDAC_SHIFT + \
                                                        (uint32)idac_IDAC_MODE_SHIFT)
#define idac_IDAC_RANGE_SHIFT          (10u)
#define idac_IDAC_RANGE_POSITION       ((uint32)idac_cy_psoc4_idac__CSD_IDAC_SHIFT + \
                                                        (uint32)idac_IDAC_RANGE_SHIFT)

#define idac_IDAC_CDS_EN_MASK      (0x80000000u)

#if(idac_IDAC_RESOLUTION == 8u)
  #define idac_IDAC_VALUE_MASK     (0xFFu)
#else
  #define idac_IDAC_VALUE_MASK     (0x7Fu)
#endif /* (idac_IDAC_RESOLUTION == 8u) */

#define idac_IDAC_MODE_MASK        (3u)
#define idac_IDAC_RANGE_MASK       (1u)
#define idac_IDAC_POLARITY_MASK    (1u)


/***************************************
*        Registers
***************************************/

#define idac_IDAC_CONTROL_REG    (*(reg32 *) (uint32)CYREG_CSD_IDAC)
#define idac_IDAC_CONTROL_PTR    ( (reg32 *) CYREG_CSD_IDAC)

#define idac_IDAC_POLARITY_CONTROL_REG    (*(reg32 *) CYREG_CSD_CONFIG)
#define idac_IDAC_POLARITY_CONTROL_PTR    ( (reg32 *) CYREG_CSD_CONFIG)

#endif /* CY_IDAC_idac_H */

/* [] END OF FILE */
