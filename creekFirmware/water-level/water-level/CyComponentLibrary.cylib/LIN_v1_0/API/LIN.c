/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file contains implementation of LIN Slave component.
*
********************************************************************************
* Copyright 2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"


/*******************************************************************************
*  Place your includes, defines and code here
*******************************************************************************/
/* `#START `$INSTANCE_NAME`_DECLARATIONS` */

/* `#END` */

/* Internal APIs */
l_u8    `$INSTANCE_NAME`_FindPidIndex(l_u8)`=ReentrantKeil($INSTANCE_NAME . "_FindPidIndex")`;
void    `$INSTANCE_NAME`_EndFrame(l_u8);
void    `$INSTANCE_NAME`_ProcessStatusFlags(l_u8);
void    `$INSTANCE_NAME`_SetAssociatedFlags(l_u8);
l_bool  `$INSTANCE_NAME`_GetEtFlagValue(l_u8)`=ReentrantKeil($INSTANCE_NAME . "_GetEtFlagValue")`;
void    `$INSTANCE_NAME`_ClearEtFlagValue(l_u8);

#if(1u == `$INSTANCE_NAME`_TL_ENABLED)

    void    `$INSTANCE_NAME`_ProcessMrf(volatile l_u8*);

    #if(1u == `$INSTANCE_NAME`_CS_ENABLED)

        l_bool `$INSTANCE_NAME`_LinProductId(volatile l_u8*);

        #if(1u == `$INSTANCE_NAME`_LIN_2_0)

            l_u8 `$INSTANCE_NAME`_MessageId(volatile l_u8*);

        #endif /* (1u == `$INSTANCE_NAME`_LIN_2_0) */

    #endif /* (1u == `$INSTANCE_NAME`_CS_ENABLED) */

    /* RAM copy of the slave NAD */
    volatile l_u8 `$INSTANCE_NAME`_nad;

#endif /* (1u == `$INSTANCE_NAME`_TL_ENABLED) */

`$messageID`

/* Retention registers are stored here */
static `$INSTANCE_NAME`_BACKUP_STRUCT  `$INSTANCE_NAME`_backup = {0u, };

#if(1u == `$INSTANCE_NAME`_INACTIVITY_ENABLED)

    /* Free-running timer */
    l_u8    `$INSTANCE_NAME`_periodCounter;

#endif /* (1u == `$INSTANCE_NAME`_INACTIVITY_ENABLED) */

volatile l_u8  `$INSTANCE_NAME`_status;       /* Internal Status                  */
volatile l_u8  `$INSTANCE_NAME`_syncCounts;   /* Sync Field Timer Counts          */
volatile l_u8  `$INSTANCE_NAME`_auxStatus;    /* Internal AUX ISR shadow status   */
volatile l_u16 `$INSTANCE_NAME`_ioctlStatus;  /* Status used by l_ifc_ioctl()     */
volatile l_u16 `$INSTANCE_NAME`_ifcStatus;    /* Interface communication status   */
volatile l_u8  `$INSTANCE_NAME`_uartFsmState; /* Current state of the UART ISR    */
volatile l_u8  `$INSTANCE_NAME`_fsmFlags;

/* RXed data before checksum checked */
volatile l_u8    `$INSTANCE_NAME`_tmpRxFrameData[8u];

/* Pointer to frame data. Points to the byte to be sent.*/
volatile l_u8*   `$INSTANCE_NAME`_frameData = NULL;

#if(1u == `$INSTANCE_NAME`_AUTO_BAUD_RATE_SYNC)

    /* Initial clock divider */
    l_u16   `$INSTANCE_NAME`_initialClockDivider;

#endif  /* (1u == `$INSTANCE_NAME`_AUTO_BAUD_RATE_SYNC) */

#if(1u == `$INSTANCE_NAME`_SAE_J2602)

    /* J2602 status variable */
    volatile l_u8 `$INSTANCE_NAME`_j2602Status;

#endif  /* (1u == `$INSTANCE_NAME`_SAE_J2602) */

/* Notification API statuses */
l_u8 `$INSTANCE_NAME`_statusFlagArray[`$INSTANCE_NAME`_SIG_FRAME_FLAGS_SIZE];
l_u8 `$INSTANCE_NAME`_etFrameFlags[`$INSTANCE_NAME`_ET_FRAMES_FLAGS_SIZE];

/* RAM copy of the slave configuration data */
volatile l_u8 `$INSTANCE_NAME`_volatileConfig[`$INSTANCE_NAME`_NUM_FRAMES];


