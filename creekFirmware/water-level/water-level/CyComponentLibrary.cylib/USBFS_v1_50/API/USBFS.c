/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  API for USBFS Component.
*
* Note:
*  Many of the functions use endpoint number.  RAM arrays are sized with 9
*  elements so they are indexed directly by epNumber.  The SIE and ARB
*  registers are indexed by variations of epNumber - 1.
*
********************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "CyLib.h"
#include "`$INSTANCE_NAME`.h"


/***************************************
* External data references
***************************************/

extern volatile uint8 `$INSTANCE_NAME`_configuration;
extern volatile uint8 `$INSTANCE_NAME`_interfaceSetting[];
extern volatile uint8 `$INSTANCE_NAME`_deviceAddress;
extern volatile uint8 `$INSTANCE_NAME`_deviceStatus;
extern volatile uint8 `$INSTANCE_NAME`_device;
extern volatile uint8 `$INSTANCE_NAME`_transferState;
extern volatile uint8 `$INSTANCE_NAME`_lastPacketSize;

extern T_`$INSTANCE_NAME`_EP_CTL_BLOCK `$INSTANCE_NAME`_EP[];


/***************************************
* External function references
***************************************/

CY_ISR_PROTO(`$INSTANCE_NAME`_EP_0_ISR);
#if(`$INSTANCE_NAME`_EP1_ISR_REMOVE == 0u)
    CY_ISR_PROTO(`$INSTANCE_NAME`_EP_1_ISR);
#endif /* End `$INSTANCE_NAME`_EP1_ISR_REMOVE */
#if(`$INSTANCE_NAME`_EP2_ISR_REMOVE == 0u)
    CY_ISR_PROTO(`$INSTANCE_NAME`_EP_2_ISR);
#endif /* End `$INSTANCE_NAME`_EP2_ISR_REMOVE */
#if(`$INSTANCE_NAME`_EP3_ISR_REMOVE == 0u)
    CY_ISR_PROTO(`$INSTANCE_NAME`_EP_3_ISR);
#endif /* End `$INSTANCE_NAME`_EP3_ISR_REMOVE */
#if(`$INSTANCE_NAME`_EP4_ISR_REMOVE == 0u)
    CY_ISR_PROTO(`$INSTANCE_NAME`_EP_4_ISR);
#endif /* End `$INSTANCE_NAME`_EP4_ISR_REMOVE */
#if(`$INSTANCE_NAME`_EP5_ISR_REMOVE == 0u)
    CY_ISR_PROTO(`$INSTANCE_NAME`_EP_5_ISR);
#endif /* End `$INSTANCE_NAME`_EP5_ISR_REMOVE */
#if(`$INSTANCE_NAME`_EP6_ISR_REMOVE == 0u)
    CY_ISR_PROTO(`$INSTANCE_NAME`_EP_6_ISR);
#endif /* End `$INSTANCE_NAME`_EP6_ISR_REMOVE */
#if(`$INSTANCE_NAME`_EP7_ISR_REMOVE == 0u)
    CY_ISR_PROTO(`$INSTANCE_NAME`_EP_7_ISR);
#endif /* End `$INSTANCE_NAME`_EP7_ISR_REMOVE */
#if(`$INSTANCE_NAME`_EP8_ISR_REMOVE == 0u)
    CY_ISR_PROTO(`$INSTANCE_NAME`_EP_8_ISR);
#endif /* End `$INSTANCE_NAME`_EP8_ISR_REMOVE */
CY_ISR_PROTO(`$INSTANCE_NAME`_BUS_RESET_ISR);
CY_ISR_PROTO(`$INSTANCE_NAME`_SOF_ISR);

