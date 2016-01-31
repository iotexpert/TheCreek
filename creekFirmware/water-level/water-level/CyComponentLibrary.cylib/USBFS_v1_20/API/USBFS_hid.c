/*******************************************************************************
* File Name: `$INSTANCE_NAME`_hid.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*    USB HID Class request handler.
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
#include "`$INSTANCE_NAME`_hid.h"

#if defined(`$INSTANCE_NAME`_ENABLE_HID_CLASS)

/*******************************************************************************
* HID Variables
********************************************************************************/
uint8	`$INSTANCE_NAME`_bHID_Protocol;			/* HID device protocol status */
uint8	`$INSTANCE_NAME`_bHID_IdleRate;			/* HID device idle rate value */
uint8	`$INSTANCE_NAME`_bHID_ReportID;			/* HID device Report ID */

/*******************************************************************************
* Custom Declratations
********************************************************************************/
/* `#START CUSTOM_DECLARATIONS` Place your declaration here */

/* `#END` */
/*******************************************************************************
* External references
********************************************************************************/
uint8 `$INSTANCE_NAME`_InitControlRead(void);
uint8 `$INSTANCE_NAME`_InitControlWrite(void);
uint8 `$INSTANCE_NAME`_InitNoDataControlTransfer(void);
T_`$INSTANCE_NAME`_LUT *`$INSTANCE_NAME`_GetConfigTablePtr(uint8 c);
T_`$INSTANCE_NAME`_LUT *`$INSTANCE_NAME`_GetDeviceTablePtr();
extern T_`$INSTANCE_NAME`_TD CurrentTD;
extern uint8 `$INSTANCE_NAME`_CODE `$INSTANCE_NAME`_HIDREPORT_DESCRIPTORS[];
extern uint8 `$INSTANCE_NAME`_bConfiguration;