/****************************************************
*   Transport Layer API
*****************************************************/
#if(1u == `$INSTANCE_NAME`_TL_ENABLED)

    #if(1u == `$INSTANCE_NAME`_CS_ENABLED)

        /* LIN Slave Identification */
        const `$INSTANCE_NAME`_SLAVE_ID CYCODE `$INSTANCE_NAME`_slaveId = \
        {
            `$INSTANCE_NAME`_CS_SUPPLIER_ID,
            `$INSTANCE_NAME`_CS_FUNCTION_ID,
            `$INSTANCE_NAME`_CS_VARIANT
        };

        /* Serial Number */
        l_u8*   serialNumber = NULL;

    #endif /* (1u == `$INSTANCE_NAME`_CS_ENABLED) */

    /* SRF and MRF frame buffers */
    volatile l_u8 `$INSTANCE_NAME`_mrfBuffer[`$INSTANCE_NAME`_FRAME_BUFF_LEN];
    volatile l_u8 `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_FRAME_BUFF_LEN];

    /* Transport Layer Rx and Tx Statuses */
    volatile l_u8   `$INSTANCE_NAME`_txTlStatus;
    volatile l_u8   `$INSTANCE_NAME`_rxTlStatus;

    /* Internal variable used to store the PCI of the previous frame */
    volatile l_u8   `$INSTANCE_NAME`_prevPci;

    /* Flags that are used for Transport Layer */
    volatile l_u8   `$INSTANCE_NAME`_tlFlags;

    #if(1u == `$INSTANCE_NAME`_TL_API_FORMAT)

        volatile l_u8*   `$INSTANCE_NAME`_txTlDataPointer     = NULL;
        volatile l_u8*   `$INSTANCE_NAME`_rxTlDataPointer     = NULL;
        volatile l_u8*   `$INSTANCE_NAME`_rxTlInitDataPointer = NULL;
        volatile l_u8*   `$INSTANCE_NAME`_tlNadPointer      = NULL;
        volatile l_u16*  `$INSTANCE_NAME`_tlLengthPointer   = NULL;

        volatile l_u16   `$INSTANCE_NAME`_txMessageLength = 0u;
        volatile l_u16   `$INSTANCE_NAME`_rxMessageLength = 0u;

        /* Internal variables */
        static volatile l_u8 `$INSTANCE_NAME`_txFrameCounter = 0u;
        static volatile l_u8 `$INSTANCE_NAME`_rxFrameCounter = 0u;
        static volatile l_u8 `$INSTANCE_NAME`_tlTimeoutCnt   = 0u;

    #else

        /* Internal variables for buffer indexing */
        l_u8 `$INSTANCE_NAME`_txBufDepth;
        l_u8 `$INSTANCE_NAME`_rxBufDepth;

        /* The Master Request Frame (MRF) buffer */
        volatile l_u8 `$INSTANCE_NAME`_rawRxQueue[`$INSTANCE_NAME`_TL_RX_QUEUE_LEN];

        /* The Slave Response Frame (SRF) buffer */
        volatile l_u8 `$INSTANCE_NAME`_rawTxQueue[`$INSTANCE_NAME`_TL_TX_QUEUE_LEN];

        /* TX buffer indexes */
        volatile l_u16 `$INSTANCE_NAME`_txWrIndex;
        volatile l_u16 `$INSTANCE_NAME`_txRdIndex;

        /* RX buffer indexes */
        volatile l_u16 `$INSTANCE_NAME`_rxWrIndex;
        volatile l_u16 `$INSTANCE_NAME`_rxRdIndex;

    #endif /* (1u == `$INSTANCE_NAME`_TL_API_FORMAT) */

#endif /* (1u == `$INSTANCE_NAME`_TL_ENABLED) */


/* LIN slave configuration data */
`$slaveConfig`

/* Frames declaration with initial signals values */
`$frameDataDefault`

/* PID information table */
`$pidInfoTable`


/*******************************************************************************
* Parity lookup table.  Given a six bit identifier as an index, the indexed
* value will provide the correct value with the parity bit set.
*******************************************************************************/
const l_u8 CYCODE `$INSTANCE_NAME`_parityTable[] =
{
    0x80u, 0xC1u, 0x42u, 0x03u, 0xC4u, 0x85u, 0x06u, 0x47u, 0x08u, 0x49u, 0xCAu,
    0x8Bu, 0x4Cu, 0x0Du, 0x8Eu, 0xCFu, 0x50u, 0x11u, 0x92u, 0xD3u, 0x14u, 0x55u,
    0xD6u, 0x97u, 0xD8u, 0x99u, 0x1Au, 0x5Bu, 0x9Cu, 0xDDu, 0x5Eu, 0x1Fu, 0x20u,
    0x61u, 0xE2u, 0xA3u, 0x64u, 0x25u, 0xA6u, 0xE7u, 0xA8u, 0xE9u, 0x6Au, 0x2Bu,
    0xECu, 0xADu, 0x2Eu, 0x6Fu, 0xF0u, 0xB1u, 0x32u, 0x73u, 0xB4u, 0xF5u, 0x76u,
    0x37u, 0x78u, 0x39u, 0xBAu, 0xFBu, 0x3Cu, 0x7Du, 0xFEu, 0xBFu
};


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
*
* Summary:
*  Starts the component operation. This function is not required to be used.
*
* Parameters:
*  None
*
* Return:
*  Zero     - if the initialization succeeded.
*  Non-zero - if the initialization failed.
*
* Reentrant:
*  No
*
*******************************************************************************/
l_bool `$INSTANCE_NAME`_Start(void)
{
    l_bool returnValue = `$INSTANCE_NAME`_FALSE;

    /* Call initialization function */
    returnValue = l_sys_init();

     /* Start LIN component */
    l_ifc_init_`$INSTANCE_NAME`();

    return (returnValue);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
*
* Summary:
*  Starts the component operation. This function is not required to be used.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
l_u8 `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`
{
    /* Stop UART */
    `$INSTANCE_NAME`_UART_Stop();

    /* Disable hardware blocks operation */
    `$INSTANCE_NAME`_CONTROL_REG &= ~`$INSTANCE_NAME`_CONTROL_ENABLE;

    /* Disable interrupts */
    return(l_sys_irq_disable());
}


/*******************************************************************************
* Function Name: l_sys_init
********************************************************************************
*
* Summary:
*  Performs the initialization of the LIN core. This function does nothing and
*  and always returns zero.
*
* Parameters:
*  None
*
* Return:
*  Zero     - if the initialization succeeded.
*  Non-zero - if the initialization failed.
*
*******************************************************************************/
l_bool l_sys_init(void)  `=ReentrantKeil("l_sys_init")`
{
    return (`$INSTANCE_NAME`_FALSE);
}


/* Signal interaction and notification API */
`$l_bool_rd`

`$l_u8_rd`

`$l_u16_rd`

`$l_bytes_rd`

`$l_bool_wr`

`$l_u8_wr`

`$l_u16_wr`

`$l_bytes_wr`

`$l_flg_tst`

`$l_flg_clr`


/*******************************************************************************
* Function Name: l_ifc_init_`$INSTANCE_NAME`
********************************************************************************
*
* Summary:
*  The function initializes the LIN Slave component instance that is specified
*  by the name iii. It sets up internal functions such as the baud rate and
*  starts up digital blocks that are used by the LIN Slave component. This is
*  the first call that must be performed, before using any other interface-
*  related LIN Slave API functions.
*
* Parameters:
*  None
*
* Return:
*  The 0 is returned if operation succeeded and 1 if an invalid operation
*  parameter was passed to the function.
*
* Global variables:
*  `$INSTANCE_NAME`_initialClockDivider - variable is set with initial value.
*  `$INSTANCE_NAME`_ifcStatus - interface is cleared before deing used.
*
* Reentrant:
*  No
*
*******************************************************************************/
l_bool l_ifc_init_`$INSTANCE_NAME`(void)
{
    l_u8 interruptState;

    /* Set bLIN interrupt priority */
    CyIntSetPriority(`$INSTANCE_NAME`_BLIN_ISR_NUMBER, `$INSTANCE_NAME`_BLIN_ISR_PRIORITY);

    /* Set bLIN interrupt vector */
    CyIntSetVector(`$INSTANCE_NAME`_BLIN_ISR_NUMBER, `$INSTANCE_NAME`_BLIN_ISR);

    /* Clear any pending bLIN interrupt */
    CyIntClearPending(`$INSTANCE_NAME`_BLIN_ISR_NUMBER);

    /* Clear any pending bLIN interrupt */
    CyIntClearPending(`$INSTANCE_NAME`_BLIN_ISR_NUMBER);

    /* Set UART interrupt priority */
    CyIntSetPriority(`$INSTANCE_NAME`_UART_ISR_NUMBER, `$INSTANCE_NAME`_UART_ISR_PRIORITY);

    /* Set UART interrupt vector */
    CyIntSetVector(`$INSTANCE_NAME`_UART_ISR_NUMBER, `$INSTANCE_NAME`_UART_ISR);

    /* Start UART */
    `$INSTANCE_NAME`_UART_Start();

    /* Set Break Detection Threshold in counts */
    `$INSTANCE_NAME`_BREAK_THRESHOLD_REG = `$INSTANCE_NAME`_BREAK_THRESHOLD_VALUE;

    /* Allow interrupts on break, sync and inactivity (optional) events */
    `$INSTANCE_NAME`_INT_MASK_REG |= (`$INSTANCE_NAME`_INT_MASK_BREAK     |
                                      `$INSTANCE_NAME`_INT_MASK_SYNC      |
                                      `$INSTANCE_NAME`_INT_MASK_INACTIVITY);

    interruptState = CyEnterCriticalSection();

    `$INSTANCE_NAME`_STATUS_AUX_CONTROL_REG |= `$INSTANCE_NAME`_STATUS_AUX_CONTROL_INT_EN;

    CyExitCriticalSection(interruptState);

    /* Bus inactivity block configuration */
    #if(1u == `$INSTANCE_NAME`_INACTIVITY_ENABLED)

        /* Bus inactivity block configuration to issue interrupt every 100 ms */
        `$INSTANCE_NAME`_INACTIVITY_DIV0_REG = `$INSTANCE_NAME`_INACT_DIV0;
        `$INSTANCE_NAME`_INACTIVITY_DIV1_REG = `$INSTANCE_NAME`_INACT_DIV1;

    #endif  /* (1u == `$INSTANCE_NAME`_INACTIVITY_ENABLED) */


    /* Save clock divider */
     #if(1u == `$INSTANCE_NAME`_AUTO_BAUD_RATE_SYNC)

        /* Save default clock divider */
        `$INSTANCE_NAME`_initialClockDivider = `$INSTANCE_NAME`_IntClk_GetDividerRegister();

    #endif  /* (1u == `$INSTANCE_NAME`_AUTO_BAUD_RATE_SYNC) */

    /* Enable hardware blocks operation */
    `$INSTANCE_NAME`_CONTROL_REG |= `$INSTANCE_NAME`_CONTROL_ENABLE;

    /* Copy PIDs from NVRAM to VRAM */
    memcpy((void *) &`$INSTANCE_NAME`_volatileConfig[0u],
        (void *) &`$INSTANCE_NAME`_LinSlaveConfig.pidTable[0u], (int)`$INSTANCE_NAME`_NUM_FRAMES);

    #if(1u == `$INSTANCE_NAME`_TL_ENABLED)

        `$INSTANCE_NAME`_nad = `$INSTANCE_NAME`_LinSlaveConfig.initialNad;

    #endif /* (1u == `$INSTANCE_NAME`_TL_ENABLED) */

    /*  Clear interface status */
    `$INSTANCE_NAME`_ifcStatus &= ~`$INSTANCE_NAME`_IFC_STS_MASK;

    /* Enable bLIN interrupt */
    CyIntEnable(`$INSTANCE_NAME`_BLIN_ISR_NUMBER);

    /* Initialization is always expected to succeeded */
    return (`$INSTANCE_NAME`_FALSE);
}


/*******************************************************************************
* Function Name: l_ifc_init
********************************************************************************
*
* Summary:
*  The function initializes the LIN Slave component instance that is specified
*  by the name iii. It sets up internal functions such as the baud rate and
*  starts up digital blocks that are used by the LIN Slave component. This is
*  the first call that must be performed, before using any other interface-
*  related LIN Slave API functions.
*
* Parameters:
*  iii - is the name of the interface handle.
*
* Return:
*  The 0 is returned if operation succeeded and 1 if an invalid operation
*  parameter was passed to the function.
*
* Global variables:
*  `$INSTANCE_NAME`_initialClockDivider - variable is set with initial value.
*  `$INSTANCE_NAME`_ifcStatus - interface is cleared before deing used.
*
* Reentrant:
*  No
*
*******************************************************************************/
l_bool l_ifc_init(l_ifc_handle iii)
{
    l_bool returnValue;

    switch(iii)
    {
        case `$INSTANCE_NAME`_IFC_HANDLE:
            returnValue = l_ifc_init_`$INSTANCE_NAME`();
        break;

        default:
            /* Unknown interface handle */
            returnValue = `$INSTANCE_NAME`_TRUE;
        break;
    }

    /* To remove unreferenced local variable warning */
    iii = iii;

    return (returnValue);
}


/*******************************************************************************
* Function Name: l_ifc_wake_up
********************************************************************************
*
* Summary:
*  This function transmits one wakeup signal. The wakeup signal is transmitted
*  directly when this function is called. When you call this API function, the
*  application is blocked until a wakeup signal is transmitted on the LIN bus.
*  The CyDelayUs() function is used as the timing source. The delay is
*  calculated based on the clock configuration entered in PSoC Creator.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void l_ifc_wake_up_`$INSTANCE_NAME`(void) `=ReentrantKeil("l_ifc_wake_up_" . $INSTANCE_NAME)`
{
    /* Force TXD low */
    `$INSTANCE_NAME`_CONTROL_REG |= `$INSTANCE_NAME`_CONTROL_TX_DIS;

    /* Wait */
    CyDelayUs(`$INSTANCE_NAME`_WAKE_UP_SIGNAL_LENGTH);

    /* Connect TXD from UART to LIN bus line */
    `$INSTANCE_NAME`_CONTROL_REG &= ~`$INSTANCE_NAME`_CONTROL_TX_DIS;
}


/*******************************************************************************
* Function Name: l_ifc_wake_up
********************************************************************************
*
* Summary:
*  This function transmits one wakeup signal. The wakeup signal is transmitted
*  directly when this function is called. When you call this API function, the
*  application is blocked until a wakeup signal is transmitted on the LIN bus.
*  The CyDelayUs() function is used as the timing source. The delay is
*  calculated based on the clock configuration entered in PSoC Creator.
*
* Parameters:
*  iii - is the name of the interface handle.
*
* Return:
*  None
*
*******************************************************************************/
void l_ifc_wake_up(l_ifc_handle iii)   `=ReentrantKeil("l_ifc_wake_up")`
{
    switch(iii)
    {
        case `$INSTANCE_NAME`_IFC_HANDLE:
            l_ifc_wake_up_`$INSTANCE_NAME`();
        break;

        default:
            /* Unknown interface handle - do nothing. */
        break;
    }

    /* To remove unreferenced local variable warning */
    iii = iii;
}


/*******************************************************************************
* Function Name: l_ifc_ioctl
********************************************************************************
*
* Summary:
*  This function controls functionality that is not covered by the other API
*  calls. It is used for protocol specific parameters or hardware specific
*  functionality.
*
* Parameters:
*  op - is the operation that should be applied.
*  pv - is the pointer to the optional parameter.
*
* Return:
*  There is no error code value returned for operation selected. This means that
*  you must ensure that the values passed into the function are correct.
*
*  L_IOCTL_READ_STATUS operation:
*  The first bit in this byte is the flag that indicates that there has been no
*  signaling on the bus for a certain elapsed time (available when
*  Bus Inactivity Timeout Detection option is enabled). If the elapsed time
*  passes a certain threshold, then this flag is set. Calling this API clears
*  all status bits after they are returned. The second bit is the flag that
*  indicates that a Targeted Reset service request (0xB5) was received
*  (when J2602-1 Compliance is enabled).
*
*  Symbolic Name : `$INSTANCE_NAME`_IOCTL_STS_BUS_INACTIVITY
*  Value         : 0x0001u
*  Description   : No signal was detected on the bus for a certain elapsed time
*
*  Symbolic Name : `$INSTANCE_NAME`_IOCTL_STS_TARGET_RESET
*  Value         : 0x0002u
*  Description   : Targeted Reset service request (0xB5) was received.
*
*  L_IOCTL_SET_BAUD_RATE operation:
*  The 0 is returned if operation succeeded and 1 if an invalid operation
*  parameter was passed to the function.
*
*  L_IOCTL_SLEEP operation:
*  The CYRET_SUCCESS is returned if operation succeeded and CYRET_BAD_PARAM if
*  an invalid operation parameter was passed to the function.
*
*  L_IOCTL_WAKEUP operation:
*  The CYRET_SUCCESS is returned if operation succeeded and CYRET_BAD_PARAM if
*  an invalid operation parameter was passed to the function.
*
*  L_IOCTL_SYNC_COUNTS operation:
*  The CYRET_SUCCESS is returned if operation succeeded and CYRET_BAD_PARAM if
*  an invalid operation parameter was passed to the function.
*
*  L_IOCTL_SET_SERIAL_NUMBER operation:
*  The CYRET_SUCCESS is returned if operation succeeded and CYRET_BAD_PARAM if
*  an invalid operation parameter was passed to the function.
*
* Global variables:
*  `$INSTANCE_NAME`_ioctlStatus - variable is cleared after it was returned
*  `$INSTANCE_NAME`_backup - configuration data are written/read from structure
*  serialNumber - global variable is modified
* `$INSTANCE_NAME`_syncCounts - internal variable is read
*
* Reentrant:
*  No
*
*******************************************************************************/
l_u16 l_ifc_ioctl_`$INSTANCE_NAME`(l_ioctl_op op, void* pv)
{
    l_u16 returnValue = (l_u16) CYRET_SUCCESS;
    l_u8 interruptState;

    switch (op)
    {
        /***********************************************************************
        *                           Read Status
        ***********************************************************************/
        case L_IOCTL_READ_STATUS:

            /* Return status */
            returnValue = `$INSTANCE_NAME`_ioctlStatus;

            /* Clear status */
            `$INSTANCE_NAME`_ioctlStatus = 0x0000u;

        break;


        /***********************************************************************
        *                           Set Baud Rate
        ***********************************************************************/
        case L_IOCTL_SET_BAUD_RATE:

            interruptState = CyEnterCriticalSection();

            /* Set new interanl clock divider with effect on end of cycle */
            `$INSTANCE_NAME`_IntClk_SetDividerRegister(((*((l_u16*)pv)) - 1u), 0u);

            /* Bus inactivity block reconfiguration */
            #if(1u == `$INSTANCE_NAME`_INACTIVITY_ENABLED)

                /* Divider 1 for specified interrupt rate */
                `$INSTANCE_NAME`_INACTIVITY_DIV0_REG = \
                    ((l_u8)(((`$INSTANCE_NAME`_INACT_OVERSAMPLE_RATE) * \
                    ((*(l_u16*)pv) / `$INSTANCE_NAME`_INACT_100MS_IN_S) / \
                    `$INSTANCE_NAME`_INACT_DIVIDE_FACTOR) - 1u));

                /* Divider 1 for specified interrupt rate */
                `$INSTANCE_NAME`_INACTIVITY_DIV1_REG = \
                    ((l_u8)(((`$INSTANCE_NAME`_INACT_OVERSAMPLE_RATE) * \
                    ((*(l_u16*)pv) / `$INSTANCE_NAME`_INACT_100MS_IN_S)) - \
                    (`$INSTANCE_NAME`_INACT_DIVIDE_FACTOR * \
                    `$INSTANCE_NAME`_INACTIVITY_DIV0_REG)));

            #endif  /* (1u == `$INSTANCE_NAME`_INACTIVITY_ENABLED) */

            CyExitCriticalSection(interruptState);

        break;


        /***********************************************************************
        *                   Prepare for the low power modes
        ***********************************************************************/
        case L_IOCTL_SLEEP:

            `$INSTANCE_NAME`_backup.control = `$INSTANCE_NAME`_CONTROL_REG;

            if(0u != (`$INSTANCE_NAME`_CONTROL_REG  & `$INSTANCE_NAME`_CONTROL_ENABLE))
            {
               `$INSTANCE_NAME`_backup.enableState = `$INSTANCE_NAME`_TRUE;
            }
            else
            {
                `$INSTANCE_NAME`_backup.enableState = `$INSTANCE_NAME`_FALSE;
            }

            /* Disable interrupts */
            (void) l_sys_irq_disable();

            /* Prepare UART for low power mode */
            `$INSTANCE_NAME`_UART_Sleep();

            /* Disable hardware blocks operation */
            `$INSTANCE_NAME`_CONTROL_REG &= ~`$INSTANCE_NAME`_CONTROL_ENABLE;

            #if(CY_PSOC3_ES2 || CY_PSOC5_ES1)

                `$INSTANCE_NAME`_backup.statusMask = `$INSTANCE_NAME`_INT_MASK_REG;

                #if(1u == `$INSTANCE_NAME`_INACTIVITY_ENABLED)

                    `$INSTANCE_NAME`_backup.inactivityDiv0 = `$INSTANCE_NAME`_INACTIVITY_DIV0_REG;
                    `$INSTANCE_NAME`_backup.inactivityDiv1 = `$INSTANCE_NAME`_INACTIVITY_DIV1_REG;

                #endif  /* (1u == `$INSTANCE_NAME`_INACTIVITY_ENABLED) */

            #endif  /* (CY_PSOC3_ES2 || CY_PSOC5_ES1) */

        break;


        /***********************************************************************
        *             Restore after wakeup from low power modes
        ***********************************************************************/
        case L_IOCTL_WAKEUP:

            #if(CY_PSOC3_ES2 || CY_PSOC5_ES1)

                `$INSTANCE_NAME`_INT_MASK_REG = `$INSTANCE_NAME`_backup.statusMask;

                #if(1u == `$INSTANCE_NAME`_INACTIVITY_ENABLED)

                    `$INSTANCE_NAME`_INACTIVITY_DIV0_REG = `$INSTANCE_NAME`_backup.inactivityDiv0;
                    `$INSTANCE_NAME`_INACTIVITY_DIV1_REG = `$INSTANCE_NAME`_backup.inactivityDiv1;

                #endif  /* (1u == `$INSTANCE_NAME`_INACTIVITY_ENABLED) */

            #endif  /* (CY_PSOC3_ES2 || CY_PSOC5_ES1) */

            `$INSTANCE_NAME`_CONTROL_REG = `$INSTANCE_NAME`_backup.control;

            /* Restore UART state*/
            `$INSTANCE_NAME`_UART_Wakeup();

            if(0u != `$INSTANCE_NAME`_backup.enableState)
            {
               l_sys_irq_restore(`$INSTANCE_NAME`_ALL_IRQ_ENABLE);
            }

        break;

        #if(1u == `$INSTANCE_NAME`_AUTO_BAUD_RATE_SYNC)

            case L_IOCTL_SYNC_COUNTS:

                /* Returns current number of sync field timer counts */
                returnValue = (l_u16) `$INSTANCE_NAME`_syncCounts;

            break;

        #endif  /* (1u == `$INSTANCE_NAME`_AUTO_BAUD_RATE_SYNC) */


        #if(1u == `$INSTANCE_NAME`_TL_ENABLED)
            #if(1u == `$INSTANCE_NAME`_CS_ENABLED)

                case L_IOCTL_SET_SERIAL_NUMBER:
                    serialNumber = (l_u8*) pv;
                break;

            #endif /* (1u == `$INSTANCE_NAME`_CS_ENABLED) */
        #endif /* (1u == `$INSTANCE_NAME`_TL_ENABLED) */


        default:
            /* Unknown operation */
            returnValue = (l_u16) CYRET_BAD_PARAM;
        break;
    }

    return (returnValue);
}


/*******************************************************************************
* Function Name: l_ifc_ioctl
********************************************************************************
*
* Summary:
*  This function controls functionality that is not covered by the other API
*  calls. It is used for protocol specific parameters or hardware specific
*  functionality. Example of such functionality can be to switch on/off the
*  wake up signal detection.
*
* Parameters:
*  iii - is the name of the interface handle.
*  op - is the operation that should be applied.
*  pv - is the pointer to the optional parameter.
*
* Return:
*  There is no error code value returned for operation selected. This means that
*  you must ensure that the values passed into the function are correct.
*
*  L_IOCTL_READ_STATUS operation
*  The first bit in this byte is the flag that indicates that there has been no
*  signaling on the bus for a certain elapsed time (available when
*  Bus Inactivity Timeout Detection option is enabled). If the elapsed time
*  passes a certain threshold, then this flag is set. Calling this API clears
*  all status bits after they are returned. The second bit is the flag that
*  indicates that a Targeted Reset service request (0xB5) was received
*  (when J2602-1 Compliance is enabled).
*
*  Symbolic Name : `$INSTANCE_NAME`_IOCTL_STS_BUS_INACTIVITY
*  Value         : 0x0001u
*  Description   : No signal was detected on the bus for a certain elapsed time
*
*  Symbolic Name : `$INSTANCE_NAME`_IOCTL_STS_TARGET_RESET
*  Value         : 0x0002u
*  Description   : Targeted Reset service request (0xB5) was received.
*
*  L_IOCTL_SET_BAUD_RATE operation
*  The 0 is returned if operation succeeded and 1 if an invalid operation
*  parameter was passed to the function.
*
*  L_IOCTL_SLEEP operation
*  The 0 is returned if operation succeeded and 1 if an invalid operation
*  parameter was passed to the function.
*
*  L_IOCTL_WAKEUP operation
*  The 0 is returned if operation succeeded and 1 if an invalid operation
*  parameter was passed to the function.
*
*  L_IOCTL_SYNC_COUNTS operation
*  The 0 is returned if operation succeeded and 1 if an invalid operation
*  parameter was passed to the function.
*
*  L_IOCTL_SET_SERIAL_NUMBER operation
*  The 0 is returned if operation succeeded and 1 if an invalid operation
*  parameter was passed to the function.
*
* Global variables:
*  `$INSTANCE_NAME`_ioctlStatus - variable is cleared after it was returned
*  `$INSTANCE_NAME`_backup - configuration data are written/read from structure
*  serialNumber - global variable is modified
* `$INSTANCE_NAME`_syncCounts - internal variable is read
*
* Reentrant:
*  No
*
*******************************************************************************/
l_u16 l_ifc_ioctl(l_ifc_handle iii, l_ioctl_op op, void* pv)
{
    l_u16 returnValue;

    switch(iii)
    {
        case `$INSTANCE_NAME`_IFC_HANDLE:
            returnValue = l_ifc_ioctl_`$INSTANCE_NAME`(op, pv);
        break;

        default:
            /* Unknown operation */
            returnValue = (l_u16) CYRET_BAD_PARAM;
        break;
    }

    /* To remove unreferenced local variable warning */
    iii = iii;

    return (returnValue);
}


/*******************************************************************************
* Function Name: l_ifc_rx
********************************************************************************
*
* Summary:
*  The LIN Slave component takes care of calling this API routine automatically.
*  Therefore, this API routine must not be called by the application code.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global variables:
*  `$INSTANCE_NAME`_j2602Status - appropriate statuses are set (optionally).
*  `$INSTANCE_NAME`_fsmFlags - appropriate flags of data transfer FSM are set.
*  `$INSTANCE_NAME`_ifcStatus   - appropriate interface statuses bits are set.
*  `$INSTANCE_NAME`_frameData   - pointer set to data to be transferred.
*  `$INSTANCE_NAME`_uartFsmState- next state of FSM saved.
*
* Reentrant:
*  No
*
*******************************************************************************/
void l_ifc_rx_`$INSTANCE_NAME`(void)
{
    l_u8 i;
    l_u8 interruptState;

    static l_u16 `$INSTANCE_NAME`_interimChecksum;  /* Holds interim checksum value     */
    static l_u8 `$INSTANCE_NAME`_framePid;          /* PID of the current frame         */
    static l_u8 `$INSTANCE_NAME`_frameSize;         /* Size of frame being processed    */
    static l_u8 `$INSTANCE_NAME`_bytesTransferred;  /* Number of transfered bytes       */
    static l_u8 `$INSTANCE_NAME`_tmpData;           /* Used to store transmitted byte   */
    static l_u8 `$INSTANCE_NAME`_pidIndex;          /* Index in pidInfoTable            */

    /* Check for correctness data for UART */
    if(0u == (`$INSTANCE_NAME`_FSM_UART_ENABLE_FLAG & `$INSTANCE_NAME`_fsmFlags))
    {
        /* Reset UART state machine */
        `$INSTANCE_NAME`_EndFrame(`$INSTANCE_NAME`_HANDLING_RESET_FSM_ERR);
    }

    /* Check for UART framing error */
    if(0u != (`$INSTANCE_NAME`_UART_ReadRxStatus() & `$INSTANCE_NAME`_UART_RX_STS_STOP_ERROR))
    {
        #if(1u == `$INSTANCE_NAME`_SAE_J2602)

            /* Set framing error bits */
            `$INSTANCE_NAME`_j2602Status |= `$INSTANCE_NAME`_J2602_STS_FRAMING_ERR;

        #endif /* (1u == `$INSTANCE_NAME`_SAE_J2602) */

        /* Set framing error  */
        `$INSTANCE_NAME`_fsmFlags |= `$INSTANCE_NAME`_FSM_FRAMING_ERROR_FLAG;

        `$ResponseErrorSignalSetTrue`

        /* Finish frame processing */
        `$INSTANCE_NAME`_EndFrame(`$INSTANCE_NAME`_HANDLING_DONT_SAVE_PID);
    }

    switch(`$INSTANCE_NAME`_uartFsmState)
    {

        /***********************************************************************
        *                       Sync Field Byte Receive
        * State description:
        *  - Available if Automatic Baud Rate Synchronization is disabled
        *  - Receives sync byte and verifies its correctness
        *  - Next state is PID Field Byte Receive (state 1)
        ***********************************************************************/
        #if(0u == `$INSTANCE_NAME`_AUTO_BAUD_RATE_SYNC)

            case `$INSTANCE_NAME`_UART_ISR_STATE_0_SNC:

                /* Handle Sync field correctness */
                if(`$INSTANCE_NAME`_FRAME_SYNC_BYTE != `$INSTANCE_NAME`_UART_ReadRxData())
                {
                    /* Set response error */
                    `$INSTANCE_NAME`_ifcStatus |= `$INSTANCE_NAME`_IFC_STS_ERROR_IN_RESPONSE;

                    #if(1u == `$INSTANCE_NAME`_SAE_J2602)

                        /* Set data error bit */
                        `$INSTANCE_NAME`_j2602Status |= `$INSTANCE_NAME`_J2602_STS_DATA_ERR;

                    #endif /* (1u == `$INSTANCE_NAME`_SAE_J2602) */

                    `$ResponseErrorSignalSetTrue`

                    /* Reset UART State Machine */
                    `$INSTANCE_NAME`_EndFrame(`$INSTANCE_NAME`_HANDLING_RESET_FSM_ERR);
                }
                else
                {
                    /* Next step is reception of the frame PID field */
                    `$INSTANCE_NAME`_uartFsmState = `$INSTANCE_NAME`_UART_ISR_STATE_1_PID;
                }
            break;

        #endif  /* (0u == `$INSTANCE_NAME`_AUTO_BAUD_RATE_SYNC) */


        /***********************************************************************
        *                       PID Field Byte Receive
        * State description:
        *  - Receives protected identifier (PID)
        *  - Checks PID parity
        *  - Set flags
        *  - Determine next state (RX or TX)
        ***********************************************************************/
        case `$INSTANCE_NAME`_UART_ISR_STATE_1_PID:

            /* Save PID */
            `$INSTANCE_NAME`_framePid = `$INSTANCE_NAME`_UART_ReadRxData();

            /* Reset number of transferred bytes */
            `$INSTANCE_NAME`_bytesTransferred = 0u;

            /* Verify PID parity */
            if(`$INSTANCE_NAME`_parityTable[`$INSTANCE_NAME`_framePid & `$INSTANCE_NAME`_PID_PARITY_MASK] != \
                `$INSTANCE_NAME`_framePid)
            {
                /* Set response error */
                `$INSTANCE_NAME`_ifcStatus |= `$INSTANCE_NAME`_IFC_STS_ERROR_IN_RESPONSE;

                #if(1u == `$INSTANCE_NAME`_SAE_J2602)

                    /* Set ERR2, ERR1 and ERR0 bits */
                    `$INSTANCE_NAME`_j2602Status |= `$INSTANCE_NAME`_J2602_STS_PARITY_ERR;

                #endif /* (1u == `$INSTANCE_NAME`_SAE_J2602) */

                `$ResponseErrorSignalSetTrue`

                /* Reset UART State Machine */
                `$INSTANCE_NAME`_EndFrame(`$INSTANCE_NAME`_HANDLING_DONT_SAVE_PID);
            }
            else    /* PID parity is correct */
            {
                /* Check if MRF or SRF frame */
                if((`$INSTANCE_NAME`_FRAME_PID_MRF == `$INSTANCE_NAME`_framePid) ||
                (`$INSTANCE_NAME`_FRAME_PID_SRF == `$INSTANCE_NAME`_framePid))
                {
                    /*  Transport Layer section. MRF and SRF detection */
                    #if(1u == `$INSTANCE_NAME`_TL_ENABLED)

                        #if(1u == `$INSTANCE_NAME`_SAE_J2602)

                            if(`$INSTANCE_NAME`_FRAME_PID_MRF_J2602 == `$INSTANCE_NAME`_framePid)
                            {
                                /* Process Master Request */

                                /* Nothing need to be done for Transport Layer */

                                /* Set response error */
                                `$INSTANCE_NAME`_ifcStatus |= `$INSTANCE_NAME`_IFC_STS_ERROR_IN_RESPONSE;

                                /* Set ERR2, ERR1 and ERR0 bits */
                                `$INSTANCE_NAME`_j2602Status |= `$INSTANCE_NAME`_J2602_STS_PARITY_ERR;

                                `$ResponseErrorSignalSetTrue`

                                /* Check for framing error */
                                if(0u == (`$INSTANCE_NAME`_fsmFlags & `$INSTANCE_NAME`_FSM_FRAMING_ERROR_FLAG))
                                {
                                    /* Save the last processed PID on the bus to the status variable */
                                    `$INSTANCE_NAME`_ifcStatus = ((`$INSTANCE_NAME`_ifcStatus & ~`$INSTANCE_NAME`_IFC_STS_PID_MASK) |
                                                                    (((l_u16)`$INSTANCE_NAME`_framePid) << 8u));
                                }

                                /* Reset UART State Machine */
                                `$INSTANCE_NAME`_EndFrame(`$INSTANCE_NAME`_HANDLING_DONT_SAVE_PID);
                            }

                        #endif  /* (1u == `$INSTANCE_NAME`_SAE_J2602) */

                        if(`$INSTANCE_NAME`_FRAME_PID_MRF == `$INSTANCE_NAME`_framePid)
                        {
                            /* Indicate that slave is required to receive the data */
                            `$INSTANCE_NAME`_tlFlags |= `$INSTANCE_NAME`_TL_RX_DIRECTION;

                            /*******************************************************
                            *               Cooked & RAW API
                            *******************************************************/

                            /* If the MRF PID is detected then pass a poiter to a start of a
                            * Frame Buffer and size of data to RX state to handle data receiving.
                            */

                            /* Frame equals 8 bytes */
                            `$INSTANCE_NAME`_frameSize = `$INSTANCE_NAME`_FRAME_DATA_SIZE_8;

                            /* Set frame data pointer to a start of a frame buffer */
                            `$INSTANCE_NAME`_frameData = `$INSTANCE_NAME`_mrfBuffer;

                            /* Switch to the subscribe data state. */
                            `$INSTANCE_NAME`_uartFsmState = `$INSTANCE_NAME`_UART_ISR_STATE_3_RX;
                        }

                        if(`$INSTANCE_NAME`_FRAME_PID_SRF == `$INSTANCE_NAME`_framePid)
                        {
                            /* Indicate that slave is required to transmit the data */
                            `$INSTANCE_NAME`_tlFlags |= `$INSTANCE_NAME`_TL_TX_DIRECTION;
                            
                            if(0u != (`$INSTANCE_NAME`_status & `$INSTANCE_NAME`_STATUS_SRVC_RSP_RDY))
                            {
                                /* Clear Service Response ready status bit */
                                `$INSTANCE_NAME`_status &= ~`$INSTANCE_NAME`_STATUS_SRVC_RSP_RDY;

                                /* Frame always equal to 8 bytes for TL */
                                `$INSTANCE_NAME`_frameSize = `$INSTANCE_NAME`_FRAME_DATA_SIZE_8;

                                /* Set frame data pointer to a start of a frame buffer */
                                `$INSTANCE_NAME`_frameData = `$INSTANCE_NAME`_srfBuffer;

                                /* Send first byte to the LIN master */
                                `$INSTANCE_NAME`_tmpData = *`$INSTANCE_NAME`_frameData;
                                `$INSTANCE_NAME`_frameData++;
                                `$INSTANCE_NAME`_UART_WriteTxData(`$INSTANCE_NAME`_tmpData);
                                `$INSTANCE_NAME`_bytesTransferred++;

                                /* Switch to the publish data state. */
                                `$INSTANCE_NAME`_uartFsmState = `$INSTANCE_NAME`_UART_ISR_STATE_2_TX;
                            }
                            else
                            {
                                #if(1u == `$INSTANCE_NAME`_TL_API_FORMAT)

                                    /***************************************************
                                    *                    Cooked API
                                    ***************************************************/

                                    /* Send one frame of a message if there is a message pending */
                                    if(`$INSTANCE_NAME`_txTlStatus == LD_IN_PROGRESS)
                                    {
                                        /* This part of code will handle PDU packing for Cooked API */
                                        /* Check length it shows if the message already sent */
                                        if(`$INSTANCE_NAME`_txMessageLength == 0u)
                                        {
                                            /* Reset UART State Machine */
                                            `$INSTANCE_NAME`_EndFrame(`$INSTANCE_NAME`_HANDLING_RESET_FSM_ERR);
                                        }
                                        /* Process the message sending */
                                        else
                                        {
                                            /* Fill Frame NAD field */
                                            `$INSTANCE_NAME`_srfBuffer[0u] = `$INSTANCE_NAME`_nad;

                                            /* Analyze length to find the type of frame the message should be sent */
                                            if(`$INSTANCE_NAME`_txMessageLength > `$INSTANCE_NAME`_FRAME_DATA_SIZE_6)
                                            {
                                                /* Process the FF Frame */
                                                if(`$INSTANCE_NAME`_prevPci == `$INSTANCE_NAME`_PDU_PCI_TYPE_UNKNOWN)
                                                {
                                                    /* Fill Frame PCI field */
                                                    `$INSTANCE_NAME`_srfBuffer[1u] = (`$INSTANCE_NAME`_PDU_PCI_TYPE_FF |
                                                        (HI8(`$INSTANCE_NAME`_txMessageLength)));

                                                    /* Fill Frame LEN field */
                                                    `$INSTANCE_NAME`_srfBuffer[2u] =
                                                        LO8(`$INSTANCE_NAME`_txMessageLength);

                                                    /* Fill Frame Data fields */
                                                    for(i = 3u; i < `$INSTANCE_NAME`_FRAME_DATA_SIZE_8; i++)
                                                    {
                                                        `$INSTANCE_NAME`_srfBuffer[i] =
                                                            `$INSTANCE_NAME`_txTlDataPointer[i - 3u];
                                                    }

                                                    /* Update the user buffer pointer */
                                                    `$INSTANCE_NAME`_txTlDataPointer =
                                                        `$INSTANCE_NAME`_txTlDataPointer +
                                                            `$INSTANCE_NAME`_FRAME_DATA_SIZE_5;

                                                    /* Save the previous PCI */
                                                    `$INSTANCE_NAME`_prevPci = `$INSTANCE_NAME`_PDU_PCI_TYPE_FF;

                                                    `$INSTANCE_NAME`_txMessageLength -=
                                                        `$INSTANCE_NAME`_FRAME_DATA_SIZE_5;
                                                }
                                                /* Process the CF Frame */
                                                else
                                                {
                                                     /* Fill Frame PCI field */
                                                    `$INSTANCE_NAME`_srfBuffer[1u] =
                                                        (`$INSTANCE_NAME`_PDU_PCI_TYPE_CF |
                                                            `$INSTANCE_NAME`_txFrameCounter);

                                                    /* Fill Frame Data fields */
                                                    for(i = 2u; i < `$INSTANCE_NAME`_FRAME_DATA_SIZE_8; i++)
                                                    {
                                                        `$INSTANCE_NAME`_srfBuffer[i] =
                                                            `$INSTANCE_NAME`_txTlDataPointer[i - 2u];
                                                    }

                                                    /* Update the user buffer pointer */
                                                    `$INSTANCE_NAME`_txTlDataPointer =
                                                        `$INSTANCE_NAME`_txTlDataPointer +
                                                            `$INSTANCE_NAME`_FRAME_DATA_SIZE_6;

                                                    /* Save the previous PCI */
                                                    `$INSTANCE_NAME`_prevPci = `$INSTANCE_NAME`_PDU_PCI_TYPE_CF;

                                                    /* Update length pointer properly */
                                                    `$INSTANCE_NAME`_txMessageLength -=
                                                        `$INSTANCE_NAME`_FRAME_DATA_SIZE_6;
                                                }
                                            }
                                            /* Process the SF Frame or last CF Frame */
                                            else
                                            {
                                                /* Check if the Previous frame is unknown which indicates that current
                                                * frame is SF, otherwise it is last CF frame. Fill Frame PCI field
                                                * properly.
                                                */
                                                if(`$INSTANCE_NAME`_PDU_PCI_TYPE_UNKNOWN == `$INSTANCE_NAME`_prevPci)
                                                {
                                                    `$INSTANCE_NAME`_srfBuffer[1u] = `$INSTANCE_NAME`_txMessageLength;

                                                    /* Save the previous PCI */
                                                    `$INSTANCE_NAME`_prevPci = `$INSTANCE_NAME`_PDU_PCI_TYPE_SF;
                                                }
                                                else
                                                {
                                                    `$INSTANCE_NAME`_srfBuffer[1u] = \
                                                        (`$INSTANCE_NAME`_PDU_PCI_TYPE_CF |
                                                            `$INSTANCE_NAME`_txFrameCounter);

                                                    /* Save the previous PCI */
                                                    `$INSTANCE_NAME`_prevPci = `$INSTANCE_NAME`_PDU_PCI_TYPE_CF;
                                                }

                                                /* Fill Frame Data fields */
                                                for(i = 2u; i < `$INSTANCE_NAME`_FRAME_DATA_SIZE_8; i++)
                                                {
                                                    if(`$INSTANCE_NAME`_txMessageLength >= (i - 1u))
                                                    {
                                                        `$INSTANCE_NAME`_srfBuffer[i] =
                                                            `$INSTANCE_NAME`_txTlDataPointer[i - 2u];
                                                    }
                                                    else
                                                    {
                                                        /* Fill unused data bytes with FFs */
                                                        `$INSTANCE_NAME`_srfBuffer[i] = 0xFFu;
                                                    }
                                                }

                                                /* Update the user buffer pointer */
                                                `$INSTANCE_NAME`_txTlDataPointer = \
                                                    `$INSTANCE_NAME`_txTlDataPointer + `$INSTANCE_NAME`_FRAME_DATA_SIZE_6;

                                                /* Update length pointer properly */
                                                if(`$INSTANCE_NAME`_txMessageLength >= `$INSTANCE_NAME`_FRAME_DATA_SIZE_6)
                                                {
                                                    `$INSTANCE_NAME`_txMessageLength -=
                                                        `$INSTANCE_NAME`_FRAME_DATA_SIZE_6;
                                                }
                                                else
                                                {
                                                    `$INSTANCE_NAME`_txMessageLength = 0u;
                                                }
                                            }

                                            /* Update the frame counter */
                                            if(`$INSTANCE_NAME`_txFrameCounter != 15u)
                                            {
                                                `$INSTANCE_NAME`_txFrameCounter++;
                                            }
                                            else
                                            {
                                                /* If frame counter is larger then 15 then reset it */
                                                `$INSTANCE_NAME`_txFrameCounter = 0u;
                                            }
                                        }

                                        /* Frame equals 8 bytes */
                                        `$INSTANCE_NAME`_frameSize = `$INSTANCE_NAME`_FRAME_DATA_SIZE_8;

                                        /* Set frame data pointer to a start of a frame buffer */
                                        `$INSTANCE_NAME`_frameData = `$INSTANCE_NAME`_srfBuffer;

                                        /* Send first byte to the LIN master */
                                        `$INSTANCE_NAME`_tmpData = *`$INSTANCE_NAME`_frameData;
                                        `$INSTANCE_NAME`_frameData++;
                                        `$INSTANCE_NAME`_UART_WriteTxData(`$INSTANCE_NAME`_tmpData);
                                        `$INSTANCE_NAME`_bytesTransferred++;

                                        /* Switch to the publish data state. */
                                        `$INSTANCE_NAME`_uartFsmState = `$INSTANCE_NAME`_UART_ISR_STATE_2_TX;
                                    }
                                    else
                                    {
                                        /* Reset UART State Machine */
                                        `$INSTANCE_NAME`_EndFrame(`$INSTANCE_NAME`_HANDLING_RESET_FSM_ERR);
                                    }

                                #else

                                    /***************************************************
                                    *                     Raw API
                                    ***************************************************/

                                    if(0u != `$INSTANCE_NAME`_txBufDepth)
                                    {
                                        /* Fill the frame buffer from SRF*/
                                        for(i = 0u; i < `$INSTANCE_NAME`_FRAME_DATA_SIZE_8; i++)
                                        {
                                            `$INSTANCE_NAME`_srfBuffer[i] = \
                                                `$INSTANCE_NAME`_rawTxQueue[`$INSTANCE_NAME`_txRdIndex++];
                                        }

                                        /* Read index should point to the the next byte in MRF */
                                        if(`$INSTANCE_NAME`_TL_TX_QUEUE_LEN == `$INSTANCE_NAME`_txRdIndex)
                                        {
                                            `$INSTANCE_NAME`_txRdIndex = 0u;
                                        }

                                        /* 8 bytes were read from the SRF so decrement the buffer depth */
                                        `$INSTANCE_NAME`_txBufDepth--;

                                        /* Update status properly */
                                        if(0u == `$INSTANCE_NAME`_txBufDepth)
                                        {
                                            `$INSTANCE_NAME`_txTlStatus = LD_QUEUE_EMPTY;
                                        }
                                        else
                                        {
                                            `$INSTANCE_NAME`_txTlStatus = LD_QUEUE_AVAILABLE;
                                        }

                                        /* Frame equals 8 bytes */
                                        `$INSTANCE_NAME`_frameSize = `$INSTANCE_NAME`_FRAME_DATA_SIZE_8;

                                        /* Set frame data pointer to a start of a frame buffer */
                                        `$INSTANCE_NAME`_frameData = `$INSTANCE_NAME`_srfBuffer;

                                        /* Send first byte to the LIN master */
                                        `$INSTANCE_NAME`_tmpData = *`$INSTANCE_NAME`_frameData;
                                        `$INSTANCE_NAME`_frameData++;
                                        `$INSTANCE_NAME`_UART_WriteTxData(`$INSTANCE_NAME`_tmpData);
                                        `$INSTANCE_NAME`_bytesTransferred++;

                                        /* Switch to the publish data state. */
                                        `$INSTANCE_NAME`_uartFsmState = `$INSTANCE_NAME`_UART_ISR_STATE_2_TX;
                                    }
                                    else
                                    {
                                        /* Reset UART State Machine */
                                        `$INSTANCE_NAME`_EndFrame(`$INSTANCE_NAME`_HANDLING_RESET_FSM_ERR);
                                    }

                                #endif /* (1u == `$INSTANCE_NAME`_TL_API_FORMAT) */
                            }


                        }

                    #else

                        /* These are invalid PIDs when TL is disabled - reset UART state machine */

                        /* Set response error */
                        `$INSTANCE_NAME`_ifcStatus |= `$INSTANCE_NAME`_IFC_STS_ERROR_IN_RESPONSE;

                        #if(1u == `$INSTANCE_NAME`_SAE_J2602)

                            /* Set ERR2, ERR1 and ERR0 bits */
                            `$INSTANCE_NAME`_j2602Status |= `$INSTANCE_NAME`_J2602_STS_PARITY_ERR;

                        #endif /* (1u == `$INSTANCE_NAME`_SAE_J2602) */

                        `$ResponseErrorSignalSetTrue`

                        /* Reset UART State Machine */
                        `$INSTANCE_NAME`_EndFrame(`$INSTANCE_NAME`_HANDLING_RESET_FSM_ERR);

                    #endif /* (1u == `$INSTANCE_NAME`_TL_ENABLED) */
                }
                else    /* Not MRF and SRF */
                {
                    /* Get PID index in `$INSTANCE_NAME`_pidInfoTable */
                    `$INSTANCE_NAME`_pidIndex = `$INSTANCE_NAME`_FindPidIndex(`$INSTANCE_NAME`_framePid);

                    if(`$INSTANCE_NAME`_INVALID_FRAME_PID != `$INSTANCE_NAME`_pidIndex)
                    {
                        /* Valid ID */

                        /* Start enhanced checksum calculation  */
                        `$INSTANCE_NAME`_interimChecksum = `$INSTANCE_NAME`_framePid;

                        /* Get size of frame */
                        `$INSTANCE_NAME`_frameSize = `$INSTANCE_NAME`_pidInfoTable[`$INSTANCE_NAME`_pidIndex].param & \
                                                        `$INSTANCE_NAME`_FRAME_DATA_SIZE_MASK;

                        /* TX response (publish action) was requested by Master */
                        if(0u != (`$INSTANCE_NAME`_FRAME_DIR_PUBLISH & \
                                 `$INSTANCE_NAME`_pidInfoTable[`$INSTANCE_NAME`_pidIndex].param))
                        {
                            /* SAE J2602 is disabled */
                            #if(0u == `$INSTANCE_NAME`_SAE_J2602)

                                /* This frame is event-triggered */
                                if(0u  != (`$INSTANCE_NAME`_FRAME_TYPE_EVENT & \
                                           `$INSTANCE_NAME`_pidInfoTable[`$INSTANCE_NAME`_pidIndex].param))
                                {
                                    /* Check do we need to process event-triggered frame */
                                    if(0u == `$INSTANCE_NAME`_GetEtFlagValue(`$INSTANCE_NAME`_pidIndex))
                                    {
                                        /* Reset UART State Machine */
                                        `$INSTANCE_NAME`_EndFrame(`$INSTANCE_NAME`_HANDLING_RESET_FSM_ERR);
                                    }
                                }

                            #endif /* (0u == `$INSTANCE_NAME`_SAE_J2602) */

                            /* Get pointer to the frame data */
                            `$INSTANCE_NAME`_frameData =
                                `$INSTANCE_NAME`_pidInfoTable[`$INSTANCE_NAME`_pidIndex].dataPtr;

                            /* Send first byte to the LIN master */
                            `$INSTANCE_NAME`_tmpData = *`$INSTANCE_NAME`_frameData;
                            `$INSTANCE_NAME`_frameData++;
                            `$INSTANCE_NAME`_UART_WriteTxData(`$INSTANCE_NAME`_tmpData);
                            `$INSTANCE_NAME`_bytesTransferred++;

                            /* Switch to the publish data state. */
                            `$INSTANCE_NAME`_uartFsmState = `$INSTANCE_NAME`_UART_ISR_STATE_2_TX;

                        }
                        else    /* RX response (subscribe action) was requested by Master */
                        {
                            /* Get pointer to the temp RX frame data buffer */
                            `$INSTANCE_NAME`_frameData = `$INSTANCE_NAME`_tmpRxFrameData;

                            /* Switch to the subscribe data state. */
                            `$INSTANCE_NAME`_uartFsmState = `$INSTANCE_NAME`_UART_ISR_STATE_3_RX;
                        }
                    }
                    else    /* Invalid ID */
                    {
                        /* Set response error */
                        `$INSTANCE_NAME`_ifcStatus |= `$INSTANCE_NAME`_IFC_STS_ERROR_IN_RESPONSE;

                        #if(1u == `$INSTANCE_NAME`_SAE_J2602)

                            /* Set data error bit */
                            `$INSTANCE_NAME`_j2602Status |= `$INSTANCE_NAME`_J2602_STS_DATA_ERR;

                        #endif /* (1u == `$INSTANCE_NAME`_SAE_J2602) */

                        `$ResponseErrorSignalSetTrue`

                        /* Reset UART State Machine */
                        `$INSTANCE_NAME`_EndFrame(`$INSTANCE_NAME`_HANDLING_RESET_FSM_ERR);
                    }
                }
            }

        break;


        /***********************************************************************
        *                       TX response (Publish)
        * State description:
        *  - Transmits data to LIN Master
        *  - Transmits next data byte if there were no any errors
        *  - Transmits checksum when data was send correctly
        ***********************************************************************/
        case `$INSTANCE_NAME`_UART_ISR_STATE_2_TX:

            /* Set the response active flag */
            `$INSTANCE_NAME`_status |= `$INSTANCE_NAME`_STATUS_RESPONSE_ACTIVE;

            /* Previously transmitted and read back bytes are not equal */
            if(`$INSTANCE_NAME`_tmpData != `$INSTANCE_NAME`_UART_ReadRxData())
            {
                /* Mismatch Error */

                #if(1u == `$INSTANCE_NAME`_SAE_J2602)

                    /* Set ERR2 bit */
                    `$INSTANCE_NAME`_j2602Status |= `$INSTANCE_NAME`_J2602_STS_DATA_ERR;

                    /* Readback error - set response error flag */
                    `$INSTANCE_NAME`_ifcStatus |= `$INSTANCE_NAME`_IFC_STS_ERROR_IN_RESPONSE;

                #else

                    /* Skip event-triggered frame */
                    if(0u  == (`$INSTANCE_NAME`_FRAME_TYPE_EVENT & \
                               `$INSTANCE_NAME`_pidInfoTable[`$INSTANCE_NAME`_pidIndex].param))
                    {
                        /* Readback error - set response error flag */
                        `$INSTANCE_NAME`_ifcStatus |= `$INSTANCE_NAME`_IFC_STS_ERROR_IN_RESPONSE;
                    }

                #endif  /* (1u == `$INSTANCE_NAME`_SAE_J2602) */

                `$ResponseErrorSignalSetTrue`

                /* Check for framing error */
                if(0u == (`$INSTANCE_NAME`_fsmFlags & `$INSTANCE_NAME`_FSM_FRAMING_ERROR_FLAG))
                {
                    /* Save the last processed PID on the bus to the status variable */
                    `$INSTANCE_NAME`_ifcStatus = ((`$INSTANCE_NAME`_ifcStatus & ~`$INSTANCE_NAME`_IFC_STS_PID_MASK) |
                                                    (((l_u16)`$INSTANCE_NAME`_framePid) << 8u));
                }

                /* End frame with response error */
                `$INSTANCE_NAME`_EndFrame(`$INSTANCE_NAME`_HANDLING_DONT_SAVE_PID);
            }
            else    /* If readback was successful than continue transmitting */
            {
                /* Add transmitted byte to the interim checksum */
                `$INSTANCE_NAME`_interimChecksum += `$INSTANCE_NAME`_tmpData;
                if(`$INSTANCE_NAME`_interimChecksum >= 256u)
                {
                    `$INSTANCE_NAME`_interimChecksum -= 255u;
                }

                /* Check to see if all data bytes were sent */
                if(`$INSTANCE_NAME`_frameSize > `$INSTANCE_NAME`_bytesTransferred)
                {
                    /* Send out the next byte of the buffer */
                    `$INSTANCE_NAME`_tmpData = *`$INSTANCE_NAME`_frameData;
                    `$INSTANCE_NAME`_frameData++;
                    `$INSTANCE_NAME`_UART_WriteTxData(`$INSTANCE_NAME`_tmpData);
                    `$INSTANCE_NAME`_bytesTransferred++;
                }
                else    /* All data bytes were sent - compute and transmit checksum */
                {
                    /* Compute and send out the checksum byte */
                    `$INSTANCE_NAME`_UART_WriteTxData((((l_u8) `$INSTANCE_NAME`_interimChecksum) ^ 0xFFu));

                    `$INSTANCE_NAME`_bytesTransferred = 0u;

                    /* Switch to the checksum state */
                    `$INSTANCE_NAME`_uartFsmState = `$INSTANCE_NAME`_UART_ISR_STATE_4_CHS;
                }
            }
        break;


        /***********************************************************************
        *                       RX response (Subscribe)
        * State description:
        *  - Receives data from LIN Master
        *  - Received data are saved to the temporary buffer
        ***********************************************************************/
        case `$INSTANCE_NAME`_UART_ISR_STATE_3_RX:

            /* Save received byte */
            `$INSTANCE_NAME`_tmpData = `$INSTANCE_NAME`_UART_ReadRxData();
            *`$INSTANCE_NAME`_frameData = `$INSTANCE_NAME`_tmpData;
            `$INSTANCE_NAME`_frameData++;
            `$INSTANCE_NAME`_bytesTransferred++;

            /* Set response active flag */
            `$INSTANCE_NAME`_status |= `$INSTANCE_NAME`_STATUS_RESPONSE_ACTIVE;

            /* One or more data bytes have been received */
            `$INSTANCE_NAME`_fsmFlags |= `$INSTANCE_NAME`_FSM_DATA_RECEIVE;

            /* Add received byte to the interim checksum */
            `$INSTANCE_NAME`_interimChecksum += `$INSTANCE_NAME`_tmpData;
            if(`$INSTANCE_NAME`_interimChecksum >= 256u)
            {
                `$INSTANCE_NAME`_interimChecksum -= 255u;
            }

            /* Check to see if the data section has not finished */
            if(`$INSTANCE_NAME`_frameSize > `$INSTANCE_NAME`_bytesTransferred)
            {
                /* There is data to be sent */
            }
            else
            {
                /* There is no data to be sent */

                `$INSTANCE_NAME`_bytesTransferred = 0u;

                /* Switch to the checksum state */
                `$INSTANCE_NAME`_uartFsmState = `$INSTANCE_NAME`_UART_ISR_STATE_4_CHS;
            }

        break;



        /***********************************************************************
        *                              Checksum
        ***********************************************************************/
        case `$INSTANCE_NAME`_UART_ISR_STATE_4_CHS:

            /* Previously transmitted and read back bytes are not equal */
            if((((l_u8) `$INSTANCE_NAME`_interimChecksum) ^ 0xFFu) != `$INSTANCE_NAME`_UART_ReadRxData())
            {
                /* Mismatch or Ckechsum Error */

                /* Set response error */
                `$INSTANCE_NAME`_ifcStatus |= `$INSTANCE_NAME`_IFC_STS_ERROR_IN_RESPONSE;

                #if(1u == `$INSTANCE_NAME`_SAE_J2602)

                    /* Set ERR2 and ERR0 bits */
                    `$INSTANCE_NAME`_j2602Status |= `$INSTANCE_NAME`_J2602_STS_CHECKSUM_ERR;

                #endif /* (1u == `$INSTANCE_NAME`_SAE_J2602) */

                `$ResponseErrorSignalSetTrue`

                /* Check for framing error */
                if(0u == (`$INSTANCE_NAME`_fsmFlags & `$INSTANCE_NAME`_FSM_FRAMING_ERROR_FLAG))
                {
                    /* Save the last processed PID on the bus to the status variable */
                    `$INSTANCE_NAME`_ifcStatus = ((`$INSTANCE_NAME`_ifcStatus & ~`$INSTANCE_NAME`_IFC_STS_PID_MASK) |
                                                    (((l_u16)`$INSTANCE_NAME`_framePid) << 8u));
                }

                /* Reset UART state machine with checksum or mismatch error */
                `$INSTANCE_NAME`_EndFrame(`$INSTANCE_NAME`_HANDLING_DONT_SAVE_PID);
            }
            else
            {
                /*  Clear all error bits in interface status */
                #if(1u == `$INSTANCE_NAME`_SAE_J2602)

                    `$INSTANCE_NAME`_j2602Status &= `$INSTANCE_NAME`_J2602_CLEAR_ERR_BITS_MASK;

                #endif  /* (1u == `$INSTANCE_NAME`_SAE_J2602) */


                /* Clear framing error and data receive flags */
                `$INSTANCE_NAME`_fsmFlags &= \
                    ~(`$INSTANCE_NAME`_FSM_FRAMING_ERROR_FLAG | `$INSTANCE_NAME`_FSM_DATA_RECEIVE);

                /* Set successful transfer interface flag */
                `$INSTANCE_NAME`_ifcStatus |= `$INSTANCE_NAME`_IFC_STS_SUCCESSFUL_TRANSFER;

                /* Save the last processed PID on the bus to the status variable */
                `$INSTANCE_NAME`_ifcStatus = ((`$INSTANCE_NAME`_ifcStatus & ~`$INSTANCE_NAME`_IFC_STS_PID_MASK) |
                                                (((l_u16)`$INSTANCE_NAME`_framePid) << 8u));

                /* Set overrun interface flag */
                if(0u != (`$INSTANCE_NAME`_FSM_OVERRUN & `$INSTANCE_NAME`_fsmFlags))
                {
                    `$INSTANCE_NAME`_ifcStatus |= `$INSTANCE_NAME`_IFC_STS_OVERRUN;
                }

                /* Set Overrun flag */
                `$INSTANCE_NAME`_fsmFlags |= `$INSTANCE_NAME`_FSM_OVERRUN;

                /* Clear response error signal if frame contains RESPONSE ERROR signal */
                #if(1u == `$INSTANCE_NAME`_RESPONSE_ERROR_SIGNAL)

                    if(`$INSTANCE_NAME`_RESPONSE_ERROR_FRAME_INDEX == `$INSTANCE_NAME`_pidIndex)
                    {
                        `$ResponseErrorSignalSetFalse`
                    }

                #endif /* (1u == `$INSTANCE_NAME`_RESPONSE_ERROR_SIGNAL) */


                #if(1u == `$INSTANCE_NAME`_TL_ENABLED)

                    if(!((`$INSTANCE_NAME`_FRAME_PID_MRF == `$INSTANCE_NAME`_framePid) || \
                         (`$INSTANCE_NAME`_FRAME_PID_SRF == `$INSTANCE_NAME`_framePid)))
                    {
                        /* SAE J2602 is disabled */
                        #if(0u == `$INSTANCE_NAME`_SAE_J2602)

                            /* This frame is event-triggered */
                            if(0u  != (`$INSTANCE_NAME`_FRAME_TYPE_EVENT & \
                                       `$INSTANCE_NAME`_pidInfoTable[`$INSTANCE_NAME`_pidIndex].param))
                            {
                                /* Clear event-triggered flags */
                                `$INSTANCE_NAME`_ClearEtFlagValue(`$INSTANCE_NAME`_pidIndex);

                                /* Reset UART State Machine */
                               `$INSTANCE_NAME`_EndFrame(`$INSTANCE_NAME`_HANDLING_RESET_FSM_ERR);
                            }

                        #endif /* (0u == `$INSTANCE_NAME`_SAE_J2602) */

                        /* Set assosiated with current frame flags */
                        `$INSTANCE_NAME`_SetAssociatedFlags(`$INSTANCE_NAME`_pidIndex);
                    }

                #else

                    /* SAE J2602 is disabled */
                    #if(0u == `$INSTANCE_NAME`_SAE_J2602)

                        /* This frame is event-triggered */
                        if(0u  != (`$INSTANCE_NAME`_FRAME_TYPE_EVENT & \
                                   `$INSTANCE_NAME`_pidInfoTable[`$INSTANCE_NAME`_pidIndex].param))
                        {
                            /* Clear event-triggered flags */
                            `$INSTANCE_NAME`_ClearEtFlagValue(`$INSTANCE_NAME`_pidIndex);

                            /* Reset UART State Machine */
                           `$INSTANCE_NAME`_EndFrame(`$INSTANCE_NAME`_HANDLING_RESET_FSM_ERR);
                        }

                    #endif /* (0u == `$INSTANCE_NAME`_SAE_J2602) */

                    /* Set assosiated with current frame flags */
                    `$INSTANCE_NAME`_SetAssociatedFlags(`$INSTANCE_NAME`_pidIndex);

                #endif  /* (1u == `$INSTANCE_NAME`_TL_ENABLED) */

                #if(1u == `$INSTANCE_NAME`_TL_ENABLED)

                    /* Check to see if received data was a "master request frame" */
                    if(`$INSTANCE_NAME`_FRAME_PID_MRF == `$INSTANCE_NAME`_framePid)
                    {
                        /* Process master request frame data here */
                        `$INSTANCE_NAME`_ProcessMrf(`$INSTANCE_NAME`_mrfBuffer);

                        #if(1u == `$INSTANCE_NAME`_TL_API_FORMAT)

                            if((`$INSTANCE_NAME`_PDU_PCI_TYPE_FF == `$INSTANCE_NAME`_prevPci) || \
                                (`$INSTANCE_NAME`_PDU_PCI_TYPE_CF == `$INSTANCE_NAME`_prevPci))
                            {
                                `$INSTANCE_NAME`_tlFlags |= `$INSTANCE_NAME`_TL_N_CR_TIMEOUT_ON;
                                `$INSTANCE_NAME`_tlTimeoutCnt = 0u;
                            }

                            if((0u == `$INSTANCE_NAME`_rxMessageLength) && \
                                ( 0u != (`$INSTANCE_NAME`_tlFlags & `$INSTANCE_NAME`_TL_RX_REQUESTED)))
                            {
                                /* Indicate that message is received */
                                `$INSTANCE_NAME`_rxTlStatus = LD_COMPLETED;

                                /* Reset the frame counter */
                                `$INSTANCE_NAME`_rxFrameCounter = 0u;

                                /* Previous PCI is required to be unknown at the beginning of a new message */
                                `$INSTANCE_NAME`_prevPci = `$INSTANCE_NAME`_PDU_PCI_TYPE_UNKNOWN;

                                /* Clear TX requested flag as the message was transfered */
                                `$INSTANCE_NAME`_tlFlags &= ~`$INSTANCE_NAME`_TL_RX_REQUESTED;

                                `$INSTANCE_NAME`_tlFlags &= ~`$INSTANCE_NAME`_TL_N_CR_TIMEOUT_ON;
                            }

                        #endif /* (1u == `$INSTANCE_NAME`_TL_API_FORMAT) */
                        
                        /* Clear the TL RX direction flag */
                        `$INSTANCE_NAME`_tlFlags &= ~`$INSTANCE_NAME`_TL_RX_DIRECTION;

                        /* Reset UART state machine */
                        `$INSTANCE_NAME`_EndFrame(`$INSTANCE_NAME`_HANDLING_RESET_FSM_ERR);
                    }
                    else if(`$INSTANCE_NAME`_FRAME_PID_SRF == `$INSTANCE_NAME`_framePid)
                    {
                        #if(1u == `$INSTANCE_NAME`_TL_API_FORMAT)

                            if((`$INSTANCE_NAME`_PDU_PCI_TYPE_FF == `$INSTANCE_NAME`_prevPci) || \
                                (`$INSTANCE_NAME`_PDU_PCI_TYPE_CF == `$INSTANCE_NAME`_prevPci))
                            {
                                `$INSTANCE_NAME`_tlFlags |= `$INSTANCE_NAME`_TL_N_AS_TIMEOUT_ON;
                                `$INSTANCE_NAME`_tlTimeoutCnt = 0u;
                            }

                            if((0u == `$INSTANCE_NAME`_txMessageLength) && \
                                ( 0u != (`$INSTANCE_NAME`_tlFlags & `$INSTANCE_NAME`_TL_TX_REQUESTED)))
                            {
                                /* Indicate that message is sent */
                                `$INSTANCE_NAME`_txTlStatus = LD_COMPLETED;

                                /* Reset the frame counter */
                                `$INSTANCE_NAME`_txFrameCounter = 0u;

                                /* Previous PCI is required to be unknown at the beginning of a new message */
                                `$INSTANCE_NAME`_prevPci = `$INSTANCE_NAME`_PDU_PCI_TYPE_UNKNOWN;

                                /* Clear TX requested flag as the message was transfered */
                                `$INSTANCE_NAME`_tlFlags &= ~`$INSTANCE_NAME`_TL_TX_REQUESTED;

                                `$INSTANCE_NAME`_tlFlags &= ~`$INSTANCE_NAME`_TL_N_AS_TIMEOUT_ON;
                            }

                        #endif /* (1u == `$INSTANCE_NAME`_TL_API_FORMAT) */

                        /* Clear the TL TX direction flag */
                        `$INSTANCE_NAME`_tlFlags &= ~`$INSTANCE_NAME`_TL_TX_DIRECTION;
                        
                        /* Reset UART state machine */
                        `$INSTANCE_NAME`_EndFrame(`$INSTANCE_NAME`_HANDLING_RESET_FSM_ERR);
                    }
                    else
                    {
                        /* RX response (subscribe action) was requested by Master */
                        if(0u == (`$INSTANCE_NAME`_FRAME_DIR_PUBLISH & \
                                 `$INSTANCE_NAME`_pidInfoTable[`$INSTANCE_NAME`_pidIndex].param))
                        {
                            interruptState = CyEnterCriticalSection();

                            /* Copy received data from temprorary buffer to the frame one */
                            for(i = 0u; i < `$INSTANCE_NAME`_frameSize; i++)
                            {
                                *(`$INSTANCE_NAME`_pidInfoTable[`$INSTANCE_NAME`_pidIndex].dataPtr + i) =
                                    `$INSTANCE_NAME`_tmpRxFrameData[i];
                            }

                            CyExitCriticalSection(interruptState);
                        }

                        /* Reset UART state machine */
                        `$INSTANCE_NAME`_EndFrame(`$INSTANCE_NAME`_HANDLING_RESET_FSM_ERR);
                    }

                #else

                    /* RX response (subscribe action) was requested by Master */
                    if(0u == (`$INSTANCE_NAME`_FRAME_DIR_PUBLISH & \
                             `$INSTANCE_NAME`_pidInfoTable[`$INSTANCE_NAME`_pidIndex].param))
                    {
                        interruptState = CyEnterCriticalSection();

                        /* Copy received data from temprorary buffer to the frame one */
                        for(i = 0u; i < `$INSTANCE_NAME`_frameSize; i++)
                        {
                            *(`$INSTANCE_NAME`_pidInfoTable[`$INSTANCE_NAME`_pidIndex].dataPtr + i) =
                                `$INSTANCE_NAME`_tmpRxFrameData[i];
                        }

                        CyExitCriticalSection(interruptState);
                    }

                    /* Reset UART state machine */
                    `$INSTANCE_NAME`_EndFrame(`$INSTANCE_NAME`_HANDLING_RESET_FSM_ERR);

                #endif  /* (1u == `$INSTANCE_NAME`_TL_ENABLED) */

            }

            `$INSTANCE_NAME`_interimChecksum = 0u;

        break;


        default:
            /* Reset UART state machine */
            `$INSTANCE_NAME`_EndFrame(`$INSTANCE_NAME`_HANDLING_RESET_FSM_ERR);
        break;
    }


    /***************************************************************************
    *  Place your UART ISR code here
    ***************************************************************************/
    /* `#START `$INSTANCE_NAME`_UART_ISR_CODE` */

    /* `#END` */
}


