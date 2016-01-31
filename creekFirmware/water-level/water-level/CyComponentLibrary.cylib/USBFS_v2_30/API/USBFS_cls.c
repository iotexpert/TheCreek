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
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

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
#if defined(`$INSTANCE_NAME`_ENABLE_CDC_CLASS)
    uint8 `$INSTANCE_NAME`_DispatchCDCClassRqst(void);
#endif /* End `$INSTANCE_NAME`_ENABLE_CDC_CLASS */

extern uint8 CYCODE *`$INSTANCE_NAME`_interfaceClass;
extern volatile T_`$INSTANCE_NAME`_EP_CTL_BLOCK `$INSTANCE_NAME`_EP[];


/***************************************
* User Implemented Class Driver Declarations.
***************************************/
/* `#START USER_DEFINED_CLASS_DECLARATIONS` Place your declaration here */

/* `#END` */


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_DispatchClassRqst
********************************************************************************
* Summary:
*  This routine dispatches class specific requests depend on inteface class.
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
uint8 `$INSTANCE_NAME`_DispatchClassRqst() `=ReentrantKeil($INSTANCE_NAME . "_DispatchClassRqst")`
{
    uint8 requestHandled = `$INSTANCE_NAME`_FALSE;
    uint8 interfaceNumber = 0u;

    switch(CY_GET_REG8(`$INSTANCE_NAME`_bmRequestType) & `$INSTANCE_NAME`_RQST_RCPT_MASK)
    {
        case `$INSTANCE_NAME`_RQST_RCPT_IFC:        /* class-specific request directed to an interface */
            interfaceNumber = CY_GET_REG8(`$INSTANCE_NAME`_wIndexLo); /* wIndexLo contain Interface number */
            break;
        case `$INSTANCE_NAME`_RQST_RCPT_EP:         /* class-specific request directed to the endpoint */
            /* find related intenface to the endpoint, wIndexLo contain EP number */
            interfaceNumber =
                `$INSTANCE_NAME`_EP[CY_GET_REG8(`$INSTANCE_NAME`_wIndexLo) & `$INSTANCE_NAME`_DIR_UNUSED].interface;
            break;
        default:    /* requestHandled is initialized as FALSE by default */
            break;
    }
    /* Handle Class request depend on interface type */
    switch(`$INSTANCE_NAME`_interfaceClass[interfaceNumber])
    {
        case `$INSTANCE_NAME`_CLASS_HID:
            #if defined(`$INSTANCE_NAME`_ENABLE_HID_CLASS)
                requestHandled = `$INSTANCE_NAME`_DispatchHIDClassRqst();
            #endif /* `$INSTANCE_NAME`_ENABLE_HID_CLASS */
            break;
        case `$INSTANCE_NAME`_CLASS_AUDIO:
            #if defined(`$INSTANCE_NAME`_ENABLE_AUDIO_CLASS)
                requestHandled = `$INSTANCE_NAME`_DispatchAUDIOClassRqst();
            #endif /* `$INSTANCE_NAME`_ENABLE_HID_CLASS */
            break;
        case `$INSTANCE_NAME`_CLASS_CDC:
            #if defined(`$INSTANCE_NAME`_ENABLE_CDC_CLASS)
                requestHandled = `$INSTANCE_NAME`_DispatchCDCClassRqst();
            #endif /* `$INSTANCE_NAME`_ENABLE_CDC_CLASS */
            break;
        default:    /* requestHandled is initialezed as FALSE by default */
            break;
    }

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
