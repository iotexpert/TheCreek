/*******************************************************************************
* File Name: `$INSTANCE_NAME`_hid.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  USB HID Class request handler.
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

#if defined(`$INSTANCE_NAME`_ENABLE_HID_CLASS)

#include "`$INSTANCE_NAME`_hid.h"


/***************************************
*    HID Variables
***************************************/

volatile uint8 `$INSTANCE_NAME`_hidProtocol[`$INSTANCE_NAME`_MAX_INTERFACES_NUMBER];  /* HID device protocol status */
volatile uint8 `$INSTANCE_NAME`_hidIdleRate[`$INSTANCE_NAME`_MAX_INTERFACES_NUMBER];  /* HID device idle reload value */
volatile uint8 `$INSTANCE_NAME`_hidIdleTimer[`$INSTANCE_NAME`_MAX_INTERFACES_NUMBER]; /* HID device idle rate value */


/***************************************
* Custom Declarations
***************************************/

/* `#START HID_CUSTOM_DECLARATIONS` Place your declaration here */

/* `#END` */


/***************************************
*    External references
***************************************/

uint8 `$INSTANCE_NAME`_InitControlRead(void) `=ReentrantKeil($INSTANCE_NAME . "_InitControlRead")`;
uint8 `$INSTANCE_NAME`_InitControlWrite(void) `=ReentrantKeil($INSTANCE_NAME . "_InitControlWrite")`;
uint8 `$INSTANCE_NAME`_InitNoDataControlTransfer(void) `=ReentrantKeil($INSTANCE_NAME . "_InitNoDataControlTransfer")`;
T_`$INSTANCE_NAME`_LUT *`$INSTANCE_NAME`_GetConfigTablePtr(uint8 c)
                                                            `=ReentrantKeil($INSTANCE_NAME . "_GetConfigTablePtr")`;
T_`$INSTANCE_NAME`_LUT *`$INSTANCE_NAME`_GetDeviceTablePtr(void)
                                                            `=ReentrantKeil($INSTANCE_NAME . "_GetDeviceTablePtr")`;

extern volatile T_`$INSTANCE_NAME`_TD `$INSTANCE_NAME`_currentTD;
extern uint8 CYCODE `$INSTANCE_NAME`_HIDREPORT_DESCRIPTORS[];
extern volatile uint8 `$INSTANCE_NAME`_configuration;
extern volatile uint8 `$INSTANCE_NAME`_interfaceSetting[];


/***************************************
*    Internal references
***************************************/

