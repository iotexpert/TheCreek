/*******************************************************************************
* File Name: `$INSTANCE_NAME`_vnd.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*    USB vendor request handler.
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
* Vendor Specific Declarations
********************************************************************************/
/* `#START VENDOR_SPECIFIC_DECLARATIONS` Place your declaration here */
    
    
/* `#END` */
/*******************************************************************************
* External References
********************************************************************************/
uint8 `$INSTANCE_NAME`_InitControlRead(void);
uint8 `$INSTANCE_NAME`_InitControlWrite(void);
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_HandleVendorRqst
********************************************************************************
* Summary:
*   This routine provide users with a method to implement vendor specifc
*   requests.
*
*   To implement vendor specific requests, add your code in this function to
*   decode and disposition the request.  If the request is handled, your code
*   must set the variable "bRequestHandled" to TRUE, indicating that the 
*   request has been handled.
*   
* Parameters:  
*   None
*******************************************************************************/
uint8 `$INSTANCE_NAME`_HandleVendorRqst()
{
	uint8 bRequestHandled = FALSE;
/* `#START VENDOR_SPECIFIC_CODE` Place your vendor specific request here */
    
    
/* `#END` */
    return bRequestHandled;   
}
/*******************************************************************************
* Additional user functions supporting Vendor Specific Requests
********************************************************************************/
/* `#START VENDOR_SPECIFIC_FUNCTIONS` Place any additional functions here */
    
    
/* `#END` */