/*******************************************************************************
* Function Name: l_ifc_rx
********************************************************************************
*
* Summary:
*  The LIN Slave component takes care of calling this API routine automatically.
*  Therefore, this API routine must not be called by the application code.
*
* Parameters:
*  iii - is the name of the interface handle.
*
* Return:
*  None
*
* Global variables:
*  Refer to the appropriate section of the l_ifc_rx_`$INSTANCE_NAME`() comments
*
* Reentrant:
*  No
*
*******************************************************************************/
void l_ifc_rx(l_ifc_handle iii)
{
    switch(iii)
    {
        case `$INSTANCE_NAME`_IFC_HANDLE:
            l_ifc_rx_`$INSTANCE_NAME`();
        break;

        default:
        break;
    }

    /* To remove unreferenced local variable warning */
    iii = iii;
}


/*******************************************************************************
* Function Name: l_ifc_rx
********************************************************************************
*
* Summary:
*  The LIN Slave component takes care of calling this API routine automatically.
*  Therefore, this API routine must not be called by the application code.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global variables:
*  Refer to the appropriate section of the l_ifc_rx_`$INSTANCE_NAME`() comments
*
* Reentrant:
*  No
*
*******************************************************************************/
void l_ifc_tx_`$INSTANCE_NAME`(void)
{
    l_ifc_rx_`$INSTANCE_NAME`();
}


