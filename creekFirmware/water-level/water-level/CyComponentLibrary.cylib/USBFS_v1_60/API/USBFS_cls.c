/*******************************************************************************
* File Name: `$INSTANCE_NAME`_cls.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  USB Class request handler.
*
* Note:
*
********************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/

#include "`$INSTANCE_NAME`.h"

#if(`$INSTANCE_NAME`_EXTERN_CLS == `$INSTANCE_NAME`_FALSE)


/***************************************
* External references
***************************************/

#if defined(`$INSTANCE_NAME`_ENABLE_HID_CLASS)
    uint8 `$INSTANCE_NAME`_DispatchHIDClassRqst(void);
#endif /* End `$INSTANCE_NAME`_ENABLE_HID_CLASS */

#if defined(`$INSTANCE_NAME`_ENABLE_AUDIO_CLASS)
    uint8 `$INSTANCE_NAME`_DispatchAUDIOClassRqst(void);
#endif /* End `$INSTANCE_NAME`_ENABLE_HID_CLASS */


/***************************************
* User Implemented Class Driver Declarations.
***************************************/
/* `#START USER_DEFINED_CLASS_DECLARATIONS` Place your declaration here */

/* `#END` */


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_DispatchClassRqst
********************************************************************************
* Summary:
*  This routine dispatches class requests
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
uint8 `$INSTANCE_NAME`_DispatchClassRqst()
{
    uint8 requestHandled = `$INSTANCE_NAME`_FALSE;
    
    #if defined(`$INSTANCE_NAME`_ENABLE_HID_CLASS)
        requestHandled = `$INSTANCE_NAME`_DispatchHIDClassRqst();
    #endif /* `$INSTANCE_NAME`_ENABLE_HID_CLASS */

    #if defined(`$INSTANCE_NAME`_ENABLE_AUDIO_CLASS)
        if(requestHandled == `$INSTANCE_NAME`_FALSE)
        {
            requestHandled = `$INSTANCE_NAME`_DispatchAUDIOClassRqst();
        }    
    #endif /* `$INSTANCE_NAME`_ENABLE_HID_CLASS */

    /* `#START USER_DEFINED_CLASS_CODE` Place your Class request here */
    
    /* `#END` */
    
    return(requestHandled);
}


/*******************************************************************************
* Additional user functions supporting Class Specific Requests
********************************************************************************/

/* `#START CLASS_SPECIFIC_FUNCTIONS` Place any additional functions here */

/* `#END` */

#endif /* `$INSTANCE_NAME`_EXTERN_CLS */


/* [] END OF FILE */