void `$INSTANCE_NAME`_FindReport(void) `=ReentrantKeil($INSTANCE_NAME . "_FindReport")`;
void `$INSTANCE_NAME`_FindReportDescriptor(void) `=ReentrantKeil($INSTANCE_NAME . "_FindReportDescriptor")`;
void `$INSTANCE_NAME`_FindHidClassDecriptor(void) `=ReentrantKeil($INSTANCE_NAME . "_FindHidClassDecriptor")`;


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_UpdateHIDTimer
********************************************************************************
*
* Summary:
*  Updates the HID report timer and reloads it if expired
*
* Parameters:
*  interface:  Interface Number.
*
* Return:
*  status.
*
* Reentrant:
*  No.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_UpdateHIDTimer(uint8 interface) `=ReentrantKeil($INSTANCE_NAME . "_UpdateHIDTimer")`
{
    uint8 status = `$INSTANCE_NAME`_IDLE_TIMER_INDEFINITE;

    if(`$INSTANCE_NAME`_hidIdleRate[interface] != 0u)
    {
        if(`$INSTANCE_NAME`_hidIdleTimer[interface] > 0u)
        {
            `$INSTANCE_NAME`_hidIdleTimer[interface]--;
            status = `$INSTANCE_NAME`_IDLE_TIMER_RUNNING;
        }
        else
        {
            `$INSTANCE_NAME`_hidIdleTimer[interface] = `$INSTANCE_NAME`_hidIdleRate[interface];
            status = `$INSTANCE_NAME`_IDLE_TIMER_EXPIRED;
        }
    }

    return(status);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetProtocol
********************************************************************************
*
* Summary:
*  Returns the selected protocol value to the application
*
* Parameters:
*  interface:  Interface Number.
*
* Return:
*  Interface protocol.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_GetProtocol(uint8 interface) `=ReentrantKeil($INSTANCE_NAME . "_GetProtocol")`
{
    return(`$INSTANCE_NAME`_hidProtocol[interface]);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_DispatchHIDClassRqst
********************************************************************************
*
* Summary:
*  This routine dispatches class requests
*
* Parameters:
*  None.
*
* Return:
*  requestHandled
*
* Reentrant:
*  No.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_DispatchHIDClassRqst() `=ReentrantKeil($INSTANCE_NAME . "_DispatchHIDClassRqst")`
{
    uint8 requestHandled = `$INSTANCE_NAME`_FALSE;
    uint8 interfaceNumber;

    interfaceNumber = CY_GET_REG8(`$INSTANCE_NAME`_wIndexLo);
    if ((CY_GET_REG8(`$INSTANCE_NAME`_bmRequestType) & `$INSTANCE_NAME`_RQST_DIR_MASK) == `$INSTANCE_NAME`_RQST_DIR_D2H)
    {   /* Control Read */
        switch (CY_GET_REG8(`$INSTANCE_NAME`_bRequest))
        {
            case `$INSTANCE_NAME`_GET_DESCRIPTOR:
                if (CY_GET_REG8(`$INSTANCE_NAME`_wValueHi) == `$INSTANCE_NAME`_DESCR_HID_CLASS)
                {
                    `$INSTANCE_NAME`_FindHidClassDecriptor();
                    if (`$INSTANCE_NAME`_currentTD.count != 0u)
                    {
                        requestHandled = `$INSTANCE_NAME`_InitControlRead();
                    }
                }
                else if (CY_GET_REG8(`$INSTANCE_NAME`_wValueHi) == `$INSTANCE_NAME`_DESCR_HID_REPORT)
                {
                    `$INSTANCE_NAME`_FindReportDescriptor();
                    if (`$INSTANCE_NAME`_currentTD.count != 0u)
                    {
                        requestHandled = `$INSTANCE_NAME`_InitControlRead();
                    }
                }
                else
                {   /* requestHandled is initialezed as FALSE by default */
                }
                break;
            case `$INSTANCE_NAME`_HID_GET_REPORT:
                `$INSTANCE_NAME`_FindReport();
                if (`$INSTANCE_NAME`_currentTD.count != 0u)
                {
                    requestHandled = `$INSTANCE_NAME`_InitControlRead();
                }
                break;

            case `$INSTANCE_NAME`_HID_GET_IDLE:
                /* This function does not support multiple reports per interface*/
                /* Validate interfaceNumber and Report ID (should be 0) */
                if( (interfaceNumber < `$INSTANCE_NAME`_MAX_INTERFACES_NUMBER) &&
                    (CY_GET_REG8(`$INSTANCE_NAME`_wValueLo) == 0u ) ) /* do not support Idle per Report ID */
                {
                    `$INSTANCE_NAME`_currentTD.count = 1u;
                    `$INSTANCE_NAME`_currentTD.pData = &`$INSTANCE_NAME`_hidIdleRate[interfaceNumber];
                    requestHandled  = `$INSTANCE_NAME`_InitControlRead();
                }
                break;
            case `$INSTANCE_NAME`_HID_GET_PROTOCOL:
                /* Validate interfaceNumber */
                if( interfaceNumber < `$INSTANCE_NAME`_MAX_INTERFACES_NUMBER)
                {
                    `$INSTANCE_NAME`_currentTD.count = 1u;
                    `$INSTANCE_NAME`_currentTD.pData = &`$INSTANCE_NAME`_hidProtocol[interfaceNumber];
                    requestHandled  = `$INSTANCE_NAME`_InitControlRead();
                }
                break;
            default:    /* requestHandled is initialezed as FALSE by default */
                break;
        }
    }
    else if ((CY_GET_REG8(`$INSTANCE_NAME`_bmRequestType) & `$INSTANCE_NAME`_RQST_DIR_MASK) ==
                                                                            `$INSTANCE_NAME`_RQST_DIR_H2D)
    {   /* Control Write */
        switch (CY_GET_REG8(`$INSTANCE_NAME`_bRequest))
        {
            case `$INSTANCE_NAME`_HID_SET_REPORT:
                `$INSTANCE_NAME`_FindReport();
                if (`$INSTANCE_NAME`_currentTD.count != 0u)
                {
                    requestHandled = `$INSTANCE_NAME`_InitControlWrite();
                }
                break;
            case `$INSTANCE_NAME`_HID_SET_IDLE:
                /* This function does not support multiple reports per interface */
                /* Validate interfaceNumber and Report ID (should be 0) */
                if( (interfaceNumber < `$INSTANCE_NAME`_MAX_INTERFACES_NUMBER) &&
                    (CY_GET_REG8(`$INSTANCE_NAME`_wValueLo) == 0u ) ) /* do not support Idle per Report ID */
                {
                    `$INSTANCE_NAME`_hidIdleRate[interfaceNumber] = CY_GET_REG8(`$INSTANCE_NAME`_wValueHi);
                    /* With regards to HID spec: "7.2.4 Set_Idle Request"
                    *  Latency. If the current period has gone past the
                    *  newly proscribed time duration, then a report
                    *  will be generated immediately.
                    */
                    if(`$INSTANCE_NAME`_hidIdleRate[interfaceNumber] <
                       `$INSTANCE_NAME`_hidIdleTimer[interfaceNumber])
                    {
                        /* Set the timer to zero and let the UpdateHIDTimer() API return IDLE_TIMER_EXPIRED status*/
                        `$INSTANCE_NAME`_hidIdleTimer[interfaceNumber] = 0u;
                    }
                    /* If the new request is received within 4 milliseconds
                    *  (1 count) of the end of the current period, then the
                    *  new request will have no effect until after the report.
                    */
                    else if(`$INSTANCE_NAME`_hidIdleTimer[interfaceNumber] <= 1u)
                    {
                        /* Do nothing.
                        * Let the UpdateHIDTimer() API continue to work and
                        * return IDLE_TIMER_EXPIRED status
                        */
                    }
                    else
                    {   /* Reload the timer*/
                        `$INSTANCE_NAME`_hidIdleTimer[interfaceNumber] =
                        `$INSTANCE_NAME`_hidIdleRate[interfaceNumber];
                    }
                    requestHandled = `$INSTANCE_NAME`_InitNoDataControlTransfer();
                }
                break;

            case `$INSTANCE_NAME`_HID_SET_PROTOCOL:
                /* Validate interfaceNumber and protocol (must be 0 or 1) */
                if( (interfaceNumber < `$INSTANCE_NAME`_MAX_INTERFACES_NUMBER) &&
                    (CY_GET_REG8(`$INSTANCE_NAME`_wValueLo) <= 1u) )
                {
                    `$INSTANCE_NAME`_hidProtocol[interfaceNumber] = CY_GET_REG8(`$INSTANCE_NAME`_wValueLo);
                    requestHandled = `$INSTANCE_NAME`_InitNoDataControlTransfer();
                }
                break;
            default:    /* requestHandled is initialezed as FALSE by default */
                break;
        }
    }
    else
    {   /* requestHandled is initialezed as FALSE by default */
    }

    return(requestHandled);
}


/*******************************************************************************
* Function Name: USB_FindHidClassDescriptor
********************************************************************************
*
* Summary:
*  This routine find Hid Class Descriptor pointer based on the Interface number
*  and Alternate setting then loads the currentTD structure with the address of
*  the buffer and the size.
*  The HID Class Descriptor resides inside the config descriptor.
*
* Parameters:
*  None.
*
* Return:
*  currentTD
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_FindHidClassDecriptor() `=ReentrantKeil($INSTANCE_NAME . "_FindHidClassDecriptor")`
{
    T_`$INSTANCE_NAME`_LUT *pTmp;
    volatile uint8 *pDescr;
    uint8 interfaceN;

    pTmp = `$INSTANCE_NAME`_GetConfigTablePtr(`$INSTANCE_NAME`_configuration - 1u);
    interfaceN = CY_GET_REG8(`$INSTANCE_NAME`_wIndexLo);
    /* Third entry in the LUT starts the Interface Table pointers */
    pTmp += 2;
    /* Now use the request interface number*/
    pTmp = &pTmp[interfaceN];
    /*USB_DEVICEx_CONFIGURATIONy_INTERFACEz_TABLE*/
    pTmp = (T_`$INSTANCE_NAME`_LUT *) pTmp->p_list;
    /* Now use Alternate setting number */
    pTmp = &pTmp[`$INSTANCE_NAME`_interfaceSetting[interfaceN]];
    /*USB_DEVICEx_CONFIGURATIONy_INTERFACEz_ALTERNATEi_HID_TABLE*/
    pTmp = (T_`$INSTANCE_NAME`_LUT *) pTmp->p_list;
    /* Fifth entry in the LUT points to Hid Class Descriptor in Configuration Descriptor*/
    pTmp += 4;
    pDescr = (volatile uint8 *)pTmp->p_list;
    `$INSTANCE_NAME`_currentTD.count = pDescr[0u];
    `$INSTANCE_NAME`_currentTD.pData = pDescr;
}


/*******************************************************************************
* Function Name: USB_FindReportDescriptor
********************************************************************************
*
* Summary:
*  This routine find Hid Report Descriptor pointer based on the Interface
*  number, then loads the currentTD structure with the address of the buffer
*  and the size.
*  Hid Report Descriptor is located after IN/OUT/FEATURE reports.
*
* Parameters:
*   void
*
* Return:
*  currentTD
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_FindReportDescriptor() `=ReentrantKeil($INSTANCE_NAME . "_FindReportDescriptor")`
{
    T_`$INSTANCE_NAME`_LUT *pTmp;
    volatile uint8 *pDescr;
    uint8 interfaceN;

    pTmp = `$INSTANCE_NAME`_GetConfigTablePtr(`$INSTANCE_NAME`_configuration - 1u);
    interfaceN = CY_GET_REG8(`$INSTANCE_NAME`_wIndexLo);
    /* Third entry in the LUT starts the Interface Table pointers */
    pTmp += 2u;
    /* Now use the request interface number*/
    pTmp = &pTmp[interfaceN];
    /*USB_DEVICEx_CONFIGURATIONy_INTERFACEz_TABLE*/
    pTmp = (T_`$INSTANCE_NAME`_LUT *) pTmp->p_list;
    /* Now use Alternate setting number */
    pTmp = &pTmp[`$INSTANCE_NAME`_interfaceSetting[interfaceN]];
    /*USB_DEVICEx_CONFIGURATIONy_INTERFACEz_ALTERNATEi_HID_TABLE*/
    pTmp = (T_`$INSTANCE_NAME`_LUT *) pTmp->p_list;
    /* Fourth entry in the LUT starts the Hid Report Descriptor */
    pTmp += 3;
    pDescr = (volatile uint8 *)pTmp->p_list;
    `$INSTANCE_NAME`_currentTD.count =  (((uint16)pDescr[1] << 8u) | pDescr[0u]);
    `$INSTANCE_NAME`_currentTD.pData = &pDescr[2u];
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_FindReport
********************************************************************************
*
* Summary:
*  This routine sets up a transfer based on the Interface number, Report Type
*  and Report ID, then loads the currentTD structure with the address of the
*  buffer and the size.  The caller has to decide if it is a control read or
*  control write.
*
* Parameters:
*  None.
*
* Return:
*  currentTD
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_FindReport() `=ReentrantKeil($INSTANCE_NAME . "_FindReport")`
{
    T_`$INSTANCE_NAME`_LUT *pTmp;
    T_`$INSTANCE_NAME`_TD *pTD;
    uint8 interfaceN;
    uint8 reportType;

    /* `#START HID_FINDREPORT` Place custom handling here */

    /* `#END` */
    `$INSTANCE_NAME`_currentTD.count = 0u;   /* Init not supported condition */
    pTmp = `$INSTANCE_NAME`_GetConfigTablePtr(`$INSTANCE_NAME`_configuration - 1u);
    reportType = CY_GET_REG8(`$INSTANCE_NAME`_wValueHi);
    interfaceN = CY_GET_REG8(`$INSTANCE_NAME`_wIndexLo);
    /* Third entry in the LUT COnfiguration Table starts the Interface Table pointers */
    pTmp += 2;
    /* Now use the request interface number */
    pTmp = &pTmp[interfaceN];
    /*USB_DEVICEx_CONFIGURATIONy_INTERFACEz_TABLE*/
    pTmp = (T_`$INSTANCE_NAME`_LUT *) pTmp->p_list;
    if(interfaceN < `$INSTANCE_NAME`_MAX_INTERFACES_NUMBER)
    {
        /* Now use Alternate setting number */
        pTmp = &pTmp[`$INSTANCE_NAME`_interfaceSetting[interfaceN]];
        /*USB_DEVICEx_CONFIGURATIONy_INTERFACEz_ALTERNATEi_HID_TABLE*/
        pTmp = (T_`$INSTANCE_NAME`_LUT *) pTmp->p_list;
        /* Validate reportType to comply with "7.2.1 Get_Report Request" */
        if((reportType >= `$INSTANCE_NAME`_HID_GET_REPORT_INPUT) &&
           (reportType <= `$INSTANCE_NAME`_HID_GET_REPORT_FEATURE))
        {
            /* Get the entry proper TD (IN, OUT or Feature Report Table)*/
            pTmp = &pTmp[reportType - 1u];
            reportType = CY_GET_REG8(`$INSTANCE_NAME`_wValueLo);    /* Get reportID */
            /* Validate table support by the HID descriptor, compare table count with reportID */
            if(pTmp->c >= reportType)
            {
                pTD = (T_`$INSTANCE_NAME`_TD *) pTmp->p_list;
                pTD = &pTD[reportType];                          /* select entry depend on report ID*/
                `$INSTANCE_NAME`_currentTD.pData = pTD->pData;   /* Bufer pointer */
                `$INSTANCE_NAME`_currentTD.count = pTD->count;   /* Bufer Size */
                `$INSTANCE_NAME`_currentTD.pStatusBlock = pTD->pStatusBlock;
            }
        }
    }
}


/*******************************************************************************
* Additional user functions supporting HID Requests
********************************************************************************/

/* `#START HID_FUNCTIONS` Place any additional functions here */

/* `#END` */

#endif  /* End `$INSTANCE_NAME`_ENABLE_HID_CLASS*/


/* [] END OF FILE */