/*******************************************************************************
* Internal references
********************************************************************************/
void `$INSTANCE_NAME`_FindReport(void);

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_UpdateHIDTimer
********************************************************************************
* Summary:
*   Updates the HID report timer and reloads it if it expires
*   
* Parameters:  
*   bInterface Interface Number
*******************************************************************************/
uint8 `$INSTANCE_NAME`_UpdateHIDTimer(uint8 bInterface)
{
    uint8 ifc = bInterface;
    return `$INSTANCE_NAME`_IDLE_TIMER_INDEFINITE;
}
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_bGetProtocol
********************************************************************************
* Summary:
*   Returns the selected protocol value to the application
*   
* Parameters:  
*   bInterface Interface Number
*******************************************************************************/
uint8 `$INSTANCE_NAME`_bGetProtocol(uint8 bInterface)
{
    uint8 ifc = bInterface;
    return `$INSTANCE_NAME`_PROTOCOL_REPORT;
}
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_DispatchHIDClassRqst
********************************************************************************
* Summary:
*   This routine dispatches class requests
*   
* Parameters:  
*   None
*******************************************************************************/
uint8 `$INSTANCE_NAME`_DispatchHIDClassRqst()
{
    uint8 *pDescr;
    uint8 bRequestHandled = `$INSTANCE_NAME`_FALSE;
    T_`$INSTANCE_NAME`_LUT *pTmp = `$INSTANCE_NAME`_GetConfigTablePtr(`$INSTANCE_NAME`_bConfiguration - 1);

    if ((CY_GET_REG8(`$INSTANCE_NAME`_bmRequestType) & `$INSTANCE_NAME`_RQST_DIR_MASK) == `$INSTANCE_NAME`_RQST_DIR_D2H)
    {   /* Control Read */
		switch (*`$INSTANCE_NAME`_bRequest) 
		{
			case `$INSTANCE_NAME`_GET_DESCRIPTOR:
                if (CY_GET_REG8(`$INSTANCE_NAME`_wValueHi) == `$INSTANCE_NAME`_DESCR_HID_CLASS)
                {
                    /* pTmp is pointing to the Config Table */
                    /* Next...                              */
				    /* Get the Config Descriptor            */
                    /*   The HID Class Descriptor resides   */
                    /*   inside the config descriptor       */
                    CurrentTD.pData     = &((uint8 *) pTmp->p_list)[18];
                    CurrentTD.wCount    = 9;
					bRequestHandled     = `$INSTANCE_NAME`_InitControlRead();
                }
                else if (CY_GET_REG8(`$INSTANCE_NAME`_wValueHi) == `$INSTANCE_NAME`_DESCR_HID_REPORT)  
				{
                    pDescr = &`$INSTANCE_NAME`_HIDREPORT_DESCRIPTORS[0];
                    CurrentTD.wCount    =  (((uint16)pDescr[1] << 8) | pDescr[0]);
                    CurrentTD.pData     = &pDescr[2];
    
                    if (CurrentTD.wCount != 0)
                    {
                        bRequestHandled     = `$INSTANCE_NAME`_InitControlRead();
                    }
				}
                break;
            case `$INSTANCE_NAME`_HID_GET_REPORT:
                /*******************************************************************************
                * Not Supported -- But leaving a place holder in case someone needs it
                ********************************************************************************/
                /* `#START CUSTOM_HID_GET_REPORT` Place your code */
                
                /* `#END` */
                `$INSTANCE_NAME`_FindReport();
                bRequestHandled     = `$INSTANCE_NAME`_InitControlRead();
                break;

			case `$INSTANCE_NAME`_HID_GET_IDLE:
				CurrentTD.wCount    = 1;
				CurrentTD.pData     = &`$INSTANCE_NAME`_bHID_IdleRate;
				bRequestHandled     = `$INSTANCE_NAME`_InitControlRead();
                break;
			case `$INSTANCE_NAME`_HID_GET_PROTOCOL:
				CurrentTD.wCount    = 1;
				CurrentTD.pData     = &`$INSTANCE_NAME`_bHID_Protocol;
				bRequestHandled     = `$INSTANCE_NAME`_InitControlRead();
                break;
            default:
                break;
        }
    }
    else if ((CY_GET_REG8(`$INSTANCE_NAME`_bmRequestType) & `$INSTANCE_NAME`_RQST_DIR_MASK) == `$INSTANCE_NAME`_RQST_DIR_H2D)
    {   /* Control Write */
		switch (CY_GET_REG8(`$INSTANCE_NAME`_bRequest)) 
		{
            case `$INSTANCE_NAME`_HID_SET_REPORT:
                /*******************************************************************************
                * Not Supported -- But leaving a place holder in case someone needs it
                ********************************************************************************/
                /* `#START CUSTOM_HID_SET_REPORT` Place your code */
                
                /* `#END` */
                `$INSTANCE_NAME`_FindReport();
                bRequestHandled     = `$INSTANCE_NAME`_InitControlWrite();
                break;
            case `$INSTANCE_NAME`_HID_SET_IDLE:
				`$INSTANCE_NAME`_bHID_IdleRate = CY_GET_REG8(`$INSTANCE_NAME`_wValueHi);
				`$INSTANCE_NAME`_bHID_ReportID = CY_GET_REG8(`$INSTANCE_NAME`_wValueLo);
				bRequestHandled     = `$INSTANCE_NAME`_InitNoDataControlTransfer();
                break;
            case `$INSTANCE_NAME`_HID_SET_PROTOCOL:
			    `$INSTANCE_NAME`_bHID_Protocol  = CY_GET_REG8(`$INSTANCE_NAME`_wValueHi);
				bRequestHandled     = `$INSTANCE_NAME`_InitNoDataControlTransfer();
                break;
            default:
                break;
        }
    }
    return bRequestHandled;   
}
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_FindReport
********************************************************************************
* Summary:
*   This routine sets up a transfer based on the Interface number, Report Type
*   and Report ID, then loads the CurrentTD structure with the address of the
*   buffer and the size.  The caller has to decide if it is a control read or
*   control write.
*   
* Parameters:  
*   None
*******************************************************************************/
void `$INSTANCE_NAME`_FindReport()
{
    T_`$INSTANCE_NAME`_LUT *pTmp = `$INSTANCE_NAME`_GetConfigTablePtr(`$INSTANCE_NAME`_bConfiguration - 1);
    T_`$INSTANCE_NAME`_TD *pTD;
    /* Third entry in the LUT starts the Interface Table pointers */
    pTmp++;
    pTmp++;
    /* Now use the request interface number */
    pTmp = &pTmp[CY_GET_REG8(`$INSTANCE_NAME`_wIndexLo)];
	pTmp = (T_`$INSTANCE_NAME`_LUT *) pTmp->p_list;
    pTmp = (T_`$INSTANCE_NAME`_LUT *) pTmp->p_list;
    /* Get the entry proper TD */
    pTD = &((T_`$INSTANCE_NAME`_TD *) pTmp->p_list)[CY_GET_REG8(`$INSTANCE_NAME`_wValueHi) - 1];

    CurrentTD.pData     = pTD->pData;
    CurrentTD.wCount    = pTD->wCount;
    CurrentTD.pStatusBlock = pTD->pStatusBlock;
}
#endif