/*******************************************************************************
* Function Name: l_ifc_rx
********************************************************************************
*
* Summary:
*  The LIN Slave component takes care of calling this API routine automatically.
*  Therefore, this API routine must not be called by the application code.
*
* Parameters:
*  iii - is the name of the interface handle.
*
* Return:
*  None
*
* Global variables:
*  Refer to the appropriate section of the l_ifc_rx_`$INSTANCE_NAME`() comments
*
* Reentrant:
*  No
*
*******************************************************************************/
void l_ifc_tx(l_ifc_handle iii)
{
    switch(iii)
    {
        case `$INSTANCE_NAME`_IFC_HANDLE:
            l_ifc_tx_`$INSTANCE_NAME`();
        break;

        default:
        break;
    }

    /* To remove unreferenced local variable warning */
    iii = iii;
}


/*******************************************************************************
* Function Name: l_ifc_aux
********************************************************************************
*
* Summary:
*  The LIN Slave component takes care of calling this API routine automatically.
*  Therefore, this API routine must not be called by the application code.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global variables:
*  `$INSTANCE_NAME`_ifcStatus   - appropriate interface statuses bits are set.
*  `$INSTANCE_NAME`_fsmFlags - appropriate flags are set.
*  `$INSTANCE_NAME`_j2602Status - appropriate statuses are set (optionally).
*  `$INSTANCE_NAME`_uartFsmState - next state of data transfer FSM saved.
*  `$INSTANCE_NAME`_fsmFlags - appropriate flags of data transfer FSM are set.
*  `$INSTANCE_NAME`_auxStatus  - shadow variable of the status register updated.
*
* Reentrant:
*  No
*
*******************************************************************************/
void l_ifc_aux_`$INSTANCE_NAME`(void)
{
    l_u8 interruptState;

    /* Update shadow status register with the hardware status */
    `$INSTANCE_NAME`_auxStatus |= `$INSTANCE_NAME`_STATUS_REG;

    /***************************************************************************
    *                             Edge Detected                                *
    ***************************************************************************/
    if(0u != (`$INSTANCE_NAME`_auxStatus & `$INSTANCE_NAME`_STATUS_EDGE_DETECTED))
    {
        /* Set bus activity interface status bit */
        `$INSTANCE_NAME`_ifcStatus |= `$INSTANCE_NAME`_IFC_STS_BUS_ACTIVITY;

        #if(1u == `$INSTANCE_NAME`_INACTIVITY_ENABLED)

            /* Clear period timer counter */
            `$INSTANCE_NAME`_periodCounter = 0x00u;

        #endif /* (1u == `$INSTANCE_NAME`_INACTIVITY_ENABLED) */
    }


    /***************************************************************************
    *                       Bus Inactivity Interrupt Detected
    ***************************************************************************/
    #if(1u == `$INSTANCE_NAME`_INACTIVITY_ENABLED)

        if(0u != (`$INSTANCE_NAME`_auxStatus & `$INSTANCE_NAME`_STATUS_INACTIVITY_INT))
        {
            #if(1u == `$INSTANCE_NAME`_TL_ENABLED)              /* TL enabled */
                #if(1u == `$INSTANCE_NAME`_TL_API_FORMAT)       /* Cooked API */

                    /* if the timeout is enabled then proceed timeout manage */
                    if(0u !=(`$INSTANCE_NAME`_tlFlags & `$INSTANCE_NAME`_TL_N_AS_TIMEOUT_ON))
                    {
                        /* Increment timeout */
                        `$INSTANCE_NAME`_tlTimeoutCnt++;

                        if(`$INSTANCE_NAME`_TL_N_AS_TIMEOUT_VALUE <= `$INSTANCE_NAME`_tlTimeoutCnt)
                        {
                            /* Set error status as the timeout occurred */
                            `$INSTANCE_NAME`_txTlStatus = LD_N_AS_TIMEOUT;
                        }
                    }
                    else if(0u !=(`$INSTANCE_NAME`_tlFlags & `$INSTANCE_NAME`_TL_N_CR_TIMEOUT_ON))
                    {
                        /* Increment timeout */
                        `$INSTANCE_NAME`_tlTimeoutCnt++;

                        if(`$INSTANCE_NAME`_TL_N_CR_TIMEOUT_VALUE <= `$INSTANCE_NAME`_tlTimeoutCnt)
                        {
                            /* Set error status as the timeout occurred */
                            `$INSTANCE_NAME`_rxTlStatus = LD_N_CR_TIMEOUT;
                        }
                    }
                    else
                    {
                        /* Reset timeout counter */
                        `$INSTANCE_NAME`_tlTimeoutCnt = 0u;
                    }

                #endif /* (1u == `$INSTANCE_NAME`_TL_API_FORMAT) */
            #endif /* (1u == `$INSTANCE_NAME`_TL_ENABLED) */

            if(`$INSTANCE_NAME`_INACTIVITY_THRESHOLD_IN_100_MS == `$INSTANCE_NAME`_periodCounter)
            {
                /* Inactivity threshold achieved */

                /* Set bus inactivity ioctl status bit */
                `$INSTANCE_NAME`_ioctlStatus |= `$INSTANCE_NAME`_IOCTL_STS_BUS_INACTIVITY;
            }
            else
            {
                `$INSTANCE_NAME`_periodCounter++;
            }
        }

    #endif  /* (1u == `$INSTANCE_NAME`_INACTIVITY_ENABLED) */


    /***************************************************************************
    *                       Break Field Detected
    ***************************************************************************/
    if(0u != (`$INSTANCE_NAME`_auxStatus & `$INSTANCE_NAME`_STATUS_BREAK_DETECTED))
    {
        /* Framing error or data tranfer was aborted */
        if(0u  != ((`$INSTANCE_NAME`_FSM_FRAMING_ERROR_FLAG | `$INSTANCE_NAME`_FSM_DATA_RECEIVE) &
                    `$INSTANCE_NAME`_fsmFlags))
        {
            /* Set response error */
            `$INSTANCE_NAME`_ifcStatus |= `$INSTANCE_NAME`_IFC_STS_ERROR_IN_RESPONSE;

            /* Clear framing error  */
            `$INSTANCE_NAME`_fsmFlags &= ~`$INSTANCE_NAME`_FSM_FRAMING_ERROR_FLAG;

            #if(1u == `$INSTANCE_NAME`_SAE_J2602)

                /* Set ERR2 bit */
                `$INSTANCE_NAME`_j2602Status |= `$INSTANCE_NAME`_J2602_STS_DATA_ERR;

            #endif  /* (1u == `$INSTANCE_NAME`_SAE_J2602) */

            `$ResponseErrorSignalSetTrue`

        }   /* No response error, continue */

        /* Set break detected flag */
        `$INSTANCE_NAME`_fsmFlags |= `$INSTANCE_NAME`_FSM_BREAK_FLAG;

        /***********************************************************************
        *  This will make bus RXD signal to be automatically routed to the
        *  UART's input when rising edge of the break field occurs (in case
        *  when Auto Baud Rate Sync is disabled; that will make UART to be
        *  able to receive sync byte) or when sync field is already processed
        *  by hardware (in case when Auto Baud Rate Sync is enabled; that will
        *  make UART to be able to receive frame's PID).
        ***********************************************************************/
        `$INSTANCE_NAME`_CONTROL_REG &=  ~`$INSTANCE_NAME`_CONTROL_RX_DIS;

        /* Auto Baud Rate Sync Enabled */
        #if(1u == `$INSTANCE_NAME`_AUTO_BAUD_RATE_SYNC)

            /* Restore initial clock divider */
            `$INSTANCE_NAME`_IntClk_SetDividerRegister(`$INSTANCE_NAME`_initialClockDivider, 0u);

        #else   /* Auto Baud Rate Sync Disabled */

            /* Clear one or more data bytes have been recieved internal flag */
            `$INSTANCE_NAME`_fsmFlags &= ~`$INSTANCE_NAME`_FSM_DATA_RECEIVE;

            /* Set UART ISR FSM to sync byte receive state */
            `$INSTANCE_NAME`_uartFsmState = `$INSTANCE_NAME`_UART_ISR_STATE_0_SNC;

            /* Set UART enable flag */
            `$INSTANCE_NAME`_fsmFlags |= `$INSTANCE_NAME`_FSM_UART_ENABLE_FLAG;

            /* Clear any pending UART interrupt */
            CyIntClearPending(`$INSTANCE_NAME`_UART_ISR_NUMBER);

            /* Enable UART ISR interrupt */
            CyIntEnable(`$INSTANCE_NAME`_UART_ISR_NUMBER);

        #endif  /* (1u == `$INSTANCE_NAME`_AUTO_BAUD_RATE_SYNC) */
    }


    /* Auto Baud Rate Sync Enabled */
    #if(1u == `$INSTANCE_NAME`_AUTO_BAUD_RATE_SYNC)

        /***********************************************************************
        *                       Sync Field Complete                            *
        ***********************************************************************/
        if(0u != (`$INSTANCE_NAME`_auxStatus & `$INSTANCE_NAME`_STATUS_SYNC_COMPLETED))
        {
            /* Save actual sync field timer counts */
            `$INSTANCE_NAME`_syncCounts = `$INSTANCE_NAME`_LOW_BIT_LENGTH_SUM_REG;
            `$INSTANCE_NAME`_syncCounts = `$INSTANCE_NAME`_LOW_BIT_LENGTH_SUM_REG;
            `$INSTANCE_NAME`_syncCounts = `$INSTANCE_NAME`_LOW_BIT_LENGTH_SUM_REG;
            `$INSTANCE_NAME`_syncCounts = `$INSTANCE_NAME`_LOW_BIT_LENGTH_SUM_REG +
                                          `$INSTANCE_NAME`_HIGH_BITS_LENGTH_SUM_REG;

            /* Set new clock divider */
            if(`$INSTANCE_NAME`_syncCounts != `$INSTANCE_NAME`_EXPECTED_TIME_COUNTS)
            {
                `$INSTANCE_NAME`_IntClk_SetDividerRegister((uint16)(((uint32)`$INSTANCE_NAME`_initialClockDivider *
                    (uint32) `$INSTANCE_NAME`_syncCounts) / `$INSTANCE_NAME`_EXPECTED_TIME_COUNTS), 0u);
            }

            /* Clear one or more data bytes have been recieved internal flag */
            `$INSTANCE_NAME`_fsmFlags &= ~`$INSTANCE_NAME`_FSM_DATA_RECEIVE;

            /* This will reset the UART ISR FSM to state 1 */
            `$INSTANCE_NAME`_uartFsmState = `$INSTANCE_NAME`_UART_ISR_STATE_1_PID;

            /* Set UART enabled flag */
            `$INSTANCE_NAME`_fsmFlags |= `$INSTANCE_NAME`_FSM_UART_ENABLE_FLAG;

            /* Clear any pending UART interrupt */
            CyIntClearPending(`$INSTANCE_NAME`_UART_ISR_NUMBER);

            /* Enable UART ISR interrupt */
            CyIntEnable(`$INSTANCE_NAME`_UART_ISR_NUMBER);
        }

    #endif /* (1u == `$INSTANCE_NAME`_AUTO_BAUD_RATE_SYNC) */


    /***************************************************************************
    *  Place your BASE ISR code here
    ***************************************************************************/
    /* `#START `$INSTANCE_NAME`_BASE_ISR_CODE` */

    /* `#END` */


    /* Clear software shadow register file  */
    `$INSTANCE_NAME`_auxStatus = 0x00u;
}


/*******************************************************************************
* Function Name: l_ifc_aux
********************************************************************************
*
* Summary:
*  The LIN Slave component takes care of calling this API routine automatically.
*  Therefore, this API routine must not be called by the application code.
*
* Parameters:
*  iii - is the name of the interface handle.
*
* Return:
*  None
*
* Global variables:
*  Refer to the appropriate section of the l_ifc_aux_`$INSTANCE_NAME`() comments
*
* Reentrant:
*  No
*
*******************************************************************************/
void l_ifc_aux(l_ifc_handle iii)
{
    switch(iii)
    {
        case `$INSTANCE_NAME`_IFC_HANDLE:
            l_ifc_aux_`$INSTANCE_NAME`();
        break;

        default:
        break;
    }

    /* To remove unreferenced local variable warning */
    iii = iii;
}


/*******************************************************************************
* Function Name: l_ifc_read_status_`$INSTANCE_NAME`
********************************************************************************
*
* Summary:
*  This function is defined by the LIN specification. This returns the status of
*  the specified LIN interface and then clears all status bits for that
*  interface. See Section 7.2.5.8 of the LIN 2.1 specification.
*
* Parameters:
*  None
*
* Return:
*  The status bits of the specified LIN interface are returned. These bits have
*  the following meanings:
*    [15:8]    Last Received PID
*    [7]        0
*    [6]        Save Configuration flag
*    [5]        0
*    [4]        Bus Activity flag
*    [3]        Go To Sleep flag
*    [2]        Overrun flag
*    [1]        Successful Transfer flag
*    [0]        Error in Response flag
*
* Global variables:
*  `$INSTANCE_NAME`_auxStatus  - shadow variable of the status register updated.
*  `$INSTANCE_NAME`_ifcStatus - the bus activity bit is set.
*
* Reentrant:
*  No
*
*******************************************************************************/
l_u16 l_ifc_read_status_`$INSTANCE_NAME`(void)
{
    l_u16 returnValue;
    l_u8 interruptState;

    interruptState = CyEnterCriticalSection();

    /***************************************************************************
    * Update software shadow status register with the value of the hardware
    * status register to obtain current value of the edge detected bit. The
    * edge detected status bit is cleared immediately and the rest of the
    * software shadow status register bits are expected to be cleared in ISR.
    ***************************************************************************/
    `$INSTANCE_NAME`_auxStatus |= `$INSTANCE_NAME`_STATUS_REG;

    /* Optionally update interface status variable */
    if(0u != (`$INSTANCE_NAME`_auxStatus & `$INSTANCE_NAME`_STATUS_EDGE_DETECTED))
    {
        /* Set bus activity bit in IFC status */
        `$INSTANCE_NAME`_ifcStatus |= `$INSTANCE_NAME`_IFC_STS_BUS_ACTIVITY;

        /* Clear edge detected bit in shadow register variable */
        `$INSTANCE_NAME`_auxStatus &= ~`$INSTANCE_NAME`_STATUS_EDGE_DETECTED;

        #if(1u == `$INSTANCE_NAME`_INACTIVITY_ENABLED)

            /* Clear period timer counter */
            `$INSTANCE_NAME`_periodCounter = 0x00u;

        #endif /* (1u == `$INSTANCE_NAME`_INACTIVITY_ENABLED) */
    }

    /* Copy the global status variable to the local temp variable */
    returnValue = `$INSTANCE_NAME`_ifcStatus;

    /* Clear status variable */
    `$INSTANCE_NAME`_ifcStatus &= ~`$INSTANCE_NAME`_IFC_STS_MASK;

    CyExitCriticalSection(interruptState);

    /* Clear the "stats not checked" flag, since the status is now being checked by the user */
    `$INSTANCE_NAME`_status &= ~`$INSTANCE_NAME`_STATUS_NOT_CHECKED;

    /* Return the status in the temp variable */
    return (returnValue);
}


