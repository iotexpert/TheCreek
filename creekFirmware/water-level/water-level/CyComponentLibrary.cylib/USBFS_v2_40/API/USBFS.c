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
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include <CyDmac.h>
#include "`$INSTANCE_NAME`.h"
#include "`$INSTANCE_NAME`_hid.h"
#if(`$INSTANCE_NAME`_DMA1_REMOVE == 0u)
    #include "`$INSTANCE_NAME`_ep1_dma.h"
#endif   /* End `$INSTANCE_NAME`_DMA1_REMOVE */
#if(`$INSTANCE_NAME`_DMA2_REMOVE == 0u)
    #include "`$INSTANCE_NAME`_ep2_dma.h"
#endif   /* End `$INSTANCE_NAME`_DMA2_REMOVE */
#if(`$INSTANCE_NAME`_DMA3_REMOVE == 0u)
    #include "`$INSTANCE_NAME`_ep3_dma.h"
#endif   /* End `$INSTANCE_NAME`_DMA3_REMOVE */
#if(`$INSTANCE_NAME`_DMA4_REMOVE == 0u)
    #include "`$INSTANCE_NAME`_ep4_dma.h"
#endif   /* End `$INSTANCE_NAME`_DMA4_REMOVE */
#if(`$INSTANCE_NAME`_DMA5_REMOVE == 0u)
    #include "`$INSTANCE_NAME`_ep5_dma.h"
#endif   /* End `$INSTANCE_NAME`_DMA5_REMOVE */
#if(`$INSTANCE_NAME`_DMA6_REMOVE == 0u)
    #include "`$INSTANCE_NAME`_ep6_dma.h"
#endif   /* End `$INSTANCE_NAME`_DMA6_REMOVE */
#if(`$INSTANCE_NAME`_DMA7_REMOVE == 0u)
    #include "`$INSTANCE_NAME`_ep7_dma.h"
#endif   /* End `$INSTANCE_NAME`_DMA7_REMOVE */
#if(`$INSTANCE_NAME`_DMA8_REMOVE == 0u)
    #include "`$INSTANCE_NAME`_ep8_dma.h"
#endif   /* End `$INSTANCE_NAME`_DMA8_REMOVE */


/***************************************
* External data references
***************************************/

extern volatile uint8 `$INSTANCE_NAME`_configuration;
extern volatile uint8 `$INSTANCE_NAME`_configurationChanged;
extern volatile uint8 `$INSTANCE_NAME`_interfaceSetting[];
extern volatile uint8 `$INSTANCE_NAME`_interfaceSetting_last[];
extern volatile uint8 `$INSTANCE_NAME`_deviceAddress;
extern volatile uint8 `$INSTANCE_NAME`_deviceStatus;
extern volatile uint8 `$INSTANCE_NAME`_device;
extern volatile uint8 `$INSTANCE_NAME`_transferState;
extern volatile uint8 `$INSTANCE_NAME`_lastPacketSize;

extern volatile T_`$INSTANCE_NAME`_EP_CTL_BLOCK `$INSTANCE_NAME`_EP[];

#if defined(`$INSTANCE_NAME`_ENABLE_HID_CLASS)
    extern volatile uint8 `$INSTANCE_NAME`_hidProtocol[];
#endif /* `$INSTANCE_NAME`_ENABLE_HID_CLASS */


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
#if(`$INSTANCE_NAME`_SOF_ISR_REMOVE == 0u)
    CY_ISR_PROTO(`$INSTANCE_NAME`_SOF_ISR);
#endif /* End `$INSTANCE_NAME`_SOF_ISR_REMOVE */
#if(`$INSTANCE_NAME`_EP_MM != `$INSTANCE_NAME`__EP_MANUAL)
    CY_ISR_PROTO(`$INSTANCE_NAME`_ARB_ISR);
#endif /* End `$INSTANCE_NAME`_EP_MM */


/***************************************
* Global data allocation
***************************************/

uint8 `$INSTANCE_NAME`_initVar = 0u;
#if(`$INSTANCE_NAME`_EP_MM != `$INSTANCE_NAME`__EP_MANUAL)
    uint8 `$INSTANCE_NAME`_DmaChan[`$INSTANCE_NAME`_MAX_EP];
    uint8 `$INSTANCE_NAME`_DmaTd[`$INSTANCE_NAME`_MAX_EP];
