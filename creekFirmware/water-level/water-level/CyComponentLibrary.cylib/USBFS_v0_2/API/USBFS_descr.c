/*******************************************************************************
* File Name: `$INSTANCE_NAME`_descr.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*    USB descriptors and storage.
*
*   Note:
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#include "cydevice.h"
#include "cyfitter.h"
#include "`$INSTANCE_NAME`.h"

/*******************************************************************************
*  User supplied descriptors.  If you want to specify your own descriptors,
*  remove the comments around the define USER_SUPPLIED_DESCRIPTORS below and
*  add your descriptors.
********************************************************************************/
/* `#START USER_DESCRIPTORS_DECLARATIONS` Place your declaration here */
/* `#END` */
/*******************************************************************************
*  USB Customizer Generated Descriptors
********************************************************************************/
#if !defined(USER_SUPPLIED_DESCRIPTORS)
`$APIGEN_DEVICE_DESCRIPTORS`
`$APIGEN_STRING_DESCRIPTORS`
`$APIGEN_SN_DESCRIPTOR`
`$APIGEN_EE_DESCRIPTOR`
`$APIGEN_HIDREPORT_DESCRIPTORS`
`$APIGEN_HIDREPORT_TABLES`
`$APIGEN_DEVICE_TABLES`
#endif
