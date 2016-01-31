/*******************************************************************************
* File Name: `$INSTANCE_NAME`_vnd.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  USB vendor request handler.
*
* Note:
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"

#if(`$INSTANCE_NAME`_EXTERN_VND == `$INSTANCE_NAME`_FALSE)


/***************************************
* Vendor Specific Declarations
***************************************/

/* `#START VENDOR_SPECIFIC_DECLARATIONS` Place your declaration here */

/* `#END` */


/***************************************
* External References
***************************************/

uint8 `$INSTANCE_NAME`_InitControlRead(void) `=ReentrantKeil($INSTANCE_NAME . "_InitControlRead")`;
uint8 `$INSTANCE_NAME`_InitControlWrite(void) `=ReentrantKeil($INSTANCE_NAME . "_InitControlWrite")`;


extern uint8 CYCODE `$INSTANCE_NAME`_MSOS_CONFIGURATION_DESCR[];

extern volatile T_`$INSTANCE_NAME`_TD `$INSTANCE_NAME`_currentTD;


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_HandleVendorRqst
********************************************************************************
*
* Summary:
*  This routine provide users with a method to implement vendor specifc
*  requests.
*
*  To implement vendor specific requests, add your code in this function to
*  decode and disposition the request.  If the request is handled, your code
*  must set the variable "requestHandled" to TRUE, indicating that the
*  request has been handled.
*
* Parameters:
*  None.
*
* Return:
*  requestHandled.
*
* Reentrant:
*  No.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_HandleVendorRqst(void) `=ReentrantKeil($INSTANCE_NAME . "_HandleVendorRqst")`
{
    uint8 requestHandled = `$INSTANCE_NAME`_FALSE;

    if ((CY_GET_REG8(`$INSTANCE_NAME`_bmRequestType) & `$INSTANCE_NAME`_RQST_DIR_MASK) == `$INSTANCE_NAME`_RQST_DIR_D2H)
    {
        /* Control Read */
        switch (CY_GET_REG8(`$INSTANCE_NAME`_bRequest))
        {
            case `$INSTANCE_NAME`_GET_EXTENDED_CONFIG_DESCRIPTOR:
                #if defined(`$INSTANCE_NAME`_ENABLE_MSOS_STRING)
                    `$INSTANCE_NAME`_currentTD.pData = &`$INSTANCE_NAME`_MSOS_CONFIGURATION_DESCR[0u];
                    `$INSTANCE_NAME`_currentTD.count = `$INSTANCE_NAME`_MSOS_CONFIGURATION_DESCR[0u];
                    requestHandled  = `$INSTANCE_NAME`_InitControlRead();
                #endif /* End `$INSTANCE_NAME`_ENABLE_MSOS_STRING */
                break;
            default:
                break;
        }
    }

    /* `#START VENDOR_SPECIFIC_CODE` Place your vendor specific request here */

    /* `#END` */

    return(requestHandled);
}


/*******************************************************************************
* Additional user functions supporting Vendor Specific Requests
********************************************************************************/

/* `#START VENDOR_SPECIFIC_FUNCTIONS` Place any additional functions here */

/* `#END` */

#endif /* `$INSTANCE_NAME`_EXTERN_VND */


/* [] END OF FILE */