/*******************************************************************************
* Function Name: l_ifc_read_status
********************************************************************************
*
* Summary:
*  This function is defined by the LIN specification. This returns the status of
*  the specified LIN interface and then clears all status bits for that
*  interface. See Section 7.2.5.8 of the LIN 2.1 specification.
*
*
* Parameters:
*  iii - is the name of the interface handle.
*
* Return:
*  The status bits of the specified LIN interface are returned. These bits have
*  the following meanings:
*    [15:8]    Last Received PID
*    [7]        0
*    [6]        Save Configuration flag
*    [5]        0
*    [4]        Bus Activity flag
*    [3]        Go To Sleep flag
*    [2]        Overrun flag
*    [1]        Successful Transfer flag
*    [0]        Error in Response flag
*
* Global variables:
*  `$INSTANCE_NAME`_auxStatus  - shadow variable of the status register updated.
*
* Reentrant:
*  No
*
*******************************************************************************/
l_u16 l_ifc_read_status(l_ifc_handle iii)
{
    l_u16 returnValue;

    /* Determine which interface is specified */
    switch(iii)
    {
        /* Go here is interface 0 is specified */
        case `$INSTANCE_NAME`_IFC_HANDLE:
            returnValue = l_ifc_read_status_`$INSTANCE_NAME`();
        break;

        default:
            returnValue = (l_u16) CYRET_BAD_PARAM;
        break;
    }

    /* To remove unreferenced local variable warning */
    iii = iii;

    return (returnValue);
}


/*******************************************************************************
* Function Name: l_sys_irq_disable
********************************************************************************
*
* Summary:
*  This function disables all interrupts for the component. It returns a mask of
*  the state that the interruptmask bits were in. This function is essentially
*  equivalent to the DisableInt API of most components.
*
*  However, the returned value must be saved and later used with the
*  l_sys_irq_restore function to restore the interrupt state properly. It is
*  highly recommended that great care be taken when using this API routine. It
*  is likely that LIN communication failures will occur if the interrupts for
*  this component are disabled for too long.
*
*  This routine is supposed to be provided by the application. However, the LIN
*  Slave component implements this routine automatically. You can modify the
*  code in the routine if necessary.
*
* Parameters:
*  None
*
* Return:
*  Returns an interrupt register mask that defines the digital blocks for which
*  interrupts were disabled.
*
*******************************************************************************/
l_irqmask l_sys_irq_disable(void)`=ReentrantKeil("l_sys_irq_disable")`
{
    l_irqmask irqMask = 0u;

    if(1u == CyIntGetState(`$INSTANCE_NAME`_UART_ISR_NUMBER))
    {
        irqMask |= (0x01u << 0u);
        CyIntDisable(`$INSTANCE_NAME`_UART_ISR_NUMBER);
    }

    if(1u == CyIntGetState(`$INSTANCE_NAME`_BLIN_ISR_NUMBER))
    {
        irqMask |= (0x01u << 1u);
        CyIntDisable(`$INSTANCE_NAME`_BLIN_ISR_NUMBER);
    }

    return (irqMask);
}