#endif /* End `$INSTANCE_NAME`_EP_MM */


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
*
* Summary:
*  This function initialize the USB SIE, arbiter and the
*  endpoint APIs, including setting the D+ Pullup
*
* Parameters:
*  device: Contains the device number of the desired device descriptor.
*          The device number can be found in the Device Descriptor Tab of 
*          "Configure" dialog, under the settings of desired Device Descriptor,
*          in the "Device Number" field.
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
void `$INSTANCE_NAME`_Start(uint8 device, uint8 mode) `=ReentrantKeil($INSTANCE_NAME . "_Start")`
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
void `$INSTANCE_NAME`_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_Init")`
{
    uint8 enableInterrupts;
    #if(`$INSTANCE_NAME`_EP_MM != `$INSTANCE_NAME`__EP_MANUAL)
        uint16 i;
    #endif   /* End `$INSTANCE_NAME`_EP_MM != `$INSTANCE_NAME`__EP_MANUAL */

    enableInterrupts = CyEnterCriticalSection();

    /* Enable USB block  */
    `$INSTANCE_NAME`_PM_ACT_CFG_REG |= `$INSTANCE_NAME`_PM_ACT_EN_FSUSB;
    /* Enable USB block for Standby Power Mode */
    `$INSTANCE_NAME`_PM_STBY_CFG_REG |= `$INSTANCE_NAME`_PM_STBY_EN_FSUSB;

    #if(CY_PSOC5A)
        /* Disable USBIO for TO3 */
        `$INSTANCE_NAME`_PM_AVAIL_CR_REG &= ~`$INSTANCE_NAME`_PM_AVAIL_EN_FSUSBIO;
    #endif /* End CY_PSOC5A */

    /* Enable core clock */
    `$INSTANCE_NAME`_USB_CLK_EN_REG = `$INSTANCE_NAME`_USB_CLK_ENABLE;

    `$INSTANCE_NAME`_CR1_REG = `$INSTANCE_NAME`_CR1_ENABLE_LOCK;

    #if(CY_PSOC5A)
        /* Enable USBIO for TO3 */
        `$INSTANCE_NAME`_PM_AVAIL_CR_REG |= `$INSTANCE_NAME`_PM_AVAIL_EN_FSUSBIO;
        /* Bus Reset Length, It has correct default value in ES3 */
        `$INSTANCE_NAME`_BUS_RST_CNT_REG = `$INSTANCE_NAME`_BUS_RST_COUNT;
    #endif /* End CY_PSOC5A */

    /* ENABLING USBIO PADS IN USB MODE FROM I/O MODE */
    #if(!CY_PSOC5A)
        /* Ensure USB transmit enable is low (USB_USBIO_CR0.ten). - Manual Transmission - Disabled */
        `$INSTANCE_NAME`_USBIO_CR0_REG &= ~`$INSTANCE_NAME`_USBIO_CR0_TEN;
        CyDelayUs(0);  /*~50ns delay */
        /* Disable the USBIO by asserting PM.USB_CR0.fsusbio_pd_n(Inverted)
        *  high. This will have been set low by the power manger out of reset.
        *  Also confirm USBIO pull-up disabled
        */
        `$INSTANCE_NAME`_PM_USB_CR0_REG &= ~(`$INSTANCE_NAME`_PM_USB_CR0_PD_N |`$INSTANCE_NAME`_PM_USB_CR0_PD_PULLUP_N);

        /* Select iomode to USB mode*/
        `$INSTANCE_NAME`_USBIO_CR1_REG &= ~ `$INSTANCE_NAME`_USBIO_CR1_IOMODE;

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

    #endif /* End !CY_PSOC5A */

    /* Write WAx */
    CY_SET_REG8(&`$INSTANCE_NAME`_ARB_RW1_WA_PTR[0u],     0u);
    CY_SET_REG8(&`$INSTANCE_NAME`_ARB_RW1_WA_MSB_PTR[0u], 0u);

    #if(`$INSTANCE_NAME`_EP_MM != `$INSTANCE_NAME`__EP_MANUAL)
        /* Init transfer descriptor. This will be used to detect the DMA state - initialized or not. */
        for (i = 0u; i < `$INSTANCE_NAME`_MAX_EP; i++)
        {
            `$INSTANCE_NAME`_DmaTd[i] = DMA_INVALID_TD;
        }
    #endif   /* End `$INSTANCE_NAME`_EP_MM != `$INSTANCE_NAME`__EP_MANUAL */

    CyExitCriticalSection(enableInterrupts);


    /* Set the bus reset Interrupt. */
    CyIntSetVector(`$INSTANCE_NAME`_BUS_RESET_VECT_NUM,   `$INSTANCE_NAME`_BUS_RESET_ISR);
    CyIntSetPriority(`$INSTANCE_NAME`_BUS_RESET_VECT_NUM, `$INSTANCE_NAME`_BUS_RESET_PRIOR);

    /* Set the SOF Interrupt. */
    #if(`$INSTANCE_NAME`_SOF_ISR_REMOVE == 0u)
        CyIntSetVector(`$INSTANCE_NAME`_SOF_VECT_NUM,   `$INSTANCE_NAME`_SOF_ISR);
        CyIntSetPriority(`$INSTANCE_NAME`_SOF_VECT_NUM, `$INSTANCE_NAME`_SOF_PRIOR);
    #endif   /* End `$INSTANCE_NAME`_SOF_ISR_REMOVE */

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

    #if((`$INSTANCE_NAME`_EP_MM != `$INSTANCE_NAME`__EP_MANUAL) && (`$INSTANCE_NAME`_ARB_ISR_REMOVE == 0u))
        /* Set the ARB Interrupt. */
        CyIntSetVector(`$INSTANCE_NAME`_ARB_VECT_NUM,   `$INSTANCE_NAME`_ARB_ISR);
        CyIntSetPriority(`$INSTANCE_NAME`_ARB_VECT_NUM, `$INSTANCE_NAME`_ARB_PRIOR);
    #endif   /* End `$INSTANCE_NAME`_EP_MM != `$INSTANCE_NAME`__EP_MANUAL */

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
*  device: Contains the device number of the desired device descriptor.
*          The device number can be found in the Device Descriptor Tab of 
*          "Configure" dialog, under the settings of desired Device Descriptor,
*          in the "Device Number" field.
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
*   `$INSTANCE_NAME`_device: Contains the device number of the desired device
*       descriptor. The device number can be found in the Device Descriptor Tab 
*       of "Configure" dialog, under the settings of desired Device Descriptor,
*       in the "Device Number" field.
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
void `$INSTANCE_NAME`_InitComponent(uint8 device, uint8 mode) `=ReentrantKeil($INSTANCE_NAME . "_InitComponent")`
{
    /* Initialize _hidProtocol variable to comply with
    *  HID 7.2.6 Set_Protocol Request:
    *  "When initialized, all devices default to report protocol."
    */
    #if defined(`$INSTANCE_NAME`_ENABLE_HID_CLASS)
        uint8 interface;

        for (interface = 0u; interface < `$INSTANCE_NAME`_MAX_INTERFACES_NUMBER; interface++)
        {
            `$INSTANCE_NAME`_hidProtocol[interface] = `$INSTANCE_NAME`_PROTOCOL_REPORT;
        }
    #endif /* `$INSTANCE_NAME`_ENABLE_HID_CLASS */

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
    #if((`$INSTANCE_NAME`_EP_MM != `$INSTANCE_NAME`__EP_MANUAL) && (`$INSTANCE_NAME`_ARB_ISR_REMOVE == 0u))
        /* usb arb interrupt enable */
        `$INSTANCE_NAME`_ARB_INT_EN_REG = `$INSTANCE_NAME`_ARB_INT_MASK;
        CyIntEnable(`$INSTANCE_NAME`_ARB_VECT_NUM);
    #endif   /* End `$INSTANCE_NAME`_EP_MM != `$INSTANCE_NAME`__EP_MANUAL */

    /* Arbiter congiguration for DMA transfers */
    #if(`$INSTANCE_NAME`_EP_MM != `$INSTANCE_NAME`__EP_MANUAL)

        #if(`$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_DMAMANUAL)
            `$INSTANCE_NAME`_ARB_CFG_REG = `$INSTANCE_NAME`_ARB_CFG_MANUAL_DMA;
        #endif   /* End `$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_DMAMANUAL */
        #if(`$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_DMAAUTO)
            /*Set cfg cmplt this rises DMA request when the full configuration is done */
            `$INSTANCE_NAME`_ARB_CFG_REG = `$INSTANCE_NAME`_ARB_CFG_AUTO_DMA | `$INSTANCE_NAME`_ARB_CFG_AUTO_MEM;
        #endif   /* End `$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_DMAAUTO */
    #endif   /* End `$INSTANCE_NAME`_EP_MM != `$INSTANCE_NAME`__EP_MANUAL */

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
            #if(`$INSTANCE_NAME`_VDDD_MV < `$INSTANCE_NAME`_3500MV)
                `$INSTANCE_NAME`_CR1_REG = `$INSTANCE_NAME`_CR1_ENABLE_LOCK;
            #else
                `$INSTANCE_NAME`_CR1_REG = `$INSTANCE_NAME`_CR1_ENABLE_LOCK | `$INSTANCE_NAME`_CR1_REG_ENABLE;
            #endif /* End `$INSTANCE_NAME`_VDDD_MV < `$INSTANCE_NAME`_3500MV */
            break;
    }

    /* Record the descriptor selection */
    `$INSTANCE_NAME`_device = device;

    /* Clear all of the component data */
    `$INSTANCE_NAME`_configuration = 0u;
    `$INSTANCE_NAME`_configurationChanged = 0u;
    `$INSTANCE_NAME`_deviceAddress  = 0u;
    `$INSTANCE_NAME`_deviceStatus = 0u;

    `$INSTANCE_NAME`_lastPacketSize = 0u;

    /*  ACK Setup, Stall IN/OUT */
    CY_SET_REG8(`$INSTANCE_NAME`_EP0_CR_PTR, `$INSTANCE_NAME`_MODE_STALL_IN_OUT);

    /* Enable the SIE with an address 0 */
    CY_SET_REG8(`$INSTANCE_NAME`_CR0_PTR, `$INSTANCE_NAME`_CR0_ENABLE);

    /* Workarond for PSOC5LP */
    CyDelayCycles(1);
    
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
*   `$INSTANCE_NAME`_device: Contains the device number of the desired device 
*		descriptor. The device number can be found in the Device Descriptor Tab 
*       of "Configure" dialog, under the settings of desired Device Descriptor,
*       in the "Device Number" field.
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
void `$INSTANCE_NAME`_ReInitComponent() `=ReentrantKeil($INSTANCE_NAME . "_ReInitComponent")`
{
    /* Initialize _hidProtocol variable to comply with HID 7.2.6 Set_Protocol
    *  Request: "When initialized, all devices default to report protocol."
    */
    #if defined(`$INSTANCE_NAME`_ENABLE_HID_CLASS)
        uint8 interface;

        for (interface = 0u; interface < `$INSTANCE_NAME`_MAX_INTERFACES_NUMBER; interface++)
        {
            `$INSTANCE_NAME`_hidProtocol[interface] = `$INSTANCE_NAME`_PROTOCOL_REPORT;
        }
    #endif /* `$INSTANCE_NAME`_ENABLE_HID_CLASS */

    `$INSTANCE_NAME`_transferState = `$INSTANCE_NAME`_TRANS_STATE_IDLE;

    /* Clear all of the component data */
    `$INSTANCE_NAME`_configuration = 0u;
    `$INSTANCE_NAME`_configurationChanged = 0u;
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
* Global variables:
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
*   `$INSTANCE_NAME`_configurationChanged: This variable is set to one after
*       SET_CONFIGURATION request and cleared in this function.
*   `$INSTANCE_NAME`_intiVar variable is set to zero
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`
{

    #if(`$INSTANCE_NAME`_EP_MM != `$INSTANCE_NAME`__EP_MANUAL)
        `$INSTANCE_NAME`_Stop_DMA(`$INSTANCE_NAME`_MAX_EP);     /* Stop all DMAs */
    #endif   /* End `$INSTANCE_NAME`_EP_MM != `$INSTANCE_NAME`__EP_MANUAL */

    /* Disable the SIE */
    `$INSTANCE_NAME`_CR0_REG &= ~`$INSTANCE_NAME`_CR0_ENABLE;
    /* Disable the d+ pullup */
    `$INSTANCE_NAME`_USBIO_CR1_REG &= ~`$INSTANCE_NAME`_USBIO_CR1_USBPUEN;
    #if(CY_PSOC5A)
        /* Disable USBIO block*/
        `$INSTANCE_NAME`_PM_AVAIL_CR_REG &= ~`$INSTANCE_NAME`_PM_AVAIL_EN_FSUSBIO;
    #endif /* End CY_PSOC5A */    
    /* Disable USB in ACT PM */
    `$INSTANCE_NAME`_PM_ACT_CFG_REG &= ~`$INSTANCE_NAME`_PM_ACT_EN_FSUSB;
    /* Disable USB block for Standby Power Mode */
    `$INSTANCE_NAME`_PM_STBY_CFG_REG &= ~`$INSTANCE_NAME`_PM_STBY_EN_FSUSB;

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

    /* Clear all of the component data */
    `$INSTANCE_NAME`_configuration = 0u;
    `$INSTANCE_NAME`_configurationChanged = 0u;
    `$INSTANCE_NAME`_deviceAddress  = 0u;
    `$INSTANCE_NAME`_deviceStatus = 0u;
    `$INSTANCE_NAME`_initVar = 0u;

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
* Function Name: `$INSTANCE_NAME`_IsConfigurationChanged
********************************************************************************
*
* Summary:
*  Returns the clear on read configuration state. It is usefull when PC send
*  double SET_CONFIGURATION request with same configuration number.
*
* Parameters:
*  None.
*
* Return:
*  Not zero value when new configuration has been changed, otherwise zero is
*  returned.
*
* Global variables:
*   `$INSTANCE_NAME`_configurationChanged: This variable is set to one after
*       SET_CONFIGURATION request and cleared in this function.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_IsConfigurationChanged(void) `=ReentrantKeil($INSTANCE_NAME . "_IsConfigurationChanged")`
{
    uint8 res = 0u;

    if(`$INSTANCE_NAME`_configurationChanged != 0u)
    {
        res = `$INSTANCE_NAME`_configurationChanged;
        `$INSTANCE_NAME`_configurationChanged = 0u;
    }

    return(res);
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
uint8  `$INSTANCE_NAME`_GetInterfaceSetting(uint8 interfaceNumber)
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

        count = ((uint16)((CY_GET_REG8(&`$INSTANCE_NAME`_SIE_EP1_CNT0_PTR[ri]) & `$INSTANCE_NAME`_EPX_CNT0_MASK) << 8u)
                         | CY_GET_REG8(&`$INSTANCE_NAME`_SIE_EP1_CNT1_PTR[ri]))
                         - `$INSTANCE_NAME`_EPX_CNTX_CRC_COUNT;
    }
    return(count);
}


#if(`$INSTANCE_NAME`_EP_MM != `$INSTANCE_NAME`__EP_MANUAL)


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_InitEP_DMA
    ********************************************************************************
    *
    * Summary: This function allocate and initializes a EP DMA chanel to be used
    *     by the `$INSTANCE_NAME`_LoadInEP() or `$INSTANCE_NAME`_ReadOutEP() APIs.
    *
    * Parameters:
    *  epNumber: Contains the data endpoint number.
    *            Valid values are between 1 and 8.
    *  *pData: A pointer to a data array which will be related to the EP transfers
    *
    * Return:
    *  None.
    *
    * Reentrant:
    *  No.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_InitEP_DMA(uint8 epNumber, uint8 *pData) `=ReentrantKeil($INSTANCE_NAME . "_InitEP_DMA")`
    {
        uint16 src;
        uint16 dst;
        #if (defined(__C51__))          /* PSoC 3 - Source is Flash */
            src = HI16(CYDEV_SRAM_BASE);
            dst = HI16(CYDEV_PERIPH_BASE);
            pData = pData;
        #else                           /* PSoC 5 */
            if((`$INSTANCE_NAME`_EP[epNumber].addr & `$INSTANCE_NAME`_DIR_IN) != 0u )
            {   /* for the IN EP source is the flash memory buffer */
                src = HI16(pData);
                dst = HI16(CYDEV_PERIPH_BASE);
            }
            else
            {   /* for the OUT EP source is the SIE register */
                src = HI16(CYDEV_PERIPH_BASE);
                dst = HI16(pData);
            }
        #endif  /* End C51 */
        switch(epNumber)
        {
            case `$INSTANCE_NAME`_EP1:
                #if(`$INSTANCE_NAME`_DMA1_REMOVE == 0u)
                    `$INSTANCE_NAME`_DmaChan[epNumber] = `$INSTANCE_NAME`_ep1_DmaInitialize(
                        `$INSTANCE_NAME`_DMA_BYTES_PER_BURST, `$INSTANCE_NAME`_DMA_REQUEST_PER_BURST, src, dst);
                #endif   /* End `$INSTANCE_NAME`_DMA1_REMOVE */
                break;
            case `$INSTANCE_NAME`_EP2:
                #if(`$INSTANCE_NAME`_DMA2_REMOVE == 0u)
                    `$INSTANCE_NAME`_DmaChan[epNumber] = `$INSTANCE_NAME`_ep2_DmaInitialize(
                        `$INSTANCE_NAME`_DMA_BYTES_PER_BURST, `$INSTANCE_NAME`_DMA_REQUEST_PER_BURST, src, dst);
                #endif   /* End `$INSTANCE_NAME`_DMA2_REMOVE */
                break;
            case `$INSTANCE_NAME`_EP3:
                #if(`$INSTANCE_NAME`_DMA3_REMOVE == 0u)
                    `$INSTANCE_NAME`_DmaChan[epNumber] = `$INSTANCE_NAME`_ep3_DmaInitialize(
                        `$INSTANCE_NAME`_DMA_BYTES_PER_BURST, `$INSTANCE_NAME`_DMA_REQUEST_PER_BURST, src, dst);
                #endif   /* End `$INSTANCE_NAME`_DMA3_REMOVE */
                break;
            case `$INSTANCE_NAME`_EP4:
                #if(`$INSTANCE_NAME`_DMA4_REMOVE == 0u)
                    `$INSTANCE_NAME`_DmaChan[epNumber] = `$INSTANCE_NAME`_ep4_DmaInitialize(
                        `$INSTANCE_NAME`_DMA_BYTES_PER_BURST, `$INSTANCE_NAME`_DMA_REQUEST_PER_BURST, src, dst);
                #endif   /* End `$INSTANCE_NAME`_DMA4_REMOVE */
                break;
            case `$INSTANCE_NAME`_EP5:
                #if(`$INSTANCE_NAME`_DMA5_REMOVE == 0u)
                    `$INSTANCE_NAME`_DmaChan[epNumber] = `$INSTANCE_NAME`_ep5_DmaInitialize(
                        `$INSTANCE_NAME`_DMA_BYTES_PER_BURST, `$INSTANCE_NAME`_DMA_REQUEST_PER_BURST, src, dst);
                #endif   /* End `$INSTANCE_NAME`_DMA5_REMOVE */
                break;
            case `$INSTANCE_NAME`_EP6:
                #if(`$INSTANCE_NAME`_DMA6_REMOVE == 0u)
                    `$INSTANCE_NAME`_DmaChan[epNumber] = `$INSTANCE_NAME`_ep6_DmaInitialize(
                        `$INSTANCE_NAME`_DMA_BYTES_PER_BURST, `$INSTANCE_NAME`_DMA_REQUEST_PER_BURST, src, dst);
                #endif   /* End `$INSTANCE_NAME`_DMA6_REMOVE */
                break;
            case `$INSTANCE_NAME`_EP7:
                #if(`$INSTANCE_NAME`_DMA7_REMOVE == 0u)
                    `$INSTANCE_NAME`_DmaChan[epNumber] = `$INSTANCE_NAME`_ep7_DmaInitialize(
                        `$INSTANCE_NAME`_DMA_BYTES_PER_BURST, `$INSTANCE_NAME`_DMA_REQUEST_PER_BURST, src, dst);
                #endif   /* End `$INSTANCE_NAME`_DMA7_REMOVE */
                break;
            case `$INSTANCE_NAME`_EP8:
                #if(`$INSTANCE_NAME`_DMA8_REMOVE == 0u)
                    `$INSTANCE_NAME`_DmaChan[epNumber] = `$INSTANCE_NAME`_ep8_DmaInitialize(
                        `$INSTANCE_NAME`_DMA_BYTES_PER_BURST, `$INSTANCE_NAME`_DMA_REQUEST_PER_BURST, src, dst);
                #endif   /* End `$INSTANCE_NAME`_DMA8_REMOVE */
                break;
            default:
                /* Do not support EP0 DMA transfers */
                break;
        }
        if((epNumber > `$INSTANCE_NAME`_EP0) && (epNumber < `$INSTANCE_NAME`_MAX_EP))
        {
            `$INSTANCE_NAME`_DmaTd[epNumber] = CyDmaTdAllocate();
        }
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_Stop_DMA
    ********************************************************************************
    *
    * Summary: Stops and free DMA
    *
    * Parameters:
    *  epNumber: Contains the data endpoint number or
    *           `$INSTANCE_NAME`_MAX_EP to stop all DMAs
    *
    * Return:
    *  None.
    *
    * Reentrant:
    *  No.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_Stop_DMA(uint8 epNumber) `=ReentrantKeil($INSTANCE_NAME . "_Stop_DMA")`
    {
        uint8 i;
        i = (epNumber < `$INSTANCE_NAME`_MAX_EP) ? epNumber : `$INSTANCE_NAME`_EP1;
        do
        {
            if(`$INSTANCE_NAME`_DmaTd[i] != DMA_INVALID_TD)
            {
                CyDmaChDisable(`$INSTANCE_NAME`_DmaChan[i]);
                CyDmaTdFree(`$INSTANCE_NAME`_DmaTd[i]);
                `$INSTANCE_NAME`_DmaTd[i] = DMA_INVALID_TD;
            }
            i++;
        }while((i < `$INSTANCE_NAME`_MAX_EP) && (epNumber == `$INSTANCE_NAME`_MAX_EP));
    }

#endif /* End `$INSTANCE_NAME`_EP_MM != `$INSTANCE_NAME`__EP_MANUAL */


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
                                                                        `=ReentrantKeil($INSTANCE_NAME . "_LoadInEP")`
{
    uint8 ri;
    uint8 *p;
    #if(`$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_MANUAL)
        uint16 i;
    #endif /* End `$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_MANUAL */

    if((epNumber > `$INSTANCE_NAME`_EP0) && (epNumber < `$INSTANCE_NAME`_MAX_EP))
    {
        ri = ((epNumber - `$INSTANCE_NAME`_EP1) << `$INSTANCE_NAME`_EPX_CNTX_ADDR_SHIFT);
        p = (uint8 *)&`$INSTANCE_NAME`_ARB_RW1_DR_PTR[ri];

        #if(`$INSTANCE_NAME`_EP_MM != `$INSTANCE_NAME`__EP_DMAAUTO)
            /* Limits length to available buffer space, auto MM could send packets up to 1024 bytes */
            if(length > `$INSTANCE_NAME`_EPX_DATA_BUF_MAX - `$INSTANCE_NAME`_EP[epNumber].buffOffset)
            {
                length = `$INSTANCE_NAME`_EPX_DATA_BUF_MAX - `$INSTANCE_NAME`_EP[epNumber].buffOffset;
            }
        #endif /* End `$INSTANCE_NAME`_EP_MM != `$INSTANCE_NAME`__EP_DMAAUTO */

        /* Set the count and data toggle */
        CY_SET_REG8(&`$INSTANCE_NAME`_SIE_EP1_CNT0_PTR[ri], (length >> 8u) | (`$INSTANCE_NAME`_EP[epNumber].epToggle));
        CY_SET_REG8(&`$INSTANCE_NAME`_SIE_EP1_CNT1_PTR[ri],  length & 0xFFu);

        #if(`$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_MANUAL)
            if(pData != NULL)
            {
                /* Copy the data using the arbiter data register */
                for (i = 0u; i < length; i++)
                {
                    CY_SET_REG8(p, pData[i]);
                }
            }
            `$INSTANCE_NAME`_EP[epNumber].apiEpState = `$INSTANCE_NAME`_NO_EVENT_PENDING;
            /* Write the Mode register */
            `$INSTANCE_NAME`_SIE_EP1_CR0_PTR[ri] = `$INSTANCE_NAME`_EP[epNumber].epMode;
        #else
            /*Init DMA if it was not initialized */
            if(`$INSTANCE_NAME`_DmaTd[epNumber] == DMA_INVALID_TD)
            {
                `$INSTANCE_NAME`_InitEP_DMA(epNumber, pData);
            }
        #endif /* End `$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_MANUAL */

        #if(`$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_DMAMANUAL)
            if((pData != NULL) && (length > 0))
            {

                /* Enable DMA in mode2 for transfering data */
                CyDmaChDisable(`$INSTANCE_NAME`_DmaChan[epNumber]);
                CyDmaTdSetConfiguration(`$INSTANCE_NAME`_DmaTd[epNumber], length, DMA_INVALID_TD,\
                                                                                    TD_TERMIN_EN | TD_INC_SRC_ADR);
                CyDmaTdSetAddress(`$INSTANCE_NAME`_DmaTd[epNumber],  LO16((uint32)pData), LO16((uint32)p));
                /* Enable the DMA */
                CyDmaChSetInitialTd(`$INSTANCE_NAME`_DmaChan[epNumber], `$INSTANCE_NAME`_DmaTd[epNumber]);
                CyDmaChEnable(`$INSTANCE_NAME`_DmaChan[epNumber], 1);
                /* Generate DMA request */
                `$INSTANCE_NAME`_ARB_EP1_CFG_PTR[ri] |= `$INSTANCE_NAME`_ARB_EPX_CFG_DMA_REQ;
                `$INSTANCE_NAME`_ARB_EP1_CFG_PTR[ri] &= ~`$INSTANCE_NAME`_ARB_EPX_CFG_DMA_REQ;
                /* Mode register will be writen in arb ISR after DMA transfer copmlete */
            }
            else
            {
                /* When zero-length packet - write the Mode register directly */
                `$INSTANCE_NAME`_SIE_EP1_CR0_PTR[ri] = `$INSTANCE_NAME`_EP[epNumber].epMode;
            }
        #endif /* End `$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_DMAMANUAL */

        #if(`$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_DMAAUTO)
            if(pData != NULL)
            {
                /* Enable DMA in mode3 for transfering data */
                CyDmaChDisable(`$INSTANCE_NAME`_DmaChan[epNumber]);
                CyDmaTdSetConfiguration(`$INSTANCE_NAME`_DmaTd[epNumber], length, `$INSTANCE_NAME`_DmaTd[epNumber],\
                                                                                    TD_TERMIN_EN | TD_INC_SRC_ADR);
                CyDmaTdSetAddress(`$INSTANCE_NAME`_DmaTd[epNumber],  LO16((uint32)pData), LO16((uint32)p));
                /* Clear Any potential pending DMA requests before starting the DMA channel to transfer data */
                CyDmaClearPendingDrq(`$INSTANCE_NAME`_DmaChan[epNumber]);
                /* Enable the DMA */
                CyDmaChSetInitialTd(`$INSTANCE_NAME`_DmaChan[epNumber], `$INSTANCE_NAME`_DmaTd[epNumber]);
                CyDmaChEnable(`$INSTANCE_NAME`_DmaChan[epNumber], 1);
            }
            else
            {
                `$INSTANCE_NAME`_EP[epNumber].apiEpState = `$INSTANCE_NAME`_NO_EVENT_PENDING;
                if(length > 0)
                {
                    /* Set Data ready status, This will generate DMA request */
                    `$INSTANCE_NAME`_ARB_EP1_CFG_PTR[ri] |= `$INSTANCE_NAME`_ARB_EPX_CFG_IN_DATA_RDY;
                    /* Mode register will be writen in arb ISR(In Buffer Full) after first DMA transfer copmlete */
                }
                else
                {
                    /* When zero-length packet - write the Mode register directly */
                    `$INSTANCE_NAME`_SIE_EP1_CR0_PTR[ri] = `$INSTANCE_NAME`_EP[epNumber].epMode;
                }
            }
        #endif /* End `$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_DMAAUTO */

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
*          it into data array. Valid values are between 0 and 1023. The function
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
                                                                        `=ReentrantKeil($INSTANCE_NAME . "_ReadOutEP")`
{
    uint8 ri;
    uint8 *p;
    #if(`$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_MANUAL)
        uint16 i;
    #endif /* End `$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_MANUAL */
    #if(`$INSTANCE_NAME`_EP_MM != `$INSTANCE_NAME`__EP_DMAAUTO)
        uint16 xferCount;
    #endif /* End `$INSTANCE_NAME`_EP_MM != $INSTANCE_NAME`__EP_DMAAUTO */

    if((epNumber > `$INSTANCE_NAME`_EP0) && (epNumber < `$INSTANCE_NAME`_MAX_EP) && (pData != NULL))
    {
        ri = ((epNumber - `$INSTANCE_NAME`_EP1) << `$INSTANCE_NAME`_EPX_CNTX_ADDR_SHIFT);
        p = (uint8 *)&`$INSTANCE_NAME`_ARB_RW1_DR_PTR[ri];

        #if(`$INSTANCE_NAME`_EP_MM != `$INSTANCE_NAME`__EP_DMAAUTO)
            /* Determine which is smaller the requested data or the available data */
            xferCount = `$INSTANCE_NAME`_GetEPCount(epNumber);
            if (length > xferCount)
            {
                length = xferCount;
            }
        #endif /* End `$INSTANCE_NAME`_EP_MM != `$INSTANCE_NAME`__EP_DMAAUTO */

        #if(`$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_MANUAL)
            /* Copy the data using the arbiter data register */
            for (i = 0u; i < length; i++)
            {
                pData[i] = CY_GET_REG8(p);
            }

            /* (re)arming of OUT endpoint */
            `$INSTANCE_NAME`_EnableOutEP(epNumber);
        #else
            /*Init DMA if it was not initialized */
            if(`$INSTANCE_NAME`_DmaTd[epNumber] == DMA_INVALID_TD)
            {
                `$INSTANCE_NAME`_InitEP_DMA(epNumber, pData);
            }
        #endif /* End `$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_MANUAL */

        #if(`$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_DMAMANUAL)
            /* Enable DMA in mode2 for transfering data */
            CyDmaChDisable(`$INSTANCE_NAME`_DmaChan[epNumber]);
            CyDmaTdSetConfiguration(`$INSTANCE_NAME`_DmaTd[epNumber], length, DMA_INVALID_TD,
                                                                                TD_TERMIN_EN | TD_INC_DST_ADR);
            CyDmaTdSetAddress(`$INSTANCE_NAME`_DmaTd[epNumber],  LO16((uint32)p), LO16((uint32)pData));
            /* Enable the DMA */
            CyDmaChSetInitialTd(`$INSTANCE_NAME`_DmaChan[epNumber], `$INSTANCE_NAME`_DmaTd[epNumber]);
            CyDmaChEnable(`$INSTANCE_NAME`_DmaChan[epNumber], 1);

            /* Generate DMA request */
            `$INSTANCE_NAME`_ARB_EP1_CFG_PTR[ri] |= `$INSTANCE_NAME`_ARB_EPX_CFG_DMA_REQ;
            `$INSTANCE_NAME`_ARB_EP1_CFG_PTR[ri] &= ~`$INSTANCE_NAME`_ARB_EPX_CFG_DMA_REQ;
            /* Out EP will be (re)armed in arb ISR after transfer complete */
        #endif /* End `$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_DMAMANUAL */

        #if(`$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_DMAAUTO)
            /* Enable DMA in mode3 for transfering data */
            CyDmaChDisable(`$INSTANCE_NAME`_DmaChan[epNumber]);
            CyDmaTdSetConfiguration(`$INSTANCE_NAME`_DmaTd[epNumber], length, `$INSTANCE_NAME`_DmaTd[epNumber],
                                                                                TD_TERMIN_EN | TD_INC_DST_ADR);
            CyDmaTdSetAddress(`$INSTANCE_NAME`_DmaTd[epNumber],  LO16((uint32)p), LO16((uint32)pData));

            /* Clear Any potential pending DMA requests before starting the DMA channel to transfer data */
            CyDmaClearPendingDrq(`$INSTANCE_NAME`_DmaChan[epNumber]);
            /* Enable the DMA */
            CyDmaChSetInitialTd(`$INSTANCE_NAME`_DmaChan[epNumber], `$INSTANCE_NAME`_DmaTd[epNumber]);
            CyDmaChEnable(`$INSTANCE_NAME`_DmaChan[epNumber], 1);
            /* Out EP will be (re)armed in arb ISR after transfer complete */
        #endif /* End `$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_DMAAUTO */

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
void `$INSTANCE_NAME`_EnableOutEP(uint8 epNumber) `=ReentrantKeil($INSTANCE_NAME . "_EnableOutEP")`
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
void `$INSTANCE_NAME`_SetPowerStatus(uint8 powerStatus) `=ReentrantKeil($INSTANCE_NAME . "_SetPowerStatus")`
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
    *  Determines VBUS presence for Self Powered Devices.
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
