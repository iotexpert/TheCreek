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



#include "cydevice_trm.h"
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

extern uint8 `$INSTANCE_NAME`_CODE `$INSTANCE_NAME`_MSOS_CONFIGURATION_DESCR[];

extern T_`$INSTANCE_NAME`_TD CurrentTD;

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
	
   if ((CY_GET_REG8(`$INSTANCE_NAME`_bmRequestType) & `$INSTANCE_NAME`_RQST_DIR_MASK) == `$INSTANCE_NAME`_RQST_DIR_D2H)
    {
        /* Control Read */
        switch (CY_GET_REG8(`$INSTANCE_NAME`_bRequest)) 
        {
        case `$INSTANCE_NAME`_GET_CONFIG_DESCRIPTOR:
		  #if defined(`$INSTANCE_NAME`_ENABLE_MSOS_STRING)
                CurrentTD.pData     = &`$INSTANCE_NAME`_MSOS_CONFIGURATION_DESCR[0];
                CurrentTD.wCount    = `$INSTANCE_NAME`_MSOS_CONFIGURATION_DESCR[0];
                bRequestHandled     = `$INSTANCE_NAME`_InitControlRead();		
		  #endif		
            break;
        default:
            break;
        }
    }		
	
/* `#START VENDOR_SPECIFIC_CODE` Place your vendor specific request here */
  
/* `#END` */
    return bRequestHandled;   
}
/*******************************************************************************
* Additional user functions supporting Vendor Specific Requests
********************************************************************************/
/* `#START VENDOR_SPECIFIC_FUNCTIONS` Place any additional functions here */
    
    
/* `#END` */