/*******************************************************************************
* Function Name: l_sys_irq_restore
********************************************************************************
*
* Summary:
*  This function restores interrupts for the component. It should be used in
*  conjunction with l_sys_irq_disable. This function is essentially equivalent
*  to the EnableInt API of most components. However, it should not be called
*  when the component is being started.
*
*  This routine is supposed to be provided by the application. However, the LIN
*  Slave component implements this routine automatically. You can modify the
*  code in the routine if necessary.
*
* Parameters:
*  previous - interrupt mask that defines the digital blocks for which
*  interrupts will be enabled.
*
* Return:
*  None
*
*******************************************************************************/
void l_sys_irq_restore(l_irqmask previous)`=ReentrantKeil("l_sys_irq_restore")`
{
    if(0u != (previous & (0x01u << 0u)))
    {
        CyIntEnable(`$INSTANCE_NAME`_UART_ISR_NUMBER);
    }

    if(0u != (previous & (0x01u << 1u)))
    {
        CyIntEnable(`$INSTANCE_NAME`_BLIN_ISR_NUMBER);
    }
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_FindPidIndex
********************************************************************************
*
* Summary:
*  Returns index of the PID in `$INSTANCE_NAME`_volatileConfig.
*
* Parameters:
*  l_u8 pid - PID of the frame which index required.
*
* Return:
*  Index if the PID in `$INSTANCE_NAME`_volatileConfig,
*  0xFFu - if PID is not found
*
*******************************************************************************/
l_u8 `$INSTANCE_NAME`_FindPidIndex(l_u8 pid)`=ReentrantKeil($INSTANCE_NAME . "_FindPidIndex")`
{
    l_u8 i;
    l_u8 returnValue = `$INSTANCE_NAME`_INVALID_FRAME_PID;

    for(i = 0u; i <= `$INSTANCE_NAME`_NUM_FRAMES; i++)
    {
        if(pid == *(`$INSTANCE_NAME`_volatileConfig + i))
        {
            returnValue = i;

            /* Break the for loop */
            break;
        }
    }

    return (returnValue);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_EndFrame
********************************************************************************
*
* Summary:
*  Finishes the frame transmission.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global variables:
*  `$INSTANCE_NAME`_ifcStatus   - appropriate interface statuses bits are set.
*  `$INSTANCE_NAME`_j2602Status - appropriate statuses are set (optionally).
*  `$INSTANCE_NAME`_fsmFlags - appropriate flags of data transfer FSM are set.
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_EndFrame(l_u8 status)
{

    l_u8 interruptState;

    switch(status)
    {
        case `$INSTANCE_NAME`_HANDLING_DONT_SAVE_PID:

            /* Clear data received flag */
            `$INSTANCE_NAME`_fsmFlags &= ~`$INSTANCE_NAME`_FSM_DATA_RECEIVE;

            if(0u != (`$INSTANCE_NAME`_fsmFlags & `$INSTANCE_NAME`_FSM_OVERRUN))
            {
                /* Set overrun */
                `$INSTANCE_NAME`_ifcStatus |= `$INSTANCE_NAME`_IFC_STS_OVERRUN;
            }


        case `$INSTANCE_NAME`_HANDLING_SKIP_OVERRUN:

            /* Set Overrun flag */
            `$INSTANCE_NAME`_fsmFlags |= `$INSTANCE_NAME`_FSM_OVERRUN;

            #if(1u == `$INSTANCE_NAME`_TL_ENABLED)

                if(0u != (`$INSTANCE_NAME`_tlFlags & `$INSTANCE_NAME`_TL_TX_DIRECTION))
                {
                    /* Transport Layer Functions: Cooked Transport Layer API */
                    #if(1u == `$INSTANCE_NAME`_TL_API_FORMAT)

                        /* Set TL TX error status */
                        `$INSTANCE_NAME`_txTlStatus = LD_FAILED;

                    #else   /* Transport Layer Functions: Raw Transport Layer API */

                        /* Set TL TX error status */
                        `$INSTANCE_NAME`_txTlStatus = LD_TRANSMIT_ERROR;

                    #endif /* (1u == `$INSTANCE_NAME`_TL_API_FORMAT) */

                    /* Clear TL flags register */
                    `$INSTANCE_NAME`_tlFlags = 0u;
                }

                if(0u != (`$INSTANCE_NAME`_tlFlags & `$INSTANCE_NAME`_TL_RX_DIRECTION))
                {
                    /* Transport Layer Functions: Cooked Transport Layer API */
                    #if(1u == `$INSTANCE_NAME`_TL_API_FORMAT)

                        /* Set TL RX error status */
                        `$INSTANCE_NAME`_rxTlStatus = LD_FAILED;

                    #else   /* Transport Layer Functions: Raw Transport Layer API */

                        /* Set TL RX error status */
                        `$INSTANCE_NAME`_rxTlStatus = LD_RECEIVE_ERROR;

                    #endif /* (1u == `$INSTANCE_NAME`_TL_API_FORMAT) */

                    /* Clear TL flags register */
                    `$INSTANCE_NAME`_tlFlags = 0u;
                }

            #endif /* (1u == `$INSTANCE_NAME`_TL_ENABLED) */


        case `$INSTANCE_NAME`_HANDLING_RESET_FSM_ERR:

            /* Clear UART enable flag */
            `$INSTANCE_NAME`_fsmFlags &= ~`$INSTANCE_NAME`_FSM_UART_ENABLE_FLAG;

            /* Shutdown and disconnect UART, clear pending interrupts */
            CyIntDisable(`$INSTANCE_NAME`_UART_ISR_NUMBER);

            /* Disconnect bus RX from UART */
            `$INSTANCE_NAME`_CONTROL_REG |=  `$INSTANCE_NAME`_CONTROL_RX_DIS;

            /*******************************************************************
            * Clear UART Rx FIFO.
            * This should be done by calling UART_ClearRxBuffer() function, but
            * its current implementation clears only memory buffer, but not FIFO
            * one.
            *******************************************************************/
            interruptState = CyEnterCriticalSection();

            `$INSTANCE_NAME`_UART_RX_FIFO_REG |= `$INSTANCE_NAME`_UART_RX_FIFO_CLEAR;
            `$INSTANCE_NAME`_UART_RX_FIFO_REG &= ~`$INSTANCE_NAME`_UART_RX_FIFO_CLEAR;

            CyExitCriticalSection(interruptState);

            /* Clear any pending UART interrupt */
            CyIntClearPending(`$INSTANCE_NAME`_UART_ISR_NUMBER);

            /* Update UART ISR FSM state */
             #if(1u == `$INSTANCE_NAME`_AUTO_BAUD_RATE_SYNC)

                /* Auto Baud Rate Sync Enabled */
                `$INSTANCE_NAME`_uartFsmState = `$INSTANCE_NAME`_UART_ISR_STATE_1_PID;

            #else

                /* Auto Baud Rate Sync disabled */
                `$INSTANCE_NAME`_uartFsmState = `$INSTANCE_NAME`_UART_ISR_STATE_0_SNC;

            #endif  /* (1u == `$INSTANCE_NAME`_AUTO_BAUD_RATE_SYNC) */

        break;
    }
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetAssociatedFlags
********************************************************************************
*
* Summary:
*  This function sets appropriate flags. Used by ISR.
*
* Parameters:
*  Index of the PID in `$INSTANCE_NAME`_LinSlaveConfig.
*
* Return:
*  None
*
* Global variables:
*  `$INSTANCE_NAME`_statusFlagArray - the bit that corresponds to the passed PID
*  will be cleared in the global array where frame flags are stored will be set.
*
* Reentrant:
*  No
*
*******************************************************************************/
`$setAssociatedFlags`


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetEtFlagValue
********************************************************************************
*
* Summary:
*  This function gets value of appropriate event-triggered frame flag.
*
* Parameters:
*  Index of the PID in `$INSTANCE_NAME`_LinSlaveConfig.
*
* Return:
*  Current flag value.
*
* Global variables:
*  `$INSTANCE_NAME`_etFrameFlags - the bit that corresponds to the passed PID
*  will be returned from the global array where event-triggered flags of frames
*  are stored.
*
*******************************************************************************/
`$getEtFlagValue`


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ClearEtFlagValue
********************************************************************************
*
* Summary:
*  This function clears value of appropriate event-triggered frame flag.
*
* Parameters:
*  Index of the PID in `$INSTANCE_NAME`_LinSlaveConfig.
*
* Return:
*  None
*
* Global variables:
*  `$INSTANCE_NAME`_etFrameFlags - the bit that corresponds to the passed PID
*  will be cleared in the global array where event-triggered flags of frames
*  are stored.
*
* Reentrant:
*  No
*
*******************************************************************************/
`$clearEtFlagValue`


#if(1u == `$INSTANCE_NAME`_TL_ENABLED)

    /*******************************************************************************
    * Function Name: ld_init
    ********************************************************************************
    *
    * Summary:
    *  This call will (re)initialize the raw and the cooked layers on the interface
    *  iii.
    *
    *  All transport layer buffers will be initialized.
    *
    *  If there is an ongoing diagnostic frame transporting a cooked or raw message
    *  on the bus, it will not be aborted.
    *
    * Parameters:
    *  iii - Interface.
    *
    * Return:
    *  None
    *
    * Global variables:
    *  `$INSTANCE_NAME`_nad     - set NAD to the default value.
    *  `$INSTANCE_NAME`_prevPci - setting previous PCI variable to default.
    *  `$INSTANCE_NAME`_tlFlags - clear all transport layer flags.
    *  `$INSTANCE_NAME`_txTlStatus -  TX status is initialized.
    *  `$INSTANCE_NAME`_rxTlStatus -  RX status is initialized.
    *   Cooked API:
    *  `$INSTANCE_NAME`_tlLengthPointer -  pointer to data is set to NULL.
    *  `$INSTANCE_NAME`_txMessageLength - set to 0.
    *  `$INSTANCE_NAME`_rxMessageLength - set to 0.
    *  `$INSTANCE_NAME`_txTlDataPointer -  pointer to TX data is set to NULL.
    *  `$INSTANCE_NAME`_rxTlDataPointer -  pointer to RX data is set to NULL.
    *  `$INSTANCE_NAME`_rxTlInitDataPointer - pointer to RX init data is set to NULL.
    *   Raw API:
    *  `$INSTANCE_NAME`_txBufDepth - reset TX buffer depth.
    *  `$INSTANCE_NAME`_rxBufDepth - reset RX buffer depth.
    *  `$INSTANCE_NAME`_txWrIndex - reset TX write index.
    *  `$INSTANCE_NAME`_txRdIndex - reset TX read index.
    *  `$INSTANCE_NAME`_rxWrIndex - reset RX write index.
    *  `$INSTANCE_NAME`_rxRdIndex - reset RX read index.
    *
    * Reentrant:
    *  No
    *
    *******************************************************************************/
    void ld_init(l_ifc_handle iii)
    {
        l_u8 interruptState;

        /* To remove unreferenced local variable warning */
        iii = iii;

        interruptState = CyEnterCriticalSection();

        /* Set initial NAD as a current active NAD before initializing TL */
        `$INSTANCE_NAME`_nad = `$INSTANCE_NAME`_LinSlaveConfig.initialNad;

        /* Previous PCI requires to be unknown after the initialization */
        `$INSTANCE_NAME`_prevPci = `$INSTANCE_NAME`_PDU_PCI_TYPE_UNKNOWN;

        `$INSTANCE_NAME`_tlFlags = 0u;

        /* Enable interrupts */
        CyExitCriticalSection(interruptState);

        #if(1u == `$INSTANCE_NAME`_TL_API_FORMAT)

            while((`$INSTANCE_NAME`_rxTlStatus == LD_IN_PROGRESS) || \
                (`$INSTANCE_NAME`_txTlStatus == LD_IN_PROGRESS))
            {
                /* Wait until current message will be processed */
            }

            /* Save interrupt state and disable interupts */
            interruptState = CyEnterCriticalSection();

            /* Initialize Tx and Rx status variables correctly */
            `$INSTANCE_NAME`_txTlStatus = LD_COMPLETED;
            `$INSTANCE_NAME`_rxTlStatus = LD_COMPLETED;

            `$INSTANCE_NAME`_tlLengthPointer = NULL;

            /* Reset the frame counters */
            `$INSTANCE_NAME`_rxMessageLength = 0u;
            `$INSTANCE_NAME`_txMessageLength = 0u;
            
            /* Reset the frame counters */
            `$INSTANCE_NAME`_txFrameCounter = 0u;
            `$INSTANCE_NAME`_rxFrameCounter = 0u;

            `$INSTANCE_NAME`_rxTlDataPointer = NULL;
            `$INSTANCE_NAME`_rxTlInitDataPointer = NULL;

            /* Enable interrupts */
            CyExitCriticalSection(interruptState);

        #else

            /* Save interrupt state and disable interupts */
            interruptState = CyEnterCriticalSection();

            /* Reset buffers depth to 0, it will indicate the buffers are empty */
            `$INSTANCE_NAME`_txBufDepth = 0u;
            `$INSTANCE_NAME`_rxBufDepth = 0u;

            /* Raw API buffers initialization */

            `$INSTANCE_NAME`_txWrIndex = 0u;
            `$INSTANCE_NAME`_txRdIndex = 0u;

            `$INSTANCE_NAME`_rxWrIndex = 0u;
            `$INSTANCE_NAME`_rxRdIndex = 0u;

            `$INSTANCE_NAME`_txTlStatus = LD_QUEUE_EMPTY;
            `$INSTANCE_NAME`_rxTlStatus = LD_NO_DATA;

            /* Enable interrupts */
            CyExitCriticalSection(interruptState);

        #endif /* (1u == `$INSTANCE_NAME`_TL_API_FORMAT) */
    }


    /*******************************************************************************
    * Function Name: ld_read_configuration
    ********************************************************************************
    *
    * Summary:
    *  This function is used to read the NAD and PID values from volatile memory.
    *  This function can be used to read the current configuration data, and then
    *  save this data into non-volatile (flash) memory. The application should save
    *  the configuration data to flash when the "Save Configuration" bit is set in
    *  the LIN status register (returned by l_ifc_read_status_`$INSTANCE_NAME`).
    *  The configuration data that is read is a series of bytes. The first byte is
    *  the current NAD of the slave. The next bytes are the current PID values for
    *  the frames that the slave responds to. The PID values are in the order in
    *  which the frames appear in the LDF or NCF file.
    *
    * Parameters:
    *  iii - Interface.
    *
    * Return:
    *  LD_READ_OK - If the service was successful.
    *
    *  LD_LENGTH_TOO_SHORT - If the configuration size is greater than the length.
    *                        It means that the data area does not contain a valid
    *                        configuration.
    *
    * Global variables:
    *  pData - array is filled with data.
    *
    * Reentrant:
    *  No
    *
    *******************************************************************************/
    l_u8 ld_read_configuration(l_ifc_handle iii, l_u8* pData, l_u8* length)
    {
        l_u8 i;
        l_u8 result = `$INSTANCE_NAME`_LD_READ_OK;

        if(*length < `$INSTANCE_NAME`_NUM_FRAMES + 1u)
        {
            /* Return with no action when the requested length is smaller
            *  than the configuration data length.
            */
            result = `$INSTANCE_NAME`_LD_LENGTH_TOO_SHORT;
        }
        else
        {
            /* Copy over the configured NAD */
            pData[0u] = `$INSTANCE_NAME`_nad;

            /* Copy the data from the PID array to the data array */
            for (i = 0u; i < `$INSTANCE_NAME`_NUM_FRAMES; i++)
            {
                pData[i + 1u] = `$INSTANCE_NAME`_volatileConfig[i];
            }

            /* Set the length parameter to the actual length of the configuration data */
            *length = `$INSTANCE_NAME`_NUM_FRAMES + 1u;
        }

        /* To remove unreferenced local variable warning */
        iii = iii;

        /* Return status */
        return (result);
    }


    /*******************************************************************************
    * Function Name: ld_set_configuration
    ********************************************************************************
    *
    * Summary:
    *  This call will not transport anything on the bus.
    *
    *  The function will configure the NAD and the PIDs according to the
    *  configuration given by data. The intended usage is to restore a saved
    *  configuration or set an initial configuration (e.g. coded by I/O pins).
    *
    *  The function shall be called after calling l_ifc_init.
    *
    *  The caller shall set the size of the data area before calling the function.
    *
    *  The data contains the NAD and the PIDs and occupies one byte each.
    *  The structure of the data is: NAD and then all PIDs for the frames.
    *  The order of the PIDs are the same as the frame list in the LDF,
    *  Section 9.2.2.2, and NCF, Section 8.2.5.
    *
    * Parameters:
    *  iii - Interface.
    *
    * Return:
    *  LD_SET_OK - If the service was successful.
    *
    *  LD_LENGTH_NOT_CORRECT - If the required size of the configuration is not
    *                          equal to the given length.
    *
    *  LD_DATA_ERROR - The set of configuration could not be set. A an error
    *                  occurred while setting the configuration and the read back
    *                  configuration settings doesn't mach required settings.
    *
    * Global variables:
    *  `$INSTANCE_NAME`_nad - new NAD is set.
    *
    *  `$INSTANCE_NAME`_volatileConfig - new frame PIDs are copied.
    *
    * Reentrant:
    *  No
    *
    *******************************************************************************/
    l_u8 ld_set_configuration(l_ifc_handle iii, l_u8* pData, l_u16 length)
    {
        l_u8 i;
        l_u8 result = `$INSTANCE_NAME`_LD_SET_OK;

        if(length != `$INSTANCE_NAME`_NUM_FRAMES + 1u)
        {
            /* Return error if the length isn't correct */
            result = `$INSTANCE_NAME`_LD_LENGTH_NOT_CORRECT;
        }
        else
        {
            /* Copy NAD to a volatile memory */
            `$INSTANCE_NAME`_nad = pData[0u];

            /* Data read back */
            if(`$INSTANCE_NAME`_nad != pData[0u])
            {
                /* Indicate data error if NAD is not set correctly */
                result = `$INSTANCE_NAME`_LD_DATA_ERROR;
            }

            /* Copy Frame PIDs to a volatile memory */
            for(i = 0u; i < `$INSTANCE_NAME`_NUM_FRAMES; i++)
            {
                `$INSTANCE_NAME`_volatileConfig[i] = pData[i + 1u];

                /* Data read back */
                if(`$INSTANCE_NAME`_volatileConfig[i] != pData[i + 1u])
                {
                    /* Indicate data error if NAD is not set correctly */
                    result = `$INSTANCE_NAME`_LD_DATA_ERROR;
                }
            }
        }

        /* To remove unreferenced local variable warning */
        iii = iii;

        /* Return success code if the copy has completed */
        return(result);
    }

    #if(1u == `$INSTANCE_NAME`_CS_ENABLED)


        /*******************************************************************************
        * Function Name: `$INSTANCE_NAME`_LinProductId
        ********************************************************************************
        *
        * Summary:
        *  Verify that recieved LIN product identification matches.
        *
        * Parameters:
        *  frameData - pointer to a 4 bytes that holds LIN product ID.
        *
        * Return:
        *  0 - in case when LIN product IDs don't match.
        *  1 - in case when LIN product IDs do match.
        *
        * Reentrant:
        *  No
        *
        *******************************************************************************/
        l_bool `$INSTANCE_NAME`_LinProductId(volatile l_u8* frameData)
        {
            l_u8 i = 1u;

            if((frameData[0u] != LO8(`$INSTANCE_NAME`_slaveId.supplierId)) && \
                (frameData[0u] != LO8(`$INSTANCE_NAME`_CS_SUPPLIER_ID_WILDCARD)))
            {
                i = 0u;        /* Zero out i if the data isn't for this slave */
            }

            if((frameData[1u] != HI8(`$INSTANCE_NAME`_slaveId.supplierId)) && \
                (frameData[1u] != HI8(`$INSTANCE_NAME`_CS_SUPPLIER_ID_WILDCARD)))
            {
                i = 0u;        /* Zero out i if the data isn't for this slave */
            }

            if((frameData[2u] != LO8(`$INSTANCE_NAME`_slaveId.functionId)) && \
                (frameData[2u] != LO8(`$INSTANCE_NAME`_CS_FUNCTION_ID_WILDCARD)))
            {
                i = 0u;        /* Zero out i if the data isn't for this slave */
            }

            if((frameData[3u] != HI8(`$INSTANCE_NAME`_slaveId.functionId)) && \
                (frameData[3u] != HI8(`$INSTANCE_NAME`_CS_FUNCTION_ID_WILDCARD)))
            {
                i = 0u;        /* Zero out i if the data isn't for this slave */
            }

            return(i);
        }


        #if(1u == `$INSTANCE_NAME`_LIN_2_0)

            /*******************************************************************************
            * Function Name: `$INSTANCE_NAME`_MessageId
            ********************************************************************************
            *
            * Summary:
            *  Search for messahe ID in the LIN message ID table in case of success returns
            *  messahe ID index in the table.
            *
            * Parameters:
            *  frameData - The data pointer points to a data area with 2 bytes.
            *
            * Return:
            *  message ID index - in case of successful operation;
            *  LD_INVALID_MESSAGE_INDEX - in case when message ID wasn't found.
            *
            * Reentrant:
            *  No
            *
            *******************************************************************************/
            l_u8 `$INSTANCE_NAME`_MessageId(volatile l_u8* frameData)
            {
                l_u8 i = 0u;
                l_u8 result = LD_INVALID_MESSAGE_INDEX;

                while((i < `$INSTANCE_NAME`_NUM_FRAMES) && (result == LD_INVALID_MESSAGE_INDEX))
                {
                    /* If LSB of the messege ID from the table is equal to received one then
                    * Compare the MSB and in case of success set result to message index.
                    */
                    if(frameData[0u] == LO8(messageIdTable[i]))
                    {
                        if(frameData[1u] == HI8(messageIdTable[i]))
                        {
                            result = i;
                        }
                    }

                    i++;
                }

                return(result);
            }

        #endif /* (1u == `$INSTANCE_NAME`_LIN_2_0) */

        #if((0u != (`$INSTANCE_NAME`_CS_SEL_SERVICES01 & `$INSTANCE_NAME`_NCS_0xB2_SEL)) || \
            (0u != (`$INSTANCE_NAME`_CS_SEL_SERVICES01 & `$INSTANCE_NAME`_NCS_0xB3_SEL)))

            /*******************************************************************************
            * Function Name: ld_read_by_id_callout
            ********************************************************************************
            *
            * Summary:
            *  This callout is used when the master node transmits a read by identifier
            *  request with an identifier in the user defined area. The slave node
            *  application will be called from the driver when such request is received.
            *
            * Parameters:
            *  iii - Interface.
            *  id - The id parameter is the identifier in the user defined area (32 to 63),
            *  from the read by identifier configuration request.
            *  frameData - The data pointer points to a data area with 5 bytes. This area
            *  will be used by the application to set up the positive response.
            *
            * Return:
            *  LD_NEGATIVE_RESPONSE - The slave node will respond with a negative response.
            *  In this case the data area is not considered
            *
            *  LD_POSTIVE_RESPONSE - The slave node will setup a positive response using
            *  the data provided by the application.
            *
            *  LD_NO_RESPONSE - The slave node will not answer.
            *
            * Reentrant:
            *  No
            *
            *******************************************************************************/
            l_u8 ld_read_by_id_callout(l_ifc_handle iii, l_u8 id, volatile l_u8* frameData)
            {
                l_u8 result = LD_NEGATIVE_RESPONSE;

                /* User code required to handle user defined identification (Optional) */
                /* `#START LD_READ_BY_ID_CALLOUT_USER_SECTION` */

                /* `#END` */

                /* To remove unreferenced local variable warning */
                iii = iii;
                id = id;
                frameData = frameData;

                return(result);
            }

        #endif /* ((0u != (`$INSTANCE_NAME`_CS_SEL_SERVICES01 & `$INSTANCE_NAME`_NCS_0xB2_SEL)) ||
            (0u != (`$INSTANCE_NAME`_CS_SEL_SERVICES01 & `$INSTANCE_NAME`_NCS_0xB3_SEL))) */

    #endif /* (1u == `$INSTANCE_NAME`_CS_ENABLED) */


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_ProcessMrf
    ********************************************************************************
    *
    * Summary:
    *  This API is called from the ISR. It is responsible to parse the RX frames
    *  that come from a LIN Master. This API handles Automatic Configuration
    *  requests and receiving both Raw and Cooked API frames.
    *
    * Parameters:
    *  frame - Pointer to an array that contains a last received frame.
    *
    * Return:
    *  None
    *
    * Global variables:
    *
    *  LIN_srfBuffer - used to hold response for a service from ACRH.
    *
    *  `$INSTANCE_NAME`_rxTlStatus - the variable is analyzed by this function to check if
    *  status of the RX buffers is not full so the receiving of a frame could
    *  occur. Also this variable is modified by this function to indicate actual
    *  status of a transaction.
    *
    *  `$INSTANCE_NAME`_rxBufferDepth - is analyzed and modified by this API in the process of
    *  frame receiving.
    *
    *  `$INSTANCE_NAME`_prevPCI - Specifies the PCI of previous frame received by LIN. This
    *  variable is analyzed by the function in the process of frame receiving to
    *  get know what the previous frame was.
    *
    *  `$INSTANCE_NAME`_rxTlDataPointer - Pointer to a user buffer used in Cooked API. Modified
    *  by the function in the process of frame receiving.
    *
    *  `$INSTANCE_NAME`_tlInitialDataPointer - Pointer to the start of a user buffer used in
    *  Cooked API. Analyzed by the function in the process of frame receiving.
    *
    *  `$INSTANCE_NAME`_tlNadPointer - Pointer to a Slaves current NAD. Modified by the
    *  function in the process of frame receiving.
    *
    *  `$INSTANCE_NAME`_tlLengthPointer - Pointer to a current length of the user data.
    *  Analyzed and in case of requirement modified by the function in the process
    *  of frame receiving.
    *
    *  `$INSTANCE_NAME`_rxFrameCounter - Stores the value of the Frame Counter for the
    *  received frames. Modified by the function in the process of frame receiving.
    *
    *  `$INSTANCE_NAME`_rawRxQueue[] - Master Request Frame (MRF) buffer Modified by the
    *  function in the process of frame receiving (if Raw API selected).
    *
    *  `$INSTANCE_NAME`_ rxWrIndex - Stores RX buffer write index Modified by the function in
    *  the process of frame receiving(if Raw API selected).
    *
    *  `$INSTANCE_NAME`_rxRdIndex - Stores RX buffer read index Modified by the function in the
    *  process of frame receiving(if Raw API selected).
    *
    * Reentrant:
    *  No
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_ProcessMrf(volatile l_u8* frame)
    {
        l_u8 i;
        l_u8 tempStatus = 0u;

        #if(0u != (`$INSTANCE_NAME`_CS_SEL_SERVICES01 & `$INSTANCE_NAME`_NCS_0xB3_SEL))

            l_u8 idByte;
            l_u8 idMask;
            l_u8 idInvert;

        #endif /* (0u != (`$INSTANCE_NAME`_CS_SEL_SERVICES01 & `$INSTANCE_NAME`_NCS_0xB3_SEL)) */


        if(`$INSTANCE_NAME`_NAD_GO_TO_SLEEP == frame[`$INSTANCE_NAME`_PDU_NAD_IDX])
        {
            `$INSTANCE_NAME`_ifcStatus |= `$INSTANCE_NAME`_IFC_STS_GO_TO_SLEEP;
        }
        else if((`$INSTANCE_NAME`_nad == frame[`$INSTANCE_NAME`_PDU_NAD_IDX]) || \
                    (`$INSTANCE_NAME`_NAD_BROADCAST == frame[`$INSTANCE_NAME`_PDU_NAD_IDX]) || \
                        (`$INSTANCE_NAME`_NAD_FUNCTIONAL == frame[`$INSTANCE_NAME`_PDU_NAD_IDX]))
        {
            /* Single Frame detected */
            if(0u == (frame[`$INSTANCE_NAME`_PDU_PCI_IDX] & `$INSTANCE_NAME`_PDU_PCI_TYPE_MASK))
            {
                /* SID used for node configuration */
                switch(frame[`$INSTANCE_NAME`_PDU_SID_IDX])
                {
                    #if(1u == `$INSTANCE_NAME`_CS_ENABLED)

                        #if(0u != (`$INSTANCE_NAME`_CS_SEL_SERVICES01 & `$INSTANCE_NAME`_NCS_0xB0_SEL))

                            case `$INSTANCE_NAME`_NCS_ASSIGN_NAD:

                                if(1u == `$INSTANCE_NAME`_LinProductId(frame + 3u))  /* Check LIN Product ID */
                                {
                                    /* Save received NAD */
                                    `$INSTANCE_NAME`_nad = frame[`$INSTANCE_NAME`_PDU_D5_NEW_NAD_IDX];

                                    /* Fill the SRF Buffer with response for a service */
                                    /* Nad field should contain initial NAD */
                                    `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_NAD_IDX] = \
                                        `$INSTANCE_NAME`_LinSlaveConfig.initialNad;

                                    /* PCI is 0 so only length required */
                                    `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_PCI_IDX] = 1u;

                                    /* The RSID for a positive response is always SID + 0x40 */
                                    `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_SID_IDX] = \
                                        `$INSTANCE_NAME`_NCS_POS_RESP_ASSIGN_NAD;

                                    /* Fill unused data bytes with 0xFFs */
                                    for(i = 3u; i < `$INSTANCE_NAME`_FRAME_LEN; i++)
                                    {
                                        `$INSTANCE_NAME`_srfBuffer[i] = 0xFFu;
                                    }

                                    /* Set the service response bit that idicates that the response is
                                    * ready to be sent to master node.
                                    */
                                    `$INSTANCE_NAME`_status |= `$INSTANCE_NAME`_STATUS_SRVC_RSP_RDY;
                                }
                                else
                                {
                                    /* No response as the Supplier ID and the Function ID is invalid */
                                }

                            break;

                        #endif /* (`$INSTANCE_NAME`_CS_SEL_SERVICES01 & `$INSTANCE_NAME`_NCS_0xB0_SEL) */

                        #if(0u != (`$INSTANCE_NAME`_CS_SEL_SERVICES01 & `$INSTANCE_NAME`_NCS_0xB1_SEL))

                            case `$INSTANCE_NAME`_NCS_ASSIGN_FRAME_ID:  /* Obsolete, not implemented for LIN lin 2.1 */
                                                                        /* LIN 2.0 specification Only */
                                #if(1u == `$INSTANCE_NAME`_LIN_2_0)

                                    /* tempStatus is used in Supplier ID verification */
                                    tempStatus = 1u;

                                    if((frame[`$INSTANCE_NAME`_PDU_D1_IDX] !=  \
                                        LO8(`$INSTANCE_NAME`_slaveId.supplierId)) && \
                                            (frame[`$INSTANCE_NAME`_PDU_D1_IDX] != \
                                                LO8(`$INSTANCE_NAME`_CS_SUPPLIER_ID_WILDCARD)))
                                    {
                                        tempStatus = 0u;  /* Zero out tempStatus if the data isn't for this slave */
                                    }

                                    if((frame[`$INSTANCE_NAME`_PDU_D2_IDX] != \
                                        HI8(`$INSTANCE_NAME`_slaveId.supplierId)) && \
                                            (frame[`$INSTANCE_NAME`_PDU_D2_IDX] != \
                                                HI8(`$INSTANCE_NAME`_CS_SUPPLIER_ID_WILDCARD)))
                                    {
                                        tempStatus = 0u;   /* Zero out tempStatus if the data isn't for this slave */
                                    }

                                    /* If tempStatus is not 0 then verification passed succesfully */
                                    if(tempStatus != 0u)
                                    {
                                        /* Now tempStatus is used to hold Message ID Index */
                                        tempStatus = `$INSTANCE_NAME`_MessageId(frame + 5u);

                                        /* If Message ID index is valid then process the request and
                                        * prepare positive answer
                                        */
                                        if(tempStatus != LD_INVALID_MESSAGE_INDEX)
                                        {
                                            /* Set the PID in the position whete the valid index points */
                                            `$INSTANCE_NAME`_volatileConfig[tempStatus] =
                                                frame[`$INSTANCE_NAME`_PDU_D5_IDX];

                                            /* Fill the SRF Buffer with response for a service */
                                            /* Nad field should contain current NAD */
                                            `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_NAD_IDX] = \
                                                `$INSTANCE_NAME`_nad;

                                            /* PCI is 0 so only length required */
                                            `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_PCI_IDX] = 1u;

                                            /* The RSID for a positive response is always SID + 0x40 */
                                            `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_SID_IDX] = \
                                                `$INSTANCE_NAME`_NCS_POS_RESP_ASSIGN_FRAME_ID;

                                            /* Fill unused data bytes with 0xFFs */
                                            for(i = 3u; i < `$INSTANCE_NAME`_FRAME_LEN; i++)
                                            {
                                                `$INSTANCE_NAME`_srfBuffer[i] = 0xFFu;
                                            }

                                            /* Set the service response bit that idicates that the response is
                                            * ready to be sent to master node.
                                            */
                                            `$INSTANCE_NAME`_status |= `$INSTANCE_NAME`_STATUS_SRVC_RSP_RDY;
                                        }
                                    }

                                #else

                                    /* Do nothing ignore obsolete request */

                                #endif /* (1u == `$INSTANCE_NAME`_LIN_2_0) */
                            break;

                        #endif /* (`$INSTANCE_NAME`_CS_SEL_SERVICES01 & `$INSTANCE_NAME`_NCS_0xB1_SEL) */

                        #if(0u != (`$INSTANCE_NAME`_CS_SEL_SERVICES01 & `$INSTANCE_NAME`_NCS_0xB2_SEL))

                            case `$INSTANCE_NAME`_NCS_READ_BY_ID:

                                /* tempStatus is used to hold a status of `$INSTANCE_NAME`_LinProductId() */
                                tempStatus = `$INSTANCE_NAME`_LinProductId(frame + 4u);

                                /* LIN Product Identification (the only identifier is supported) */
                                if(`$INSTANCE_NAME`_NCS_READ_BY_ID_ID == frame[`$INSTANCE_NAME`_PDU_D1_IDX])
                                {
                                    if(1u == tempStatus)
                                    {
                                        /* Fill the SRF Buffer with response for a service */
                                        /* Nad field should contain current NAD */
                                        `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_NAD_IDX] = \
                                            `$INSTANCE_NAME`_nad;

                                        /* PCI is 0 so only length required */
                                        `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_PCI_IDX] = 6u;

                                        /* The RSID for a positive response is always SID + 0x40 */
                                        `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_SID_IDX] = \
                                            `$INSTANCE_NAME`_NCS_POS_RESP_READ_BY_ID;

                                        /* Fill data fields with Suplier and function IDs */
                                        `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_D1_IDX] = \
                                           LO8(`$INSTANCE_NAME`_slaveId.supplierId);
                                        `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_D2_IDX] = \
                                            HI8(`$INSTANCE_NAME`_slaveId.supplierId);
                                        `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_D3_IDX] = \
                                            LO8(`$INSTANCE_NAME`_slaveId.functionId);
                                        `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_D4_IDX] = \
                                            HI8(`$INSTANCE_NAME`_slaveId.functionId);
                                        `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_D5_IDX] = \
                                            `$INSTANCE_NAME`_slaveId.variant;

                                        /* Set the service response bit that idicates that the response is
                                        * ready to be sent to master node.
                                        */
                                        `$INSTANCE_NAME`_status |= `$INSTANCE_NAME`_STATUS_SRVC_RSP_RDY;
                                    }
                                    else
                                    {
                                        /* No action */
                                    }
                                }
                                else if(`$INSTANCE_NAME`_NCS_READ_BY_ID_SERIAL == frame[`$INSTANCE_NAME`_PDU_D1_IDX])
                                {
                                    /* Serial number identification*/

                                    /* If the slave serial number matchs recieved one
                                    * then prepare positive response.
                                    */
                                    if(1u == tempStatus)
                                    {
                                        /* Fill the SRF Buffer with response for a service */
                                        /* Nad field should contain current NAD */
                                        `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_NAD_IDX] = \
                                            `$INSTANCE_NAME`_nad;

                                        /* PCI is 0 so only length required */
                                        `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_PCI_IDX] = 5u;

                                        /* The RSID for a positive response is always SID + 0x40 */
                                        `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_SID_IDX] = \
                                            `$INSTANCE_NAME`_NCS_POS_RESP_READ_BY_ID;

                                        /* Fill unused data bytes with serial number ID */
                                        for(i = 3u; i < (`$INSTANCE_NAME`_FRAME_LEN - 1u); i++)
                                        {
                                            `$INSTANCE_NAME`_srfBuffer[i] = serialNumber[i - 3u];
                                        }

                                        /* The serial number is 4 byte length so set to 0xFF last unsued byte */
                                        `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_D5_IDX] = 0xFFu;

                                        /* Set the service response bit that idicates that the response is
                                        * ready to be sent to master node.
                                        */
                                        `$INSTANCE_NAME`_status |= `$INSTANCE_NAME`_STATUS_SRVC_RSP_RDY;
                                    }
                                    else
                                    {
                                         /* Do nothing serial number is invalid */
                                    }

                                }
                                else if((frame[`$INSTANCE_NAME`_PDU_D1_IDX] >= 32u) && \
                                (frame[`$INSTANCE_NAME`_PDU_D1_IDX] <= 63u))     /* User defined identification */
                                {

                                    if(1u == tempStatus)
                                    {
                                        /* If user didn't reassign the status of ld_read_by_id_callout() then
                                        * LD_NEGATIVE_RESPONSE will alwas be returned by ld_read_by_id_callout(). This will
                                        * indicate to master that the service by user defined identification is not
                                        * supported. tempStatus will be used to hold status of ld_read_by_id_callout().
                                        */
                                        tempStatus = ld_read_by_id_callout(`$INSTANCE_NAME`_IFC_HANDLE, \
                                            frame[`$INSTANCE_NAME`_PDU_D1_IDX], frame + 3u);

                                        if(tempStatus == LD_NEGATIVE_RESPONSE)
                                        {
                                            /* Fill the SRF Buffer with negative response for a service */
                                            /* Nad field should contain current NAD */
                                            `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_NAD_IDX] = \
                                                `$INSTANCE_NAME`_nad;

                                            /* PCI is 0 so only length required */
                                            `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_PCI_IDX] = 3u;

                                            /* The RSID for a positive response is always SID + 0x40 */
                                            `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_SID_IDX] = \
                                                `$INSTANCE_NAME`_NCS_RSID_NEG_REPLY;

                                            /* D1 will hold the service ID */
                                            `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_D1_ID_IDX] = \
                                                `$INSTANCE_NAME`_NCS_READ_BY_ID;

                                            /* D2 contains error code */
                                            `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_D2_IDX] = 0x12u;

                                            /* Fill unused data bytes with 0xFFs */
                                            for(i = 5u; i < `$INSTANCE_NAME`_FRAME_LEN; i++)
                                            {
                                                `$INSTANCE_NAME`_srfBuffer[i] = 0xFFu;
                                            }

                                            /* Set the service response bit that idicates that the response is
                                            * ready to be sent to master node.
                                            */
                                            `$INSTANCE_NAME`_status |= `$INSTANCE_NAME`_STATUS_SRVC_RSP_RDY;
                                        }
                                        else if(tempStatus == LD_POSITIVE_RESPONSE)
                                        {

                                            /* Fill the SRF Buffer with response for a service */
                                            /* Nad field should contain current NAD */
                                            `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_NAD_IDX] = \
                                                `$INSTANCE_NAME`_nad;

                                            /* PCI is 0 so only length required */
                                            `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_PCI_IDX] = 6u;

                                            /* The RSID for a positive response is always SID + 0x40 */
                                            `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_SID_IDX] = \
                                                `$INSTANCE_NAME`_NCS_POS_RESP_READ_BY_ID;

                                            /* Fill unused data bytes with user defined information */
                                            for(i = 3u; i < `$INSTANCE_NAME`_FRAME_LEN; i++)
                                            {
                                                `$INSTANCE_NAME`_srfBuffer[i] = frame[i];
                                            }

                                            /* Set the service response bit that idicates that the response is
                                            * ready to be sent to master node.
                                            */
                                            `$INSTANCE_NAME`_status |= `$INSTANCE_NAME`_STATUS_SRVC_RSP_RDY;
                                        }
                                        else
                                        {
                                            /* Do nothing as there no response from user */
                                        }
                                    }
                                    else
                                    {
                                         /* Do nothing serial number is invalid */
                                    }
                                }
                                else if((frame[`$INSTANCE_NAME`_PDU_D1_IDX] >= 16u) && \
                                    (frame[`$INSTANCE_NAME`_PDU_D1_IDX] <= 31u))     /* Message ID identification*/
                                {
                                    /* LIN 2.0 specification Only */
                                    #if(1u == `$INSTANCE_NAME`_LIN_2_0)

                                        /* If the slave serial number matchs recieved one
                                        * then prepare positive response.
                                        */
                                        if(1u == tempStatus)
                                        {
                                            /* Fill the SRF Buffer with response for a service */
                                            /* Nad field should contain current NAD */
                                            `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_NAD_IDX] = \
                                                `$INSTANCE_NAME`_nad;

                                            /* tempStatus now used to store calculated Message ID index */
                                            tempStatus = frame[`$INSTANCE_NAME`_PDU_D1_IDX] - LD_MESSAGE_ID_BASE;

                                            if(`$INSTANCE_NAME`_NUM_FRAMES > tempStatus)
                                            {
                                                /* PCI is 0 so only length required */
                                                `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_PCI_IDX] = 4u;

                                                /* The RSID for a positive response is always SID + 0x40 */
                                                `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_SID_IDX] = \
                                                    `$INSTANCE_NAME`_NCS_POS_RESP_READ_BY_ID;

                                                /* D1 = Message ID LSB */
                                                `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_D1_IDX] = \
                                                    HI8(messageIdTable[tempStatus]);

                                                /* D2 = Message ID MSB */
                                                `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_D2_IDX] = \
                                                    LO8(messageIdTable[tempStatus]);

                                                /* D3 = PID */
                                                `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_D3_IDX] = \
                                                    `$INSTANCE_NAME`_volatileConfig[tempStatus];

                                                /* The Message ID response is 3 byte length so set last two bytes
                                                * to 0xFF.
                                                */
                                                `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_D4_IDX] = 0xFFu;
                                                `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_D5_IDX] = 0xFFu;
                                            }
                                            else
                                            {
                                                /* PCI is 0 so only length required */
                                                `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_PCI_IDX] = 3u;

                                                /* The RSID for a positive response is always SID + 0x40 */
                                                `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_SID_IDX] = \
                                                    `$INSTANCE_NAME`_NCS_RSID_NEG_REPLY;

                                                /* D1 will hold the service ID */
                                                `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_D1_ID_IDX] = \
                                                    `$INSTANCE_NAME`_NCS_READ_BY_ID;

                                                /* D2 contains error code */
                                                `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_D2_IDX] = 0x12u;

                                                /* Fill unused data bytes with 0xFFs */
                                                for(i = 5u; i < `$INSTANCE_NAME`_FRAME_LEN; i++)
                                                {
                                                    `$INSTANCE_NAME`_srfBuffer[i] = 0xFFu;
                                                }
                                            }

                                            /* Set the service response bit that idicates that the response is
                                            * ready to be sent to master node.
                                            */
                                            `$INSTANCE_NAME`_status |= `$INSTANCE_NAME`_STATUS_SRVC_RSP_RDY;
                                        }

                                    #else

                                        /* Do nothing ignore errorneus request */

                                    #endif /* (1u == `$INSTANCE_NAME`_LIN_2_0) */
                                }

                            break;

                        #endif /* (`$INSTANCE_NAME`_CS_SEL_SERVICES01 & `$INSTANCE_NAME`_NCS_0xB2_SEL) */

                        #if(0u != (`$INSTANCE_NAME`_CS_SEL_SERVICES01 & `$INSTANCE_NAME`_NCS_0xB3_SEL))

                            case `$INSTANCE_NAME`_NCS_COND_CHANGE_NAD:

                                if(`$INSTANCE_NAME`_NCS_READ_BY_ID_ID == frame[`$INSTANCE_NAME`_PDU_D1_ID_IDX])
                                {
                                    /* LIN Product Identification */
                                    if((6u > frame[`$INSTANCE_NAME`_PDU_D2_BYTE_IDX]) && \
                                        (0u != frame[`$INSTANCE_NAME`_PDU_D2_BYTE_IDX]))
                                    {
                                        switch (frame[`$INSTANCE_NAME`_PDU_D2_BYTE_IDX])
                                        {
                                            /* tempStatus will be used as a teplorary variable to store ID byte */

                                            case `$INSTANCE_NAME`_CS_BYTE_SUPPLIER_ID1:

                                                tempStatus = LO8(`$INSTANCE_NAME`_slaveId.supplierId);

                                            break;

                                            case `$INSTANCE_NAME`_CS_BYTE_SUPPLIER_ID2:

                                                tempStatus = HI8(`$INSTANCE_NAME`_slaveId.supplierId);

                                            break;

                                            case `$INSTANCE_NAME`_CS_BYTE_FUNCTION_ID1:

                                                tempStatus = LO8(`$INSTANCE_NAME`_slaveId.functionId);

                                            break;

                                            case `$INSTANCE_NAME`_CS_BYTE_FUNCTION_ID2:

                                                tempStatus = HI8(`$INSTANCE_NAME`_slaveId.functionId);

                                            break;

                                            case `$INSTANCE_NAME`_CS_BYTE_VARIANT:

                                                tempStatus = `$INSTANCE_NAME`_slaveId.variant;

                                            break;

                                            default:

                                                /* This state should never be used */

                                            break;
                                        }

                                        if(0u == ((tempStatus ^ frame[`$INSTANCE_NAME`_PDU_D4_INVERT_IDX]) & \
                                            frame[`$INSTANCE_NAME`_PDU_D3_MASK_IDX]))
                                        {
                                            /* Change NAD to new NAD */
                                            `$INSTANCE_NAME`_nad = frame[`$INSTANCE_NAME`_PDU_D5_NEW_NAD_IDX];

                                            /* Fill the SRF Buffer with response for a service */
                                            /* Nad field should contain current NAD */
                                            `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_NAD_IDX] = \
                                                `$INSTANCE_NAME`_nad;

                                            /* PCI is 0 so only length required */
                                            `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_PCI_IDX] = 1u;

                                            /* The RSID for a positive response is always SID + 0x40 */
                                            `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_SID_IDX] = \
                                                `$INSTANCE_NAME`_NCS_POS_RESP_COND_CHANGE_NAD;

                                            /* Fill unused bytes with 0xFF */
                                            for(i = 3u; i < `$INSTANCE_NAME`_FRAME_LEN; i++)
                                            {
                                                `$INSTANCE_NAME`_srfBuffer[i] = 0xFFu;
                                            }

                                            /* Set the service response bit that idicates that the response is
                                            * ready to be sent to master node.
                                            */
                                            `$INSTANCE_NAME`_status |= `$INSTANCE_NAME`_STATUS_SRVC_RSP_RDY;
                                        }
                                        else
                                        {
                                            /* Do nothing ignore errorneus request */
                                        }
                                    }
                                    else
                                    {
                                        /* Do nothing ignore errorneus request */
                                    }
                                }
                                else if(`$INSTANCE_NAME`_NCS_READ_BY_ID_SERIAL == frame[`$INSTANCE_NAME`_PDU_D1_ID_IDX])
                                {
                                    if((5u < frame[`$INSTANCE_NAME`_PDU_D2_BYTE_IDX]) && \
                                        (0u != frame[`$INSTANCE_NAME`_PDU_D2_BYTE_IDX]))
                                    {
                                        /* Byte = 1 corresponds to the first byte (serialNumber[0]) */
                                        if(0u == ((serialNumber[frame[`$INSTANCE_NAME`_PDU_D2_BYTE_IDX] - 1u] ^ \
                                            frame[`$INSTANCE_NAME`_PDU_D4_INVERT_IDX]) & \
                                                frame[`$INSTANCE_NAME`_PDU_D3_MASK_IDX]))
                                        {
                                            /* Change NAD to new NAD */
                                            `$INSTANCE_NAME`_nad = frame[`$INSTANCE_NAME`_PDU_D5_NEW_NAD_IDX];

                                            /* Fill the SRF Buffer with response for a service */
                                            /* Nad field should contain current NAD */
                                            `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_NAD_IDX] = \
                                                `$INSTANCE_NAME`_nad;

                                            /* PCI is 0 so only length required */
                                            `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_PCI_IDX] = 1u;

                                            /* The RSID for a positive response is always SID + 0x40 */
                                            `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_SID_IDX] = \
                                                `$INSTANCE_NAME`_NCS_POS_RESP_COND_CHANGE_NAD;

                                            /* Fill unused bytes with 0xFF */
                                            for(i = 3u; i < `$INSTANCE_NAME`_FRAME_LEN; i++)
                                            {
                                                `$INSTANCE_NAME`_srfBuffer[i] = 0xFFu;
                                            }

                                            /* Set the service response bit that idicates that the response is
                                            * ready to be sent to master node.
                                            */
                                            `$INSTANCE_NAME`_status |= `$INSTANCE_NAME`_STATUS_SRVC_RSP_RDY;
                                        }
                                        else
                                        {
                                            /* Do nothing ignore errorneus request */
                                        }
                                    }
                                    else
                                    {
                                        /* Do nothing ignore errorneus request */
                                    }
                                }
                                else if((frame[`$INSTANCE_NAME`_PDU_D1_IDX] >= 32u) && \
                                    (frame[`$INSTANCE_NAME`_PDU_D1_IDX] <= 63u))     /* User defined identification*/
                                {
                                    /* Need to store Byte, Invert and Mask in a variables for user defined
                                    * identification as frame[] should contain user data after execution of
                                    * ld_read_by_id_callout();
                                    */
                                    idByte = frame[`$INSTANCE_NAME`_PDU_D2_BYTE_IDX] - 1u;
                                    idInvert = frame[`$INSTANCE_NAME`_PDU_D4_INVERT_IDX];
                                    idMask = frame[`$INSTANCE_NAME`_PDU_D3_MASK_IDX];

                                    /* If user didn't reassign the status of ld_read_by_id_callout() then
                                    * LD_NEGATIVE_RESPONSE will alwas be returned by ld_read_by_id_callout(). This will
                                    * indicate to master that the service by user defined identification is not
                                    * supported. tempStatus will be used to hold status of ld_read_by_id_callout().
                                    */
                                    tempStatus = ld_read_by_id_callout(`$INSTANCE_NAME`_IFC_HANDLE, \
                                        frame[`$INSTANCE_NAME`_PDU_D1_IDX], frame + `$INSTANCE_NAME`_PDU_D1_IDX);

                                    if((tempStatus == LD_NEGATIVE_RESPONSE) || (tempStatus == LD_NO_RESPONSE))
                                    {
                                        /* Do nothing as there no response from user */
                                    }
                                    else
                                    {
                                        if(0u == ((frame[idByte + `$INSTANCE_NAME`_PDU_D1_IDX] ^ idInvert) & idMask))
                                        {
                                            /* Change NAD to new NAD */
                                            `$INSTANCE_NAME`_nad = frame[`$INSTANCE_NAME`_PDU_D5_NEW_NAD_IDX];

                                            /* Fill the SRF Buffer with response for a service */
                                            /* Nad field should contain changed NAD */
                                            `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_NAD_IDX] = \
                                                `$INSTANCE_NAME`_nad;

                                            /* PCI is 0 so only length required */
                                            `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_PCI_IDX] = 1u;

                                            /* The RSID for a positive response is always SID + 0x40 */
                                            `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_SID_IDX] = \
                                                `$INSTANCE_NAME`_NCS_POS_RESP_COND_CHANGE_NAD;

                                            /* Fill unused bytes with 0xFF */
                                            for(i = 3u; i < `$INSTANCE_NAME`_FRAME_LEN; i++)
                                            {
                                                `$INSTANCE_NAME`_srfBuffer[i] = 0xFFu;
                                            }

                                            /* Set the service response bit that idicates that the response is
                                            * ready to be sent to master node.
                                            */
                                            `$INSTANCE_NAME`_status |= `$INSTANCE_NAME`_STATUS_SRVC_RSP_RDY;
                                        }
                                        else
                                        {
                                            /* Do nothing ignore errorneus request */
                                        }
                                    }
                                }
                                else
                                {
                                    /* Do nothing ignore errorneus request */
                                }
                            break;

                        #endif /* (`$INSTANCE_NAME`_CS_SEL_SERVICES01 & `$INSTANCE_NAME`_NCS_0xB3_SEL) */

                        #if(0u != (`$INSTANCE_NAME`_CS_SEL_SERVICES01 & `$INSTANCE_NAME`_NCS_0xB4_SEL))

                            case `$INSTANCE_NAME`_NCS_DATA_DUMP:

                                /* Not Supported */

                            break;

                        #endif /* (`$INSTANCE_NAME`_CS_SEL_SERVICES01 & `$INSTANCE_NAME`_NCS_0xB4_SEL) */

                        #if(0u != (`$INSTANCE_NAME`_CS_SEL_SERVICES01 & `$INSTANCE_NAME`_NCS_0xB5_SEL))

                            /* LIN Slave Node Position Detection */
                            case `$INSTANCE_NAME`_NCS_ASSIGN_NAD_SNPD:

                                #if(1u == `$INSTANCE_NAME`_SAE_J2602)

                                    `$INSTANCE_NAME`_ioctlStatus = `$INSTANCE_NAME`_IOCTL_STS_TARGET_RESET;

                                #endif /* (1u == `$INSTANCE_NAME`_SAE_J2602) */

                            break;

                        #endif /* (`$INSTANCE_NAME`_CS_SEL_SERVICES01 & `$INSTANCE_NAME`_NCS_0xB5_SEL) */

                        #if(0u != (`$INSTANCE_NAME`_CS_SEL_SERVICES01 & `$INSTANCE_NAME`_NCS_0xB6_SEL))

                            case `$INSTANCE_NAME`_NCS_SAVE_CONFIG:

                                /* Set save configuration bit in the status register */
                                `$INSTANCE_NAME`_ifcStatus |= `$INSTANCE_NAME`_IFC_STS_SAVE_CONFIG;

                                /* Fill the SRF Buffer with response for a service */
                                /* Nad field should contain current NAD */
                                `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_NAD_IDX] = \
                                    `$INSTANCE_NAME`_nad;

                                /* PCI is 0 so only length required */
                                `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_PCI_IDX] = 1u;

                                /* The RSID for a positive response is always SID + 0x40 */
                                `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_SID_IDX] = \
                                    `$INSTANCE_NAME`_NCS_POS_RESP_SAVE_CONFIG;

                                /* Fill unused data bytes with 0xFFs */
                                for(i = 3u; i < `$INSTANCE_NAME`_FRAME_LEN; i++)
                                {
                                    `$INSTANCE_NAME`_srfBuffer[i] = 0xFFu;
                                }

                                /* Set the service response bit that idicates that the response is
                                * ready to be sent to master node.
                                */
                                `$INSTANCE_NAME`_status |= `$INSTANCE_NAME`_STATUS_SRVC_RSP_RDY;

                            break;

                        #endif /* (`$INSTANCE_NAME`_CS_SEL_SERVICES01 & `$INSTANCE_NAME`_NCS_0xB6_SEL) */

                        #if(0u != (`$INSTANCE_NAME`_CS_SEL_SERVICES01 & `$INSTANCE_NAME`_NCS_0xB7_SEL))

                            case `$INSTANCE_NAME`_NCS_ASSIGN_FRAME_ID_RANGE:

                                /* Zero out the temp satus. It will be used as error counter. */
                                tempStatus = 0u;

                                for(i = 0u; i < `$INSTANCE_NAME`_NCS_MAX_FRAME_ID_RANGE; i++)
                                {
                                    if((i + frame[`$INSTANCE_NAME`_PDU_D1_START_IDX]) < `$INSTANCE_NAME`_NUM_FRAMES)
                                    {
                                        if((frame[i + `$INSTANCE_NAME`_PDU_D2_PID_IDX] != 0xFF) && \
                                            ((frame[i + `$INSTANCE_NAME`_PDU_D2_PID_IDX] &  \
                                                `$INSTANCE_NAME`_PID_PARITY_MASK) < `$INSTANCE_NAME`_FRAME_PID_MRF))
                                        {
                                            /* The unassign value "0" is used to invalidate
                                            *  this frame for transportation on the bus.
                                            */
                                            /* Set the new received PID value */
                                            `$INSTANCE_NAME`_volatileConfig[i + \
                                                frame[`$INSTANCE_NAME`_PDU_D1_START_IDX]] = \
                                                    frame[i + `$INSTANCE_NAME`_PDU_D2_PID_IDX];
                                        }
                                        else if(frame[i + `$INSTANCE_NAME`_PDU_D2_PID_IDX] == 0xFFu)
                                        {
                                            /* Do nothing. */
                                        }
                                        else
                                        {
                                            /* Indicate an error by changing the stutus other then 0
                                            * if the Frame ID is reserved.
                                            */
                                            tempStatus++;
                                        }
                                    }
                                    else
                                    {
                                        if(frame[i + `$INSTANCE_NAME`_PDU_D2_PID_IDX] != 0xFFu)
                                        {
                                            tempStatus++;  /* Indicate an error by changing the stutus other then 0 */
                                        }
                                    }
                                }

                                if(tempStatus == 0u) /* No errors condition check */
                                {
                                    /* Fill the SRF Buffer with response for a service */
                                    /* Nad field should contain current NAD */
                                    `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_NAD_IDX] = \
                                        `$INSTANCE_NAME`_nad;

                                    /* PCI is 0 so only length required */
                                    `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_PCI_IDX] = 1u;

                                    /* The RSID for a positive response is always SID + 0x40 */
                                    `$INSTANCE_NAME`_srfBuffer[`$INSTANCE_NAME`_PDU_SID_IDX] = \
                                        `$INSTANCE_NAME`_NCS_POS_RESP_ASSIGN_FRAME_ID_RANGE;

                                    /* Fill unused data bytes with 0xFFs */
                                    for(i = 3u; i < `$INSTANCE_NAME`_FRAME_LEN; i++)
                                    {
                                        `$INSTANCE_NAME`_srfBuffer[i] = 0xFFu;
                                    }

                                    /* Set the service response bit that idicates that the response is
                                    * ready to be sent to master node.
                                    */
                                    `$INSTANCE_NAME`_status |= `$INSTANCE_NAME`_STATUS_SRVC_RSP_RDY;
                                }
                                else
                                {
                                    /* Do nothing ignore errorneus request */
                                }

                            break;

                        #endif /* (`$INSTANCE_NAME`_CS_SEL_SERVICES01 & `$INSTANCE_NAME`_NCS_0xB7_SEL) */

                    #endif /* (1u == `$INSTANCE_NAME`_CS_ENABLED) */

                    default:

                       /* This will indicate that requested service is disabled and the Frame
                       * will be "passed" to TL. This means user should process this Frame
                       * properly usinf TL API.
                       */
                       `$INSTANCE_NAME`_tlFlags |= `$INSTANCE_NAME`_TL_CS_SERVICE_DISABLED;

                       /* Indicates that detected SID is a diagnostic SID and it should be
                       * passed to Transport Layer.
                       */
                      `$INSTANCE_NAME`_tlFlags |= `$INSTANCE_NAME`_TL_DIAG_FRAME_DETECTED;

                    break;
                }

                if((0u != (`$INSTANCE_NAME`_tlFlags & `$INSTANCE_NAME`_TL_CS_SERVICE_DISABLED)) || \
                    (0u != (`$INSTANCE_NAME`_tlFlags & `$INSTANCE_NAME`_TL_DIAG_FRAME_DETECTED)))
                {
                    /* SID used for diagnostics */
                    if(frame[`$INSTANCE_NAME`_PDU_PCI_IDX] <= `$INSTANCE_NAME`_PDU_SF_DATA_LEN)
                    {
                        #if(1u == `$INSTANCE_NAME`_TL_API_FORMAT) /* Cooked API */

                            /* Get one frame of a message if there is a message pending */
                            if(`$INSTANCE_NAME`_rxTlStatus == LD_IN_PROGRESS)
                            {
                                /* Make sure the pointer is points to the beginning of
                                * receive buffer.
                                */
                                if(0u != (`$INSTANCE_NAME`_tlFlags & `$INSTANCE_NAME`_TL_CS_SERVICE_DISABLED))
                                {
                                    `$INSTANCE_NAME`_rxTlDataPointer = `$INSTANCE_NAME`_rxTlInitDataPointer;
                                }
                                else
                                {
                                    /* Do nothing */
                                    /* `$INSTANCE_NAME`_rxTlDataPointer = `$INSTANCE_NAME`_rxTlDataPointer */
                                }

                                /* Copy data to user buffer */
                                for(i = 0u; i < frame[`$INSTANCE_NAME`_PDU_PCI_IDX]; i++)
                                {
                                    *`$INSTANCE_NAME`_rxTlDataPointer++ = frame[i + 2u];
                                }

                                /* Store the NAD */
                                *`$INSTANCE_NAME`_tlNadPointer = frame[`$INSTANCE_NAME`_PDU_NAD_IDX];

                                /* Get length of the data bytes */
                                *`$INSTANCE_NAME`_tlLengthPointer = (l_u16) frame[`$INSTANCE_NAME`_PDU_PCI_IDX];

                                /* Update length pointer properly */
                                `$INSTANCE_NAME`_rxMessageLength = 0u;

                                /* The SF message is received, so set the proper status */
                                `$INSTANCE_NAME`_rxTlStatus = LD_COMPLETED;
                            }

                        #else /* Raw API */

                            if(`$INSTANCE_NAME`_rxBufDepth < (`$INSTANCE_NAME`_TL_RX_QUEUE_LEN / 8u))
                            {
                                /* Fill the RX queue from a MRF buffer */
                                for(i = 0u; i < `$INSTANCE_NAME`_FRAME_DATA_SIZE_8; i++)
                                {
                                    `$INSTANCE_NAME`_rawRxQueue[`$INSTANCE_NAME`_rxWrIndex++] = frame[i];
                                }

                                /* Read index should point to the the next byte in MRF */
                                if(`$INSTANCE_NAME`_rxWrIndex == `$INSTANCE_NAME`_TL_RX_QUEUE_LEN)
                                {
                                    `$INSTANCE_NAME`_rxWrIndex = 0u;
                                }

                                /* 8 Bytes copied to MRF - increment buffer depth */
                                `$INSTANCE_NAME`_rxBufDepth++;

                                /* Specification doesn't require status of queue full
                                * so unconditionally set the status to data available
                                */
                                `$INSTANCE_NAME`_rxTlStatus = LD_DATA_AVAILABLE;
                            }

                        #endif /* (1u == `$INSTANCE_NAME`_TL_API_FORMAT) */
                    }
                    else
                    {
                        /* Do nothing. Length is not valid the data shouls not be trusted. */
                    }

                    /* Clear Service Disabled and Diagnostic Frame detected bits
                    * and process diagnostic frame receiving into a user buffer or MRF.
                    */
                    `$INSTANCE_NAME`_tlFlags &= \
                        ~(`$INSTANCE_NAME`_TL_CS_SERVICE_DISABLED | `$INSTANCE_NAME`_TL_DIAG_FRAME_DETECTED);

                }
                else
                {
                    /* Do nothing. Length is not valid the data shouls not be trusted. */
                }
            }
            else if((frame[`$INSTANCE_NAME`_PDU_PCI_IDX] & `$INSTANCE_NAME`_PDU_PCI_TYPE_MASK) == \
                `$INSTANCE_NAME`_PDU_PCI_TYPE_FF)                                 /* First Frame detected */
            {
                if(frame[`$INSTANCE_NAME`_PDU_LEN_IDX] > `$INSTANCE_NAME`_FRAME_DATA_SIZE_7)
                {
                    #if(1u == `$INSTANCE_NAME`_TL_API_FORMAT)    /* Cooked API */

                        /* Get one frame of a message if there is a message pending
                        * and the PCI is valid
                        */
                        if((`$INSTANCE_NAME`_rxTlStatus == LD_IN_PROGRESS) && \
                            (`$INSTANCE_NAME`_prevPci == `$INSTANCE_NAME`_PDU_PCI_TYPE_UNKNOWN ))
                        {
                            if(`$INSTANCE_NAME`_TL_BUF_LEN_MAX >= ((l_u16)
                                (((frame[`$INSTANCE_NAME`_PDU_PCI_IDX] & ~`$INSTANCE_NAME`_PDU_PCI_TYPE_MASK) << 8u) | \
                                    frame[`$INSTANCE_NAME`_PDU_LEN_IDX])))
                            {
                                /* Get First Frame Length with following two operations */
                                *`$INSTANCE_NAME`_tlLengthPointer = (l_u16)
                                    frame[`$INSTANCE_NAME`_PDU_PCI_IDX] & ~`$INSTANCE_NAME`_PDU_PCI_TYPE_MASK;

                                *`$INSTANCE_NAME`_tlLengthPointer = (l_u16)
                                    (*`$INSTANCE_NAME`_tlLengthPointer << 8u) | frame[`$INSTANCE_NAME`_PDU_LEN_IDX];

                                /* Copy Length to current length variable */
                                `$INSTANCE_NAME`_rxMessageLength = *`$INSTANCE_NAME`_tlLengthPointer;

                                for(i = 3u; i < `$INSTANCE_NAME`_FRAME_DATA_SIZE_8; i++)
                                {
                                    *`$INSTANCE_NAME`_rxTlDataPointer++ = frame[i];   /* Get Frame Data */
                                }

                                /* Update length pointer properly */
                                `$INSTANCE_NAME`_rxMessageLength -= `$INSTANCE_NAME`_FRAME_DATA_SIZE_5;

                                /* Save the state of the Frame Counter for monitor future possible errors */
                                `$INSTANCE_NAME`_rxFrameCounter = 0u;

                                /* Save the PCI type */
                                `$INSTANCE_NAME`_prevPci = `$INSTANCE_NAME`_PDU_PCI_TYPE_FF;
                            }
                            else
                            {
                                /* Do nothing Length is invalid. */
                            }
                        }

                    #else /* Raw API */

                        if(`$INSTANCE_NAME`_rxBufDepth < (`$INSTANCE_NAME`_TL_RX_QUEUE_LEN / 8u))
                        {
                            /* Fill the MRF from a frame buffer */
                            for(i = 0u; i < `$INSTANCE_NAME`_FRAME_DATA_SIZE_8; i++)
                            {
                                `$INSTANCE_NAME`_rawRxQueue[`$INSTANCE_NAME`_rxWrIndex++] = frame[i];
                            }

                            /* Read index should point to the the next byte in MRF */
                            if(`$INSTANCE_NAME`_rxWrIndex == `$INSTANCE_NAME`_TL_RX_QUEUE_LEN)
                            {
                                `$INSTANCE_NAME`_rxWrIndex = 0u;
                            }

                            /* 8 Bytes copied to MRF - increment buffer depth */
                            `$INSTANCE_NAME`_rxBufDepth++;

                            /* Specification doesn't require status of queue full
                            * so unconditionally set the status to data available
                            */
                            `$INSTANCE_NAME`_rxTlStatus = LD_DATA_AVAILABLE;
                        }

                    #endif /* (1u == `$INSTANCE_NAME`_TL_API_FORMAT) */
                }
                else
                {
                    /* Do nothing. Length is not valid the data shouls not be trusted. */
                }
            }
            else if((frame[`$INSTANCE_NAME`_PDU_PCI_IDX] & `$INSTANCE_NAME`_PDU_PCI_TYPE_MASK) == \
                `$INSTANCE_NAME`_PDU_PCI_TYPE_CF)                                 /* Consecutive Frames detected */
            {
                #if(1u == `$INSTANCE_NAME`_TL_API_FORMAT) /* Cooked API */

                    /* Get one frame of a message if there is a message pending and the
                    * PCI is valid.
                    */
                    if((`$INSTANCE_NAME`_rxTlStatus == LD_IN_PROGRESS) && \
                        ((`$INSTANCE_NAME`_prevPci == `$INSTANCE_NAME`_PDU_PCI_TYPE_FF) || \
                            (`$INSTANCE_NAME`_prevPci == `$INSTANCE_NAME`_PDU_PCI_TYPE_CF)))
                    {
                        /* Check if frame counter is valid */
                        if(((`$INSTANCE_NAME`_rxFrameCounter + 1u) == \
                            (frame[`$INSTANCE_NAME`_PDU_PCI_IDX] & ~`$INSTANCE_NAME`_PDU_PCI_TYPE_MASK)) || \
                                (((`$INSTANCE_NAME`_rxFrameCounter + 1u) == 16u) && \
                                ((frame[`$INSTANCE_NAME`_PDU_PCI_IDX] & ~`$INSTANCE_NAME`_PDU_PCI_TYPE_MASK) == 0u)))
                        {
                            for(i = 2u; i < `$INSTANCE_NAME`_FRAME_DATA_SIZE_8; i++)
                            {
                                *`$INSTANCE_NAME`_rxTlDataPointer++ = frame[i];    /* Get Frame Data */
                            }

                            /* Save current Frame Counter */
                            `$INSTANCE_NAME`_rxFrameCounter = \
                                frame[`$INSTANCE_NAME`_PDU_PCI_IDX] & ~`$INSTANCE_NAME`_PDU_PCI_TYPE_MASK;

                            /* Save the PCI type */
                            `$INSTANCE_NAME`_prevPci = `$INSTANCE_NAME`_PDU_PCI_TYPE_CF;

                            /* Update length pointer properly */
                            if(`$INSTANCE_NAME`_rxMessageLength >= `$INSTANCE_NAME`_FRAME_DATA_SIZE_6)
                            {
                                `$INSTANCE_NAME`_rxMessageLength -= `$INSTANCE_NAME`_FRAME_DATA_SIZE_6;
                            }
                            else
                            {
                                `$INSTANCE_NAME`_rxMessageLength = 0u;
                            }
                        }
                        else
                        {
                            /* Indicate an error if Frame Counter is invalid. */
                            `$INSTANCE_NAME`_rxTlStatus = LD_WRONG_SN;
                        }
                    }

                #else /* Raw API */

                    if(`$INSTANCE_NAME`_rxBufDepth < (`$INSTANCE_NAME`_TL_RX_QUEUE_LEN / 8u))
                    {
                        /* Fill the MRF from a frame buffer */
                        for(i = 0u; i < `$INSTANCE_NAME`_FRAME_DATA_SIZE_8; i++)
                        {
                            `$INSTANCE_NAME`_rawRxQueue[`$INSTANCE_NAME`_rxWrIndex++] = frame[i];
                        }

                        /* Read index should point to the the next byte in MRF */
                        if(`$INSTANCE_NAME`_rxWrIndex == `$INSTANCE_NAME`_TL_RX_QUEUE_LEN)
                        {
                            `$INSTANCE_NAME`_rxWrIndex = 0u;
                        }

                        /* 8 Bytes copied to MRF - increment buffer depth */
                        `$INSTANCE_NAME`_rxBufDepth++;

                        /* Specification doesn't require status of queue full
                        * so unconditionally set the status to data available
                        */
                        `$INSTANCE_NAME`_rxTlStatus = LD_DATA_AVAILABLE;
                    }

                #endif /* (1u == `$INSTANCE_NAME`_TL_API_FORMAT) */
            }
            else
            {
                /* Do nothing SID is invalid. */
            }
        }
        else  /* Indicate an error */
        {
            #if(1u == `$INSTANCE_NAME`_TL_API_FORMAT) /* Cooked API */

                /* Reception failed */
                if((0u != (`$INSTANCE_NAME`_tlFlags & `$INSTANCE_NAME`_TL_RX_DIRECTION)) &&
                    (0u != (`$INSTANCE_NAME`_tlFlags & `$INSTANCE_NAME`_TL_RX_REQUESTED)))
                {
                    `$INSTANCE_NAME`_rxTlStatus = LD_FAILED;
                }

            #endif /* (1u == `$INSTANCE_NAME`_TL_API_FORMAT) */
        }
    }

    #if(1u == `$INSTANCE_NAME`_TL_API_FORMAT)

        /*******************************************************************************
        * Function Name: ld_send_message
        ********************************************************************************
        *
        * Summary:
        *  The call packs the information specified by data and length into one or
        *  multiple diagnostic frames. If the call is made in a master node application
        *  the frames are transmitted to the slave node with the address NAD. If the
        *  call is made in a slave node application the frames are transmitted to the
        *  master node with the address NAD. The parameter NAD is not used in slave
        *  nodes.
        *
        *  The value of the SID (or RSID) shall be the first byte in the data area.
        *
        *  Length must be in the range of 1 to 4095 bytes. The length shall also include
        *  the SID (or RSID) value, i.e. message length plus one.
        *
        *  The call is asynchronous, i.e. not suspended until the message has been sent,
        *  and the buffer may not be changed by the application as long as calls to
        *  ld_tx_status returns LD_IN_PROGRESS.
        *
        *  The data is transmitted in suitable frames (master request frame for master
        *  nodes and slave response frame for slave nodes).
        *
        *  If there is a message in progress, the call will return with no action.
        *
        * Parameters:
        *  iii - Interface.
        *  length - Size of data to be sent in bytes.
        *  nad - Address of the slave node to which data is sent.
        *  data - Array of data to be sent. The value of the RSID is the first byte in 
        *  the data area.
        *
        * Return:
        *  None
        *
        * Reentrant:
        *  No
        *
        *******************************************************************************/
        void ld_send_message(l_ifc_handle iii, l_u16 length, l_u8 nad, l_u8* ld_data)
        {
            l_u8 interruptState;

            /* To remove unreferenced local variable warning */
            iii = iii;

            /* NAD isn't used in the slave node */
            nad = nad;

            if(`$INSTANCE_NAME`_txTlStatus != LD_IN_PROGRESS)
            {
                /* Interrupts chould be disabled as the global variables used by LIN ISR
                * is used below.
                */
                interruptState = CyEnterCriticalSection();

                `$INSTANCE_NAME`_txTlDataPointer = ld_data;

                /* Set up the length pointer, Length shouldn't be greater then
                * `$INSTANCE_NAME`_TL_BUF_LEN_MAX.
                */
                `$INSTANCE_NAME`_txMessageLength = length;

                /* Indicates that there is a message in progress */
                `$INSTANCE_NAME`_txTlStatus = LD_IN_PROGRESS;

                /* Indicates that Cooked API requested trasmit data */
                `$INSTANCE_NAME`_tlFlags |= `$INSTANCE_NAME`_TL_TX_REQUESTED;

                /* Restore the interrupt state */
                CyExitCriticalSection(interruptState);
            }
        }


        /*******************************************************************************
        * Function Name: ld_receive_message
        ********************************************************************************
        *
        * Summary:
        *  The call prepares the LIN diagnostic module to receive one message and store
        *  it in the buffer pointed to by data. At the call, length shall specify the
        *  maximum length allowed. When the reception has completed, length is changed
        *  to the actual length and NAD to the NAD in the message.
        *
        *  SID (or RSID) will be the first byte in the data area.
        *
        *  Length will be in the range of 1 to 4095 bytes, but never more than the value
        *  originally set in the call. SID (or RSID) is included in the length.
        *
        *  The parameter NAD is not used in slave nodes.
        *
        *  The call is asynchronous, i.e. not suspended until the message has been
        *  received, and the buffer may not be changed by the application as long as
        *  calls to ld_rx_status returns LD_IN_PROGRESS. If the call is made after the
        *  message transmission has commenced on the bus (i.e. the SF or FF is already
        *  transmitted), this message will not be received. Instead the function will
        *  wait until next message commence.
        *
        *  The data is received from the succeeding suitable frames (master request
        *  frame for slave nodes and slave response frame for master nodes).
        *
        *  The application shall monitor the ld_rx_status and shall not call this
        *  function until the status is LD_COMPLETED. Otherwise this function may
        *  return inconsistent data in the parameters.
        *
        * Parameters:
        *  iii - Interface.
        *  length: Size of data to be received in bytes.
        *  nad: Address of the slave node from which data is received.
        *  data: Array of data to be received. The value of the SID is the first byte 
        *  in the data area.
        *
        * Return:
        *  None
        *
        * Reentrant:
        *  No
        *
        *******************************************************************************/
        void ld_receive_message(l_ifc_handle iii, l_u16* const length, l_u8* const nad, l_u8* const ld_data)
        {
            l_u8 interruptState;

            /* To remove unreferenced local variable warning */
            iii = iii;

            if(`$INSTANCE_NAME`_rxTlStatus != LD_IN_PROGRESS)
            {
                /* Interrupts chould be disabled as the global variables used by LIN ISR
                * is used below.
                */
                interruptState = CyEnterCriticalSection();

                /* Set the user status bits */
                `$INSTANCE_NAME`_rxTlStatus = LD_IN_PROGRESS;

                /* Set up the data pointer */
                `$INSTANCE_NAME`_rxTlDataPointer = ld_data;

                /* Set up the initial data pointer that should
                * always point to the beginning of a user buffer.
                */
                `$INSTANCE_NAME`_rxTlInitDataPointer = ld_data;

                /* Set up the NAD pointer */
                `$INSTANCE_NAME`_tlNadPointer = nad;

                /* Set up the length pointer */
                `$INSTANCE_NAME`_tlLengthPointer = length;

                `$INSTANCE_NAME`_rxMessageLength = *length;

                /* Indicates that Cooked API requested receive data */
                `$INSTANCE_NAME`_tlFlags |= `$INSTANCE_NAME`_TL_RX_REQUESTED;

                /* Restore the interrupt state */
                CyExitCriticalSection(interruptState);
            }
        }


        /*******************************************************************************
        * Function Name: ld_tx_status
        ********************************************************************************
        *
        * Summary:
        *  The call returns the status of the last made call to ld_send_message.
        *
        * Parameters:
        *  iii - Interface.
        *
        * Return:
        *  LD_IN_PROGRESS - The transmission is not yet completed.
        *
        *  LD_COMPLETED - The transmission has completed successfully (and you can
        *                 issue a new ld_send_message call). This value is also
        *                 returned after initialization of the Transport Layer.
        *
        *  LD_FAILED - The transmission ended in an error. The data was only partially
        *              sent. The transport layer shall be reinitialized before
        *              processing further messages. To find out why a transmission has
        *              failed, check the status management function l_read_status.
        *
        *  LD_N_AS_TIMEOUT - The transmission failed because of a N_As timeout,
        *
        *
        * Reentrant:
        *  No
        *
        *******************************************************************************/
        uint8 ld_tx_status(l_ifc_handle iii)
        {
            /* To remove unreferenced local variable warning */
            iii = iii;

            return(`$INSTANCE_NAME`_txTlStatus);
        }


        /*******************************************************************************
        * Function Name: ld_rx_status
        ********************************************************************************
        *
        * Summary:
        *  The call returns the status of the last made call to ld_receive_message.
        *
        * Parameters:
        *  iii - Interface.
        *
        * Return:
        *  LD_IN_PROGRESS - The reception is not yet completed.
        *
        *  LD_COMPLETED - The reception has completed successfully and all information
        *                 (length, NAD, data) is available. (You can also issue a new
        *                 ld_receive_message call). This value is also returned after
        *                 initialization of the Tansport Layer.
        *
        *  LD_FAILED - The reception ended in an error. The data was only partially
        *              received and should not be trusted. Initialize before processing
        *              further transport layer messages. To find out why a reception
        *              has failed, check the status management function l_read_status.
        *
        *  LD_N_CR_TIMEOUT - The reception failed because of a N_Cr timeout,
        *
        *  LD_WRONG_SN - The reception failed because of an unexpected sequence
        *                number.
        *
        * Reentrant:
        *  No
        *
        *******************************************************************************/
        l_u8 ld_rx_status(l_ifc_handle iii)
        {
            /* To remove unreferenced local variable warning */
            iii = iii;

            return(`$INSTANCE_NAME`_rxTlStatus);
        }


    #else


        /*******************************************************************************
        * Function Name: ld_put_raw
        ********************************************************************************
        *
        * Summary:
        *  The call queues the transmission of 8 bytes of data in one frame. The data
        *  is sent in the next suitable frame (slave response frame). The data area
        *  will be copied in the call, the pointer will not be memorized. If no more
        *  queue resources are available, the data may be jettisoned and the
        *  appropriate error status will be set.
        *
        * Parameters:
        *  iii - Interface.
        *  data - Array of data to be sent.
        *
        * Return:
        *  None
        *
        * Reentrant:
        *  No
        *
        *******************************************************************************/
        void ld_put_raw(l_ifc_handle iii, const l_u8* const ld_data)
        {
            l_u8 interruptState;
            l_u8 i;

            /* To remove unreferenced local variable warning */
            iii = iii;

            /* Interrupts chould be disabled as the global variables used by LIN ISR
            * is used below.
            */
            interruptState = CyEnterCriticalSection();

            /* Copy data only when the buffer is not full */
            if(`$INSTANCE_NAME`_txBufDepth < (`$INSTANCE_NAME`_TL_TX_QUEUE_LEN / 8u))
            {
                /* Copy 8 bytes of data to SRF buffer */
                for(i = 0u; i < `$INSTANCE_NAME`_FRAME_DATA_SIZE_8; i++)
                {
                    /* Copy one byte of data to SRF buffer */
                    `$INSTANCE_NAME`_rawTxQueue[`$INSTANCE_NAME`_txWrIndex++] = *(ld_data + i);
                }

                /* If the end of the buffer is reached then reset the write index */
                if(`$INSTANCE_NAME`_txWrIndex == `$INSTANCE_NAME`_TL_TX_QUEUE_LEN)
                {
                    `$INSTANCE_NAME`_txWrIndex = 0u;
                }

                /* 8 bytes of data were copied so increment a buffer depth */
                `$INSTANCE_NAME`_txBufDepth++;

                /* Update the status properly */
                if(`$INSTANCE_NAME`_txBufDepth == (`$INSTANCE_NAME`_TL_TX_QUEUE_LEN / 8u))
                {
                    `$INSTANCE_NAME`_txTlStatus = LD_QUEUE_FULL;
                }
                else
                {
                    `$INSTANCE_NAME`_txTlStatus = LD_QUEUE_AVAILABLE;
                }
            }

            /* Restore the interrupt state */
            CyExitCriticalSection(interruptState);
        }


        /*******************************************************************************
        * Function Name: ld_get_raw
        ********************************************************************************
        *
        * Summary:
        *  The call copies the oldest received diagnostic frame data to the memory
        *  specified by data. The data returned is received from master request frame
        *  If the receive queue is empty no data will be copied.
        *
        * Parameters:
        *  iii - Interface.
        *  data - Array to which the oldest received diagnostic frame data will be 
        *  copied.
        *
        * Return:
        *  None
        *
        * Reentrant:
        *  No
        *
        *******************************************************************************/
        void ld_get_raw(l_ifc_handle iii, l_u8* const ld_data)
        {
            l_u8 interruptState;
            l_u8 i;

            /* To remove unreferenced local variable warning */
            iii = iii;

            /* Interrupts chould be disabled as the global variables used by LIN ISR
            * is used below.
            */
            interruptState = CyEnterCriticalSection();

            /* If queue is empty then do not copy anything */
            if(`$INSTANCE_NAME`_rxBufDepth != 0u)
            {
                /* Copy 8 bytes of data to SRF buffer */
                for(i = 0u; i < `$INSTANCE_NAME`_FRAME_DATA_SIZE_8; i++)
                {
                    *(ld_data + i) = `$INSTANCE_NAME`_rawRxQueue[`$INSTANCE_NAME`_rxRdIndex++];
                }

                /* 8 bytes of data were copied so decrement a buffer depth */
                `$INSTANCE_NAME`_rxBufDepth--;

                /* If the end of the buffer is reached then go to the start */
                if(`$INSTANCE_NAME`_rxRdIndex == `$INSTANCE_NAME`_TL_RX_QUEUE_LEN)
                {
                    `$INSTANCE_NAME`_rxRdIndex = 0u;
                }

                /* Update the status properly */
                if(`$INSTANCE_NAME`_rxBufDepth == 0u)
                {
                    `$INSTANCE_NAME`_rxTlStatus = LD_NO_DATA;
                }
                else
                {
                    `$INSTANCE_NAME`_rxTlStatus = LD_DATA_AVAILABLE;
                }
            }

            /* Restore the interrupt state */
            CyExitCriticalSection(interruptState);
        }


        /*******************************************************************************
        * Function Name: ld_raw_tx_status
        ********************************************************************************
        *
        * Summary:
        *  The call returns the status of the raw frame transmission function.
        *
        * Parameters:
        *  iii - Interface.
        *
        * Return:
        *  Status:
        *   LD_QUEUE_EMPTY - The transmit queue is empty. In case previous calls to
        *   ld_put_raw, all frames in the queue have been transmitted.
        *
        *   LD_QUEUE_AVAILABLE - The transmit queue contains entries, but is not full.
        *
        *   LD_QUEUE_FULL - The transmit queue is full and can not accept further
        *   frames.
        *
        *   LD_TRANSMIT_ERROR - LIN protocol errors occurred during the transfer;
        *   initialize and redo the transfer.
        *
        * Reentrant:
        *  No
        *
        *******************************************************************************/
        l_u8 ld_raw_tx_status(l_ifc_handle iii)
        {
            /* To remove unreferenced local variable warning */
            iii = iii;

            return(`$INSTANCE_NAME`_txTlStatus);
        }


        /*******************************************************************************
        * Function Name: `$INSTANCE_NAME`_ld_raw_rx_status
        ********************************************************************************
        *
        * Summary:
        *  The call returns the status of the raw frame receive function.
        *
        * Parameters:
        *  iii - Interface.
        *
        * Return:
        *  Status:
        *   LD_NO_DATA - The receive queue is empty.
        *
        *   LD_DATA_AVAILABLE - The receive queue contains data that can be read.
        *
        *   LD_RECEIVE_ERROR - LIN protocol errors occurred during the transfer;
        *
        *   initialize and redo the transfer.
        *
        * Reentrant:
        *  No
        *
        *******************************************************************************/
        l_u8 ld_raw_rx_status(l_ifc_handle iii)
        {
            /* To remove unreferenced local variable warning */
            iii = iii;

            return(`$INSTANCE_NAME`_rxTlStatus);
        }

    #endif /* (1u == `$INSTANCE_NAME`_TL_API_FORMAT) */

#endif /* (1u == `$INSTANCE_NAME`_TL_ENABLED) */

/* [] END OF FILE */
