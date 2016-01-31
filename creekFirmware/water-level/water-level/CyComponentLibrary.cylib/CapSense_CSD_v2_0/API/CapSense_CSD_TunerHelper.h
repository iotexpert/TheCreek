/*******************************************************************************
* File Name: `$INSTANCE_NAME`_TunerHelper.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides constants and structure declarations for the tunner hepl
*  APIs for CapSense CSD component.
*
* Note:
*
********************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(CY_CAPSENSE_CSD_TUNERHELPER_`$INSTANCE_NAME`_H)
#define CY_CAPSENSE_CSD_TUNERHELPER_`$INSTANCE_NAME`_H

#include "`$INSTANCE_NAME`.h"
#include "`$INSTANCE_NAME`_CSHL.h"
#if ((`$INSTANCE_NAME`_TUNER_API_GENERATE) && (`$INSTANCE_NAME`_TUNNING_METHOD != `$INSTANCE_NAME`_NO_TUNNING))
    #include "`$INSTANCE_NAME`_MBX.h"
    #include "`$EzI2CInstanceName`.h"
#endif /* End (`$INSTANCE_NAME`_TUNER_API_GENERATE) */


/***************************************
*     Constants for mailboxes
***************************************/

#define `$INSTANCE_NAME`_DEFAULT_MAILBOXES_NUMBER   (1u)


/***************************************
*        Function Prototypes
***************************************/

void `$INSTANCE_NAME`_TunerStart(void);
void `$INSTANCE_NAME`_TunerComm(void);


#endif  /* End (CY_CAPSENSE_CSD_TUNERHELPER_`$INSTANCE_NAME`_H)*/


/* [] END OF FILE */
