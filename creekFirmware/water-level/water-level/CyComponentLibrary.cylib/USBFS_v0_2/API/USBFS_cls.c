/*******************************************************************************
* File Name: `$INSTANCE_NAME`_cls.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*    USB Class request handler.
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
* External references
********************************************************************************/
#if defined(`$INSTANCE_NAME`_ENABLE_HID_CLASS)
uint8 `$INSTANCE_NAME`_DispatchHIDClassRqst(void);
#endif
/*******************************************************************************
* User Implemented Class Driver Declarations.
********************************************************************************/
/* `#START USER_DEFINED_CLASS_DECLARATIONS` Place your declaration here */
/* `#END` */

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_DispatchClassRqst
********************************************************************************
* Summary:
*   This routine dispatches class requests
*   
* Parameters:  
*   None
*******************************************************************************/
uint8 `$INSTANCE_NAME`_DispatchClassRqst()
{
	uint8 bRequestHandled = FALSE;
/* TODO: Dispatch requests based on the interface number */
#if defined(`$INSTANCE_NAME`_ENABLE_HID_CLASS)
    bRequestHandled = `$INSTANCE_NAME`_DispatchHIDClassRqst();
#endif
    return bRequestHandled;   
}