uint8 `$INSTANCE_NAME`_initVar = 0u;


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
*
* Summary:
*  This function initialize the USB SIE, arbiter and the
*  endpoint APIs, including setting the D+ Pullup
*
* Parameters:
*  device: Contains the device number from the desired device descriptor set 
*          entered with the USBFS customizer.
*  mode: The operating voltage. This determines whether the voltage regulator 
*        is enabled for 5V operation or if pass through mode is used for 3.3V 
*        operation. Symbolic names and their associated values are given in the 
*        following table.
*       `$INSTANCE_NAME`_3V_OPERATION - Disable voltage regulator and pass-thru 
*                                       Vcc for pull-up
*       `$INSTANCE_NAME`_5V_OPERATION - Enable voltage regulator and use 
*                                       regulator for pull-up
*       `$INSTANCE_NAME`_DWR_VDDD_OPERATION - Enable or Disable voltage 
*                         regulator depend on Vddd Voltage configuration in DWR.
*
* Return:
*   None.
*
* Global variables:
*  The `$INSTANCE_NAME`_intiVar variable is used to indicate initial 
*  configuration of this component. The variable is initialized to zero (0u) 
*  and set to one (1u) the first time `$INSTANCE_NAME`_Start() is called. 
*  This allows for component Re-Start without unnecessary re-initialization 
*  in all subsequent calls to the `$INSTANCE_NAME`_Start() routine. 
*  If re-initialization of the component is required the variable should be set 
*  to zero before call of UART_Start() routine, or the user may call 
*  `$INSTANCE_NAME`_Init() and `$INSTANCE_NAME`_InitComponent() as done 
*  in the `$INSTANCE_NAME`_Start() routine.
*
* Side Effects:
*   This function will reset all communication states to default.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Start(uint8 device, uint8 mode)
{
    /* If not Initialized then initialize all required hardware and software */
    if(`$INSTANCE_NAME`_initVar == 0u)
    {
        `$INSTANCE_NAME`_Init();
        `$INSTANCE_NAME`_initVar = 1u;
    }
    `$INSTANCE_NAME`_InitComponent(device, mode);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Init
********************************************************************************
*
* Summary:
*  Initialize component's hardware. Usually called in `$INSTANCE_NAME`_Start().
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Init(void)
{
    uint16 i;
    uint8 *p;
    uint8 enableInterrupts;
    enableInterrupts = CyEnterCriticalSection();

    p = (uint8 *)&`$INSTANCE_NAME`_ARB_RW1_DR_PTR[0u];
    
    /* Enable USB block  */
    `$INSTANCE_NAME`_PM_ACT_CFG_REG |= `$INSTANCE_NAME`_PM_ACT_EN_FSUSB;
    
    #if(`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
        /* Disable USBIO for TO3 */
        `$INSTANCE_NAME`_PM_AVAIL_CR_REG &= ~`$INSTANCE_NAME`_PM_AVAIL_EN_FSUSBIO;
    #endif /* End `$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1 */

    /* Enable core clock */
    `$INSTANCE_NAME`_USB_CLK_EN_REG = `$INSTANCE_NAME`_USB_CLK_ENABLE;

    /* Bus Reset Length */
    `$INSTANCE_NAME`_BUS_RST_CNT_REG = `$INSTANCE_NAME`_BUS_RST_COUNT;
    
    #if(`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
        /* Disable USBIO for TO3 */
        `$INSTANCE_NAME`_PM_AVAIL_CR_REG |= `$INSTANCE_NAME`_PM_AVAIL_EN_FSUSBIO;
    #endif /* End `$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1 */

    /* ENABLING USBIO PADS IN USB MODE FROM I/O MODE */
    #if(`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2)
        /* Ensure USB transmit enable is low (USB_USBIO_CR0.ten). - Manual Transmission - Disabled */
        `$INSTANCE_NAME`_USBIO_CR0_REG &= ~`$INSTANCE_NAME`_USBIO_CR0_TEN;
        CyDelayUs(0);  /*~50ns delay */
        /* Disable the USBIO by asserting PM.USB_CR0.fsusbio_pd_n(Inverted) high. This will
           have been set low by the power manger out of reset. 
           Also confirm USBIO pull-up disabled*/
        `$INSTANCE_NAME`_PM_USB_CR0_REG &= ~(`$INSTANCE_NAME`_PM_USB_CR0_PD_N |`$INSTANCE_NAME`_PM_USB_CR0_PD_PULLUP_N);
        /* Enable the USBIO reference by setting PM.USB_CR0.fsusbio_ref_en.*/
        `$INSTANCE_NAME`_PM_USB_CR0_REG |= `$INSTANCE_NAME`_PM_USB_CR0_REF_EN;
        /* The reference will be available 1 us after the regulator is enabled */
        CyDelayUs(1); 
        /* OR 40us after power restored */
        CyDelayUs(40); 
        /* Ensure the single ended disable bits are low (PRT15.INP_DIS[7:6])(input receiver enabled). */
        `$INSTANCE_NAME`_DM_INP_DIS_REG &= ~`$INSTANCE_NAME`_DM_MASK;
        `$INSTANCE_NAME`_DP_INP_DIS_REG &= ~`$INSTANCE_NAME`_DP_MASK;
        
        /* Enable USBIO */
        `$INSTANCE_NAME`_PM_USB_CR0_REG |= `$INSTANCE_NAME`_PM_USB_CR0_PD_N;
        CyDelayUs(2); 
        /* Set the USBIO pull-up enable */
        `$INSTANCE_NAME`_PM_USB_CR0_REG |= `$INSTANCE_NAME`_PM_USB_CR0_PD_PULLUP_N;
    
    #endif /* End `$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2 */

    /* If VBUS Monitoring is enable, setup the DR in the port reg */
    #if (`$INSTANCE_NAME`_MON_VBUS == 1u)
        `$INSTANCE_NAME`_VBUS_DR_REG &= ~`$INSTANCE_NAME`_VBUS_MASK;
    #endif /* `$INSTANCE_NAME`_MON_VBUS */

    /* Write WAx */
    CY_SET_REG8(&`$INSTANCE_NAME`_ARB_RW1_WA_PTR[0u],     0u);
    CY_SET_REG8(&`$INSTANCE_NAME`_ARB_RW1_WA_MSB_PTR[0u], 0u);

    /* Copy the data using the arbiter data register */
    for (i = 0u; i < `$INSTANCE_NAME`_EPX_DATA_BUF_MAX; i++)
    {
        CY_SET_REG8(p, 0xFFu);
    }

    CyExitCriticalSection(enableInterrupts);

    /* Set the bus reset Interrupt. */
    CyIntSetVector(`$INSTANCE_NAME`_BUS_RESET_VECT_NUM,   `$INSTANCE_NAME`_BUS_RESET_ISR);
    CyIntSetPriority(`$INSTANCE_NAME`_BUS_RESET_VECT_NUM, `$INSTANCE_NAME`_BUS_RESET_PRIOR);

    /* Set the SOF Interrupt. */
    CyIntSetVector(`$INSTANCE_NAME`_SOF_VECT_NUM,   `$INSTANCE_NAME`_SOF_ISR);
    CyIntSetPriority(`$INSTANCE_NAME`_SOF_VECT_NUM, `$INSTANCE_NAME`_SOF_PRIOR);

    /* Set the Control Endpoint Interrupt. */
    CyIntSetVector(`$INSTANCE_NAME`_EP_0_VECT_NUM,   `$INSTANCE_NAME`_EP_0_ISR);
    CyIntSetPriority(`$INSTANCE_NAME`_EP_0_VECT_NUM, `$INSTANCE_NAME`_EP_0_PRIOR);

    /* Set the Data Endpoint 1 Interrupt. */
    #if(`$INSTANCE_NAME`_EP1_ISR_REMOVE == 0u)
        CyIntSetVector(`$INSTANCE_NAME`_EP_1_VECT_NUM,   `$INSTANCE_NAME`_EP_1_ISR);
        CyIntSetPriority(`$INSTANCE_NAME`_EP_1_VECT_NUM, `$INSTANCE_NAME`_EP_1_PRIOR);
    #endif   /* End `$INSTANCE_NAME`_EP1_ISR_REMOVE */

    /* Set the Data Endpoint 2 Interrupt. */
    #if(`$INSTANCE_NAME`_EP2_ISR_REMOVE == 0u)
        CyIntSetVector(`$INSTANCE_NAME`_EP_2_VECT_NUM,   `$INSTANCE_NAME`_EP_2_ISR);
        CyIntSetPriority(`$INSTANCE_NAME`_EP_2_VECT_NUM, `$INSTANCE_NAME`_EP_2_PRIOR);
    #endif   /* End `$INSTANCE_NAME`_EP2_ISR_REMOVE */

    /* Set the Data Endpoint 3 Interrupt. */
    #if(`$INSTANCE_NAME`_EP3_ISR_REMOVE == 0u)
        CyIntSetVector(`$INSTANCE_NAME`_EP_3_VECT_NUM,   `$INSTANCE_NAME`_EP_3_ISR);
        CyIntSetPriority(`$INSTANCE_NAME`_EP_3_VECT_NUM, `$INSTANCE_NAME`_EP_3_PRIOR);
    #endif   /* End `$INSTANCE_NAME`_EP3_ISR_REMOVE */

    /* Set the Data Endpoint 4 Interrupt. */
    #if(`$INSTANCE_NAME`_EP4_ISR_REMOVE == 0u)
        CyIntSetVector(`$INSTANCE_NAME`_EP_4_VECT_NUM,   `$INSTANCE_NAME`_EP_4_ISR);
        CyIntSetPriority(`$INSTANCE_NAME`_EP_4_VECT_NUM, `$INSTANCE_NAME`_EP_4_PRIOR);
    #endif   /* End `$INSTANCE_NAME`_EP4_ISR_REMOVE */

    /* Set the Data Endpoint 5 Interrupt. */
    #if(`$INSTANCE_NAME`_EP5_ISR_REMOVE == 0u)
        CyIntSetVector(`$INSTANCE_NAME`_EP_5_VECT_NUM,   `$INSTANCE_NAME`_EP_5_ISR);
        CyIntSetPriority(`$INSTANCE_NAME`_EP_5_VECT_NUM, `$INSTANCE_NAME`_EP_5_PRIOR);
    #endif   /* End `$INSTANCE_NAME`_EP5_ISR_REMOVE */

    /* Set the Data Endpoint 6 Interrupt. */
    #if(`$INSTANCE_NAME`_EP6_ISR_REMOVE == 0u)
        CyIntSetVector(`$INSTANCE_NAME`_EP_6_VECT_NUM,   `$INSTANCE_NAME`_EP_6_ISR);
        CyIntSetPriority(`$INSTANCE_NAME`_EP_6_VECT_NUM, `$INSTANCE_NAME`_EP_6_PRIOR);
    #endif   /* End `$INSTANCE_NAME`_EP6_ISR_REMOVE */

     /* Set the Data Endpoint 7 Interrupt. */
    #if(`$INSTANCE_NAME`_EP7_ISR_REMOVE == 0u)
        CyIntSetVector(`$INSTANCE_NAME`_EP_7_VECT_NUM,   `$INSTANCE_NAME`_EP_7_ISR);
        CyIntSetPriority(`$INSTANCE_NAME`_EP_7_VECT_NUM, `$INSTANCE_NAME`_EP_7_PRIOR);
    #endif   /* End `$INSTANCE_NAME`_EP7_ISR_REMOVE */

    /* Set the Data Endpoint 8 Interrupt. */
    #if(`$INSTANCE_NAME`_EP8_ISR_REMOVE == 0u)
        CyIntSetVector(`$INSTANCE_NAME`_EP_8_VECT_NUM,   `$INSTANCE_NAME`_EP_8_ISR);
        CyIntSetPriority(`$INSTANCE_NAME`_EP_8_VECT_NUM, `$INSTANCE_NAME`_EP_8_PRIOR);
    #endif   /* End `$INSTANCE_NAME`_EP8_ISR_REMOVE */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_InitComponent
********************************************************************************
*
* Summary:
*  Initialize the component, except for the HW which is done one time in
*  the Start function.  This function pulls up D+.
*
* Parameters:
*  device: Contains the device number from the desired device descriptor set 
*          entered with the USBFS customizer.
*  mode: The operating voltage. This determines whether the voltage regulator 
*        is enabled for 5V operation or if pass through mode is used for 3.3V 
*        operation. Symbolic names and their associated values are given in the 
*        following table.
*       `$INSTANCE_NAME`_3V_OPERATION - Disable voltage regulator and pass-thru 
*                                       Vcc for pull-up
*       `$INSTANCE_NAME`_5V_OPERATION - Enable voltage regulator and use 
*                                       regulator for pull-up
*       `$INSTANCE_NAME`_DWR_VDDD_OPERATION - Enable or Disable voltage 
*                         regulator depend on Vddd Voltage configuration in DWR.
*
* Return:
*   None.
*
* Global variables:
*   `$INSTANCE_NAME`_device: Contains the started device number from the 
*       desired device descriptor set entered with the USBFS customizer.
*   `$INSTANCE_NAME`_transferState: This variable used by the communication 
*       functions to handle current transfer state. Initialized to 
*       TRANS_STATE_IDLE in this API. 
*   `$INSTANCE_NAME`_configuration: Contains current configuration number 
*       which is set by the Host using SET_CONFIGURATION request. 
*       Initialized to zero in this API.
*   `$INSTANCE_NAME`_deviceAddress: Contains current device address. This 
*       variable is initialized to zero in this API. Host starts to communicate 
*      to device with address 0 and then set it to whatever value using 
*      SET_ADDRESS request.  
*   `$INSTANCE_NAME`_deviceStatus: initialized to 0.
*       This is two bit variable which contain power status in first bit 
*       (DEVICE_STATUS_BUS_POWERED or DEVICE_STATUS_SELF_POWERED) and remote 
*       wakeup status (DEVICE_STATUS_REMOTE_WAKEUP) in second bit. 
*   `$INSTANCE_NAME`_lastPacketSize initialized to 0;
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_InitComponent(uint8 device, uint8 mode)
{
    /* Enable Interrupts. */
    CyIntEnable(`$INSTANCE_NAME`_BUS_RESET_VECT_NUM);
    CyIntEnable(`$INSTANCE_NAME`_EP_0_VECT_NUM);
    #if(`$INSTANCE_NAME`_EP1_ISR_REMOVE == 0u)
        CyIntEnable(`$INSTANCE_NAME`_EP_1_VECT_NUM);
    #endif   /* End `$INSTANCE_NAME`_EP1_ISR_REMOVE */
    #if(`$INSTANCE_NAME`_EP2_ISR_REMOVE == 0u)
        CyIntEnable(`$INSTANCE_NAME`_EP_2_VECT_NUM);
    #endif   /* End `$INSTANCE_NAME`_EP2_ISR_REMOVE */
    #if(`$INSTANCE_NAME`_EP3_ISR_REMOVE == 0u)
        CyIntEnable(`$INSTANCE_NAME`_EP_3_VECT_NUM);
    #endif   /* End `$INSTANCE_NAME`_EP3_ISR_REMOVE */
    #if(`$INSTANCE_NAME`_EP4_ISR_REMOVE == 0u)
        CyIntEnable(`$INSTANCE_NAME`_EP_4_VECT_NUM);
    #endif   /* End `$INSTANCE_NAME`_EP4_ISR_REMOVE */
    #if(`$INSTANCE_NAME`_EP5_ISR_REMOVE == 0u)
        CyIntEnable(`$INSTANCE_NAME`_EP_5_VECT_NUM);
    #endif   /* End `$INSTANCE_NAME`_EP5_ISR_REMOVE */
    #if(`$INSTANCE_NAME`_EP6_ISR_REMOVE == 0u)
        CyIntEnable(`$INSTANCE_NAME`_EP_6_VECT_NUM);
    #endif   /* End `$INSTANCE_NAME`_EP6_ISR_REMOVE */
    #if(`$INSTANCE_NAME`_EP7_ISR_REMOVE == 0u)
        CyIntEnable(`$INSTANCE_NAME`_EP_7_VECT_NUM);
    #endif   /* End `$INSTANCE_NAME`_EP7_ISR_REMOVE */
    #if(`$INSTANCE_NAME`_EP8_ISR_REMOVE == 0u)
        CyIntEnable(`$INSTANCE_NAME`_EP_8_VECT_NUM);
    #endif   /* End `$INSTANCE_NAME`_EP8_ISR_REMOVE */

    `$INSTANCE_NAME`_transferState = `$INSTANCE_NAME`_TRANS_STATE_IDLE;

    /* USB Locking: Enabled, VRegulator: depend on mode or DWR Voltage configuration*/
    switch(mode)
    {
        case `$INSTANCE_NAME`_3V_OPERATION:
            `$INSTANCE_NAME`_CR1_REG = `$INSTANCE_NAME`_CR1_ENABLE_LOCK;
            break;
        case `$INSTANCE_NAME`_5V_OPERATION:
            `$INSTANCE_NAME`_CR1_REG = `$INSTANCE_NAME`_CR1_ENABLE_LOCK | `$INSTANCE_NAME`_CR1_REG_ENABLE;
            break;
        default:   /*`$INSTANCE_NAME`_DWR_VDDD_OPERATION */    
            if(`$INSTANCE_NAME`_VDDD_MV < `$INSTANCE_NAME`_3500MV)
            {        
                `$INSTANCE_NAME`_CR1_REG = `$INSTANCE_NAME`_CR1_ENABLE_LOCK;
            }
            else
            {
                `$INSTANCE_NAME`_CR1_REG = `$INSTANCE_NAME`_CR1_ENABLE_LOCK | `$INSTANCE_NAME`_CR1_REG_ENABLE;
            }
            break;
    }

    /* Record the descriptor selection */
    `$INSTANCE_NAME`_device = device;

    /* Clear all of the component data */
    `$INSTANCE_NAME`_configuration = 0u;
    `$INSTANCE_NAME`_deviceAddress  = 0u;
    `$INSTANCE_NAME`_deviceStatus = 0u;

    `$INSTANCE_NAME`_lastPacketSize = 0u;

    /*  ACK Setup, Stall IN/OUT */
    CY_SET_REG8(`$INSTANCE_NAME`_EP0_CR_PTR, `$INSTANCE_NAME`_MODE_STALL_IN_OUT);

    /* Enable the SIE with an address 0 */
    CY_SET_REG8(`$INSTANCE_NAME`_CR0_PTR, `$INSTANCE_NAME`_CR0_ENABLE);

    /* Finally, Enable d+ pullup and select iomode to USB mode*/
    CY_SET_REG8(`$INSTANCE_NAME`_USBIO_CR1_PTR, `$INSTANCE_NAME`_USBIO_CR1_USBPUEN);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReInitComponent
********************************************************************************
*
* Summary:
*  This function reinitialize the component configuration and is 
*  intend to be called from the Reset interrupt.
*
* Parameters:
*  None.
*
* Return:
*   None.
*
* Global variables:
*   `$INSTANCE_NAME`_device: Contains the started device number from the 
*       desired device descriptor set entered with the USBFS customizer.
*   `$INSTANCE_NAME`_transferState: This variable used by the communication 
*       functions to handle current transfer state. Initialized to 
*       TRANS_STATE_IDLE in this API. 
*   `$INSTANCE_NAME`_configuration: Contains current configuration number 
*       which is set by the Host using SET_CONFIGURATION request. 
*       Initialized to zero in this API.
*   `$INSTANCE_NAME`_deviceAddress: Contains current device address. This 
*       variable is initialized to zero in this API. Host starts to communicate 
*      to device with address 0 and then set it to whatever value using 
*      SET_ADDRESS request.  
*   `$INSTANCE_NAME`_deviceStatus: initialized to 0.
*       This is two bit variable which contain power status in first bit 
*       (DEVICE_STATUS_BUS_POWERED or DEVICE_STATUS_SELF_POWERED) and remote 
*       wakeup status (DEVICE_STATUS_REMOTE_WAKEUP) in second bit. 
*   `$INSTANCE_NAME`_lastPacketSize initialized to 0;
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_ReInitComponent()
{
    `$INSTANCE_NAME`_transferState = `$INSTANCE_NAME`_TRANS_STATE_IDLE;

    /* Clear all of the component data */
    `$INSTANCE_NAME`_configuration = 0u;
    `$INSTANCE_NAME`_deviceAddress  = 0u;
    `$INSTANCE_NAME`_deviceStatus = 0u;

    `$INSTANCE_NAME`_lastPacketSize = 0u;

    /*  ACK Setup, Stall IN/OUT */
    CY_SET_REG8(`$INSTANCE_NAME`_EP0_CR_PTR, `$INSTANCE_NAME`_MODE_STALL_IN_OUT);

    /* Enable the SIE with an address 0 */
    CY_SET_REG8(`$INSTANCE_NAME`_CR0_PTR, `$INSTANCE_NAME`_CR0_ENABLE);

}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
*
* Summary:
*  This function shuts down the USB function including to release
*  the D+ Pullup and disabling the SIE.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`
{
    /* Disable the SIE */
    `$INSTANCE_NAME`_CR0_REG &= ~`$INSTANCE_NAME`_CR0_ENABLE;
    /* Disable the d+ pullup */
    `$INSTANCE_NAME`_USBIO_CR1_REG &= ~`$INSTANCE_NAME`_USBIO_CR1_USBPUEN;
    
    /* Disable the reset and EP interrupts */
    CyIntDisable(`$INSTANCE_NAME`_BUS_RESET_VECT_NUM);
    CyIntDisable(`$INSTANCE_NAME`_EP_0_VECT_NUM);
    #if(`$INSTANCE_NAME`_EP1_ISR_REMOVE == 0u)
        CyIntDisable(`$INSTANCE_NAME`_EP_1_VECT_NUM);
    #endif   /* End `$INSTANCE_NAME`_EP1_ISR_REMOVE */
    #if(`$INSTANCE_NAME`_EP2_ISR_REMOVE == 0u)
        CyIntDisable(`$INSTANCE_NAME`_EP_2_VECT_NUM);
    #endif   /* End `$INSTANCE_NAME`_EP2_ISR_REMOVE */
    #if(`$INSTANCE_NAME`_EP3_ISR_REMOVE == 0u)
        CyIntDisable(`$INSTANCE_NAME`_EP_3_VECT_NUM);
    #endif   /* End `$INSTANCE_NAME`_EP3_ISR_REMOVE */
    #if(`$INSTANCE_NAME`_EP4_ISR_REMOVE == 0u)
        CyIntDisable(`$INSTANCE_NAME`_EP_4_VECT_NUM);
    #endif   /* End `$INSTANCE_NAME`_EP4_ISR_REMOVE */
    #if(`$INSTANCE_NAME`_EP5_ISR_REMOVE == 0u)
        CyIntDisable(`$INSTANCE_NAME`_EP_5_VECT_NUM);
    #endif   /* End `$INSTANCE_NAME`_EP5_ISR_REMOVE */
    #if(`$INSTANCE_NAME`_EP6_ISR_REMOVE == 0u)
        CyIntDisable(`$INSTANCE_NAME`_EP_6_VECT_NUM);
    #endif   /* End `$INSTANCE_NAME`_EP6_ISR_REMOVE */
    #if(`$INSTANCE_NAME`_EP7_ISR_REMOVE == 0u)
        CyIntDisable(`$INSTANCE_NAME`_EP_7_VECT_NUM);
    #endif   /* End `$INSTANCE_NAME`_EP7_ISR_REMOVE */
    #if(`$INSTANCE_NAME`_EP8_ISR_REMOVE == 0u)
        CyIntDisable(`$INSTANCE_NAME`_EP_8_VECT_NUM);
    #endif   /* End `$INSTANCE_NAME`_EP8_ISR_REMOVE */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_CheckActivity
********************************************************************************
*
* Summary:
*  Returns the activity status of the bus.  Clears the status hardware to
*  provide fresh activity status on the next call of this routine.
*
* Parameters:
*  None.
*
* Return:
*  1 - If bus activity was detected since the last call to this function
*  0 - If bus activity not was detected since the last call to this function
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_CheckActivity(void) `=ReentrantKeil($INSTANCE_NAME . "_CheckActivity")`
{
    uint8 r;
    
    r = CY_GET_REG8(`$INSTANCE_NAME`_CR1_PTR);
    CY_SET_REG8(`$INSTANCE_NAME`_CR1_PTR, (r & ~`$INSTANCE_NAME`_CR1_BUS_ACTIVITY));

    return((r & `$INSTANCE_NAME`_CR1_BUS_ACTIVITY) >> `$INSTANCE_NAME`_CR1_BUS_ACTIVITY_SHIFT);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetConfiguration
********************************************************************************
*
* Summary:
*  Returns the current configuration setting
*
* Parameters:
*  None.
*
* Return:
*  configuration.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_GetConfiguration(void) `=ReentrantKeil($INSTANCE_NAME . "_GetConfiguration")`
{
    return(`$INSTANCE_NAME`_configuration);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetInterfaceSetting
********************************************************************************
*
* Summary:
*  Returns the alternate setting from current interface
*
* Parameters:
*  uint8 interfaceNumber, interface number
*
* Return:
*  Alternate setting.
*
*******************************************************************************/
uint8  `$INSTANCE_NAME`_GetInterfaceSetting(uint8 interfaceNumber) \
                                                    `=ReentrantKeil($INSTANCE_NAME . "_GetInterfaceSetting")`
{
    return(`$INSTANCE_NAME`_interfaceSetting[interfaceNumber]);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetEPState
********************************************************************************
*
* Summary:
*  Returned the state of the requested endpoint.
*
* Parameters:
*  epNumber: Endpoint Number
*
* Return:
*  State of the requested endpoint.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_GetEPState(uint8 epNumber) `=ReentrantKeil($INSTANCE_NAME . "_GetEPState")`
{
    return(`$INSTANCE_NAME`_EP[epNumber].apiEpState);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetEPCount
********************************************************************************
*
* Summary:
*  This function supports Data Endpoints only(EP1-EP8).
*  Returns the transfer count for the requested endpoint.  The value from
*  the count registers includes 2 counts for the two byte checksum of the
*  packet.  This function subtracts the two counts. 
*
* Parameters:
*  epNumber: Data Endpoint Number.
*            Valid values are between 1 and 8.
*
* Return:
*  Returns the current byte count from the specified endpoin or 0 for an 
*  invalid endpoint.
*
*******************************************************************************/
uint16 `$INSTANCE_NAME`_GetEPCount(uint8 epNumber) `=ReentrantKeil($INSTANCE_NAME . "_GetEPCount")`
{
    uint8 ri;
    uint16 count = 0u;
    
    if((epNumber > `$INSTANCE_NAME`_EP0) && (epNumber < `$INSTANCE_NAME`_MAX_EP))
    {
        ri = ((epNumber - `$INSTANCE_NAME`_EP1) << `$INSTANCE_NAME`_EPX_CNTX_ADDR_SHIFT);
    
        count = (uint16)((CY_GET_REG8(&`$INSTANCE_NAME`_SIE_EP1_CNT0_PTR[ri]) & `$INSTANCE_NAME`_EPX_CNT0_MASK) << 8u) \
                        | CY_GET_REG8(&`$INSTANCE_NAME`_SIE_EP1_CNT1_PTR[ri]);
        count -= `$INSTANCE_NAME`_EPX_CNTX_CRC_COUNT;
    }
    return(count); 
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_LoadInEP
********************************************************************************
*
* Summary:
*  Loads and enables the specified USB data endpoint for an IN interrupt or bulk 
*  transfer.
*
* Parameters:
*  epNumber: Contains the data endpoint number.
*            Valid values are between 1 and 8.
*  *pData: A pointer to a data array from which the data for the endpoint space 
*          is loaded.
*  length: The number of bytes to transfer from the array and then send as a 
*          result of an IN request. Valid values are between 0 and 512.
*
* Return:
*  None.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_LoadInEP(uint8 epNumber, uint8 *pData, uint16 length)
{
    uint16 i;
    uint8 ri;
    uint8 *p;

    if((epNumber > `$INSTANCE_NAME`_EP0) && (epNumber < `$INSTANCE_NAME`_MAX_EP) && (pData != `$INSTANCE_NAME`_NULL))
    {
        ri = ((epNumber - `$INSTANCE_NAME`_EP1) << `$INSTANCE_NAME`_EPX_CNTX_ADDR_SHIFT);
        p = (uint8 *)&`$INSTANCE_NAME`_ARB_RW1_DR_PTR[ri];
        /* Write WAx */
        CY_SET_REG8(&`$INSTANCE_NAME`_ARB_RW1_WA_PTR[ri],     `$INSTANCE_NAME`_EP[epNumber].buffOffset & 0xFFu);
        CY_SET_REG8(&`$INSTANCE_NAME`_ARB_RW1_WA_MSB_PTR[ri], (`$INSTANCE_NAME`_EP[epNumber].buffOffset >> 8u));
        
        /* Limits length to available buffer space */
        if(length > `$INSTANCE_NAME`_EPX_DATA_BUF_MAX - `$INSTANCE_NAME`_EP[epNumber].buffOffset)
        {
            length = `$INSTANCE_NAME`_EPX_DATA_BUF_MAX - `$INSTANCE_NAME`_EP[epNumber].buffOffset;
        }
        /* Copy the data using the arbiter data register */
        for (i = 0u; i < length; i++)
        {
            CY_SET_REG8(p, pData[i]);
        }
        /* Set the count and data toggle */
        CY_SET_REG8(&`$INSTANCE_NAME`_SIE_EP1_CNT0_PTR[ri], (length >> 8u) | (`$INSTANCE_NAME`_EP[epNumber].epToggle));
        CY_SET_REG8(&`$INSTANCE_NAME`_SIE_EP1_CNT1_PTR[ri],  length & 0xFFu);
        /* Write the RAx */
        CY_SET_REG8(&`$INSTANCE_NAME`_ARB_RW1_RA_PTR[ri],     `$INSTANCE_NAME`_EP[epNumber].buffOffset & 0xFFu);
        CY_SET_REG8(&`$INSTANCE_NAME`_ARB_RW1_RA_MSB_PTR[ri], (`$INSTANCE_NAME`_EP[epNumber].buffOffset >> 8u));
    
        `$INSTANCE_NAME`_EP[epNumber].apiEpState = `$INSTANCE_NAME`_NO_EVENT_PENDING;
        /* Write the Mode register */
        CY_SET_REG8(&`$INSTANCE_NAME`_SIE_EP1_CR0_PTR[ri], `$INSTANCE_NAME`_EP[epNumber].epMode);
    }
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadOutEP
********************************************************************************
*
* Summary:
*  Read data from an endpoint.  The application must call 
*  `$INSTANCE_NAME`_GetEPState to see if an event is pending.
*
* Parameters:
*  epNumber: Contains the data endpoint number. 
*            Valid values are between 1 and 8.
*  pData: A pointer to a data array from which the data for the endpoint space 
*         is loaded.
*  length: The number of bytes to transfer from the USB Out enpoint and loads 
*          it into data array. Valid values are between 0 and 256. The function
*          moves fewer than the requested number of bytes if the host sends 
*          fewer bytes than requested.
*
* Returns:
*  Number of bytes received, 0 for an invalid endpoint.
*
* Reentrant:
*  No.
*
*******************************************************************************/
uint16 `$INSTANCE_NAME`_ReadOutEP(uint8 epNumber, uint8 *pData, uint16 length)
{
    uint16 i, xferCount;
    uint8 ri;
    uint8 *p;

    if((epNumber > `$INSTANCE_NAME`_EP0) && (epNumber < `$INSTANCE_NAME`_MAX_EP) && (pData != `$INSTANCE_NAME`_NULL))
    {
        ri = ((epNumber - `$INSTANCE_NAME`_EP1) << `$INSTANCE_NAME`_EPX_CNTX_ADDR_SHIFT);
        p = (uint8 *)&`$INSTANCE_NAME`_ARB_RW1_DR_PTR[ri];
        /* Write the RAx */
        CY_SET_REG8(&`$INSTANCE_NAME`_ARB_RW1_RA_PTR[ri],     `$INSTANCE_NAME`_EP[epNumber].buffOffset & 0xFFu);
        CY_SET_REG8(&`$INSTANCE_NAME`_ARB_RW1_RA_MSB_PTR[ri], (`$INSTANCE_NAME`_EP[epNumber].buffOffset >> 8u));

        /* Determine which is smaller the requested data or the available data */
        xferCount = `$INSTANCE_NAME`_GetEPCount(epNumber);
        if (length > xferCount)
        {
            length = xferCount;
        }
        /* Copy the data using the arbiter data register */
        for (i = 0u; i < length; i++)
        {
            pData[i] = CY_GET_REG8(p);
        }
        /* Write the WAx */
        CY_SET_REG8(&`$INSTANCE_NAME`_ARB_RW1_WA_PTR[ri],     `$INSTANCE_NAME`_EP[epNumber].buffOffset & 0xFFu);
        CY_SET_REG8(&`$INSTANCE_NAME`_ARB_RW1_WA_MSB_PTR[ri], (`$INSTANCE_NAME`_EP[epNumber].buffOffset >> 8u));
    
        /* (re)arming of OUT endpoint */
        `$INSTANCE_NAME`_EnableOutEP(epNumber);
    }
    else
    {
        length = 0;
    }

    return(length);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_EnableOutEP
********************************************************************************
*
* Summary:
*  This function enables an OUT endpoint.  It should not be
*  called for an IN endpoint.
*
* Parameters:
*  epNumber: Endpoint Number
*            Valid values are between 1 and 8.
*
* Return:
*   None.
*
* Global variables:
*  `$INSTANCE_NAME`_EP[epNumber].apiEpState - set to NO_EVENT_PENDING
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_EnableOutEP(uint8 epNumber)
{
    uint8 ri;

    if((epNumber > `$INSTANCE_NAME`_EP0) && (epNumber < `$INSTANCE_NAME`_MAX_EP))
    {
        ri = ((epNumber - `$INSTANCE_NAME`_EP1) << `$INSTANCE_NAME`_EPX_CNTX_ADDR_SHIFT);
        `$INSTANCE_NAME`_EP[epNumber].apiEpState = `$INSTANCE_NAME`_NO_EVENT_PENDING;
        /* Write the Mode register */
        CY_SET_REG8(&`$INSTANCE_NAME`_SIE_EP1_CR0_PTR[ri], `$INSTANCE_NAME`_EP[epNumber].epMode);
    }
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_DisableOutEP
********************************************************************************
*
* Summary: 
*  This function disables an OUT endpoint.  It should not be
*  called for an IN endpoint.
*
* Parameters:
*  epNumber: Endpoint Number
*            Valid values are between 1 and 8.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_DisableOutEP(uint8 epNumber) `=ReentrantKeil($INSTANCE_NAME . "_DisableOutEP")`
{
    uint8 ri ;
    
    if((epNumber > `$INSTANCE_NAME`_EP0) && (epNumber < `$INSTANCE_NAME`_MAX_EP))
    {
        ri = ((epNumber - `$INSTANCE_NAME`_EP1) << `$INSTANCE_NAME`_EPX_CNTX_ADDR_SHIFT);
        /* Write the Mode register */
        CY_SET_REG8(&`$INSTANCE_NAME`_SIE_EP1_CR0_PTR[ri], `$INSTANCE_NAME`_MODE_NAK_OUT);
    }    
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Force
********************************************************************************
*
* Summary:
*  Forces the bus state
*
* Parameters:
*  bState
*    `$INSTANCE_NAME`_FORCE_J
*    `$INSTANCE_NAME`_FORCE_K
*    `$INSTANCE_NAME`_FORCE_SE0
*    `$INSTANCE_NAME`_FORCE_NONE
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Force(uint8 bState) `=ReentrantKeil($INSTANCE_NAME . "_Force")`
{
    CY_SET_REG8(`$INSTANCE_NAME`_USBIO_CR0_PTR, bState);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetEPAckState
********************************************************************************
*
* Summary:
*  Returns the ACK of the CR0 Register (ACKD)
*
* Parameters:
*  epNumber: Endpoint Number
*            Valid values are between 1 and 8.
*
* Returns
*  0 if nothing has been ACKD, non-=zero something has been ACKD
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_GetEPAckState(uint8 epNumber) `=ReentrantKeil($INSTANCE_NAME . "_GetEPAckState")`
{
    uint8 ri;
    uint8 cr = 0;
    
    if((epNumber > `$INSTANCE_NAME`_EP0) && (epNumber < `$INSTANCE_NAME`_MAX_EP))
    {
        ri = ((epNumber - `$INSTANCE_NAME`_EP1) << `$INSTANCE_NAME`_EPX_CNTX_ADDR_SHIFT);
        cr = CY_GET_REG8(&`$INSTANCE_NAME`_SIE_EP1_CR0_PTR[ri]) & `$INSTANCE_NAME`_MODE_ACKD; 
    }
    
    return(cr);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetPowerStatus
********************************************************************************
*
* Summary:
*  Sets the device power status for reporting in the Get Device Status
*  request
*
* Parameters:
*  powerStatus: `$INSTANCE_NAME`_DEVICE_STATUS_BUS_POWERED(0) - Bus Powered, 
*               `$INSTANCE_NAME`_DEVICE_STATUS_SELF_POWERED(1) - Self Powered
*
* Return:
*   None.
*
* Global variables:
*  `$INSTANCE_NAME`_deviceStatus - set power status
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetPowerStatus(uint8 powerStatus)
{
    if (powerStatus != `$INSTANCE_NAME`_DEVICE_STATUS_BUS_POWERED)
    {
        `$INSTANCE_NAME`_deviceStatus |=  `$INSTANCE_NAME`_DEVICE_STATUS_SELF_POWERED;
    }
    else
    {
        `$INSTANCE_NAME`_deviceStatus &=  ~`$INSTANCE_NAME`_DEVICE_STATUS_SELF_POWERED;
    }
}


#if (`$INSTANCE_NAME`_MON_VBUS == 1u)

    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_VBusPresent
    ********************************************************************************
    *
    * Summary:
    *  Determines VBUS presense for Self Powered Devices.  
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  1 if VBUS is present, otherwise 0.
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_VBusPresent(void) `=ReentrantKeil($INSTANCE_NAME . "_VBusPresent")`
    {
        return((CY_GET_REG8(`$INSTANCE_NAME`_VBUS_PS_PTR) & `$INSTANCE_NAME`_VBUS_MASK) ? 1u : 0u);
    }

#endif /* `$INSTANCE_NAME`_MON_VBUS */


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_RWUEnabled
********************************************************************************
*
* Summary:
*  Returns TRUE if Remote Wake Up is enabled, otherwise FALSE
*
* Parameters:
*   None.
*
* Return:
*  TRUE -  Remote Wake Up Enabled
*  FALSE - Remote Wake Up Disabled
*
* Global variables:
*  `$INSTANCE_NAME`_deviceStatus - checked to determine remote status
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_RWUEnabled(void) `=ReentrantKeil($INSTANCE_NAME . "_RWUEnabled")`
{
    uint8 result = `$INSTANCE_NAME`_FALSE;
    if((`$INSTANCE_NAME`_deviceStatus & `$INSTANCE_NAME`_DEVICE_STATUS_REMOTE_WAKEUP) != 0u)
    {
        result = `$INSTANCE_NAME`_TRUE;
    }

    return(result);
}


/* [] END OF FILE */
