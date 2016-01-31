/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  The CAN Component provide functionality of Control Area Network.
*  Two types of mailbox configurations available "Full" and "Basic".
*
* Note:
*  CAN configuration puts as constant to ROM and could change only directly
*  registers write by user.
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"

uint8 `$INSTANCE_NAME`_initVar = 0u;

/* Iniital values of CAN RX and TX registers */
const `$INSTANCE_NAME`_RX_CFG CYCODE `$INSTANCE_NAME`_RXConfigStruct[] =
{
    { 0u, 0x`$Rx0_Cmd`u, 0x`$Rx0_Amr`u, 0x`$Rx0_Acr`u },
    { 1u, 0x`$Rx1_Cmd`u, 0x`$Rx1_Amr`u, 0x`$Rx1_Acr`u },
    { 2u, 0x`$Rx2_Cmd`u, 0x`$Rx2_Amr`u, 0x`$Rx2_Acr`u },
    { 3u, 0x`$Rx3_Cmd`u, 0x`$Rx3_Amr`u, 0x`$Rx3_Acr`u },
    { 4u, 0x`$Rx4_Cmd`u, 0x`$Rx4_Amr`u, 0x`$Rx4_Acr`u },
    { 5u, 0x`$Rx5_Cmd`u, 0x`$Rx5_Amr`u, 0x`$Rx5_Acr`u },
    { 6u, 0x`$Rx6_Cmd`u, 0x`$Rx6_Amr`u, 0x`$Rx6_Acr`u },
    { 7u, 0x`$Rx7_Cmd`u, 0x`$Rx7_Amr`u, 0x`$Rx7_Acr`u },
    { 8u, 0x`$Rx8_Cmd`u, 0x`$Rx8_Amr`u, 0x`$Rx8_Acr`u },
    { 9u, 0x`$Rx9_Cmd`u, 0x`$Rx9_Amr`u, 0x`$Rx9_Acr`u },
    { 10u, 0x`$Rx10_Cmd`u, 0x`$Rx10_Amr`u, 0x`$Rx10_Acr`u },
    { 11u, 0x`$Rx11_Cmd`u, 0x`$Rx11_Amr`u, 0x`$Rx11_Acr`u },
    { 12u, 0x`$Rx12_Cmd`u, 0x`$Rx12_Amr`u, 0x`$Rx12_Acr`u },
    { 13u, 0x`$Rx13_Cmd`u, 0x`$Rx13_Amr`u, 0x`$Rx13_Acr`u },
    { 14u, 0x`$Rx14_Cmd`u, 0x`$Rx14_Amr`u, 0x`$Rx14_Acr`u },
    { 15u, 0x`$Rx15_Cmd`u, 0x`$Rx15_Amr`u, 0x`$Rx15_Acr`u }
};

const `$INSTANCE_NAME`_TX_CFG CYCODE `$INSTANCE_NAME`_TXConfigStruct[] =
{
    { 0u, 0x`$Tx0_Cmd`u, 0x`$Tx0_Id`u },
    { 1u, 0x`$Tx1_Cmd`u, 0x`$Tx1_Id`u },
    { 2u, 0x`$Tx2_Cmd`u, 0x`$Tx2_Id`u },
    { 3u, 0x`$Tx3_Cmd`u, 0x`$Tx3_Id`u },
    { 4u, 0x`$Tx4_Cmd`u, 0x`$Tx4_Id`u },
    { 5u, 0x`$Tx5_Cmd`u, 0x`$Tx5_Id`u },
    { 6u, 0x`$Tx6_Cmd`u, 0x`$Tx6_Id`u },
    { 7u, 0x`$Tx7_Cmd`u, 0x`$Tx7_Id`u }
};


/*******************************************************************************
* FUNCTION NAME:   `$INSTANCE_NAME`_Init
********************************************************************************
*
* Summary:
*  Inits/Restores default CAN configuration provided with customizer.
*
* Parameters:
*  None.
*
* Return:
*  Indication whether the configuration has been accepted or rejected.
*
* Side Effects:
*  All registers will be reset to their initial values. This will re-initialize 
*  the component with the following exceptions - it will not clear data from 
*  the mailboxes.
*  Enable power to the CAN Core.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_Init")`
{
    uint8 i;
    uint8 result = `$INSTANCE_NAME`_FAIL;
    uint8 localResult = CYRET_SUCCESS;
    uint8 enableInterrupts = 0u;
    
    enableInterrupts = CyEnterCriticalSection();
    
    /* Enable CAN block in Active mode */
    `$INSTANCE_NAME`_PM_ACT_CFG_REG |= `$INSTANCE_NAME`_ACT_PWR_EN;
        
    /* Enable CAN block in Alternate Active (Standby) mode */
    `$INSTANCE_NAME`_PM_STBY_CFG_REG |= `$INSTANCE_NAME`_STBY_PWR_EN; 
    
    CyExitCriticalSection(enableInterrupts);
    
    /* Sets the CAN controller to stop mode */
    `$INSTANCE_NAME`_CMD_REG.byte[0u] &= ~`$INSTANCE_NAME`_MODE_MASK;

    /* Verify that bit is cleared */
    if ((`$INSTANCE_NAME`_CMD_REG.byte[0u] & `$INSTANCE_NAME`_MODE_MASK) == 0u)
    {                
`$ISRInitialization` 
        if (`$INSTANCE_NAME`_SetPreScaler(`$INSTANCE_NAME`_BITRATE) == CYRET_SUCCESS)
        {
            if (`$INSTANCE_NAME`_SetArbiter(`$INSTANCE_NAME`_ARBITER) == CYRET_SUCCESS)
            {
                if (`$INSTANCE_NAME`_SetTsegSample(`$INSTANCE_NAME`_CFG_REG_TSEG1, `$INSTANCE_NAME`_CFG_REG_TSEG2,
                    `$INSTANCE_NAME`_CFG_REG_SJW, `$INSTANCE_NAME`_SAMPLING_MODE) == CYRET_SUCCESS)
                {
                    if (`$INSTANCE_NAME`_SetRestartType(`$INSTANCE_NAME`_RESET_TYPE) == CYRET_SUCCESS)
                    {
                        if (`$INSTANCE_NAME`_SetEdgeMode(`$INSTANCE_NAME`_SYNC_EDGE) == CYRET_SUCCESS)
                        {
                            /* Initialize TX maiboxes */
                            for(i = 0u; i < `$INSTANCE_NAME`_NUMBER_OF_TX_MAILBOXES; i++)
                            {
                                if (`$INSTANCE_NAME`_TxBufConfig((`$INSTANCE_NAME`_TX_CFG *)
                                    (&`$INSTANCE_NAME`_TXConfigStruct[i])) != CYRET_SUCCESS)
                                {
                                    localResult = `$INSTANCE_NAME`_FAIL;
                                    break;
                                }
                            }
    
                            if (localResult == CYRET_SUCCESS)
                            {
                                /* Initialize RX mailboxes */
                                for(i = 0u; i < `$INSTANCE_NAME`_NUMBER_OF_RX_MAILBOXES; i++)
                                {
                                    if (`$INSTANCE_NAME`_RxBufConfig((`$INSTANCE_NAME`_RX_CFG *)
                                        (&`$INSTANCE_NAME`_RXConfigStruct[i])) != CYRET_SUCCESS)
                                    {
                                        localResult = `$INSTANCE_NAME`_FAIL;
                                        break;
                                    }
                                }
    
                                if (localResult == CYRET_SUCCESS)
                                {
                                    /* Write IRQ Mask */
                                    if (`$INSTANCE_NAME`_SetIrqMask(`$INSTANCE_NAME`_INIT_INTERRUPT_MASK) ==
                                        CYRET_SUCCESS)
                                    {
                                        /* Active mode always */
                                        if (`$INSTANCE_NAME`_SetOpMode(`$INSTANCE_NAME`_ACTIVE_MODE) == CYRET_SUCCESS)
                                        {
                                            result = CYRET_SUCCESS;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    return(result);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Enable
********************************************************************************
*
* Summary:   
*  This function enable CAN Component and ISR of CAN Component. 
*
* Parameters:  
*  None.
*
* Return: 
*  Indication whether register is written and verified..
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`
{
    uint8 result = `$INSTANCE_NAME`_FAIL;
    uint8 enableInterrupts = 0u;
    
    enableInterrupts = CyEnterCriticalSection();
    
    /* Enable CAN block in Active mode */
    `$INSTANCE_NAME`_PM_ACT_CFG_REG |= `$INSTANCE_NAME`_ACT_PWR_EN;

    /* Enable CAN block in Alternate Active (Standby) mode */
    `$INSTANCE_NAME`_PM_STBY_CFG_REG |= `$INSTANCE_NAME`_STBY_PWR_EN;
    
    CyExitCriticalSection(enableInterrupts);
    
`$ISREnable`
    /* Sets the CAN controller to run mode */
    `$INSTANCE_NAME`_CMD_REG.byte[0u] |= `$INSTANCE_NAME`_MODE_MASK;
    
    /* Verify that bit is set */
    if ((`$INSTANCE_NAME`_CMD_REG.byte[0u] & `$INSTANCE_NAME`_MODE_MASK) == `$INSTANCE_NAME`_MODE_MASK)
    {
        result = CYRET_SUCCESS;
    }
    
    return(result);
}


/*******************************************************************************
* FUNCTION NAME:   `$INSTANCE_NAME`_Start
********************************************************************************
*
* Summary:
*  This function sets CAN Component into Run mode. Starts the Rate Counter if
*  polling mailboxes available.
*
* Parameters:
*  None. 
*
* Return:
*  Indication whether register is written and verified.  
*
* Global variables:
*  `$INSTANCE_NAME`_initVar - used to check initial configuration, modified on
*  first function call.
*
* Reentrant:
*  No.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_Start(void) `=ReentrantKeil($INSTANCE_NAME . "_Start")`
{
    uint8 result = `$INSTANCE_NAME`_FAIL;
    
    if (`$INSTANCE_NAME`_initVar == 0u)
    {
        result = `$INSTANCE_NAME`_Init();
        `$INSTANCE_NAME`_initVar = 1u;        
    }
    
    if (result == CYRET_SUCCESS)
    {
        result = `$INSTANCE_NAME`_Enable();
    }

    return(result);
}


/*******************************************************************************
* FUNCTION NAME:   `$INSTANCE_NAME`_Stop
********************************************************************************
*
* Summary:
*  This function sets CAN Component into Stop mode.
*
* Parameters:
*  None.
*
* Return:
*  Indication whether register is written and verified.
*
* Side Effects:
*  Disable power to the CAN Core.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`
{
    uint8 result = `$INSTANCE_NAME`_FAIL;
    uint8 enableInterrupts = 0u;        
    
    /* Sets the CAN controller to stop mode */
    `$INSTANCE_NAME`_CMD_REG.byte[0u] &= ~`$INSTANCE_NAME`_MODE_MASK;

    /* Verify that bit is cleared */
    if ((`$INSTANCE_NAME`_CMD_REG.byte[0u] & `$INSTANCE_NAME`_MODE_MASK) == 0u)
    {
        result = CYRET_SUCCESS;
    }
    
`$ISRDisable`
    enableInterrupts = CyEnterCriticalSection(); 

    /* Disable CAN block in Active mode */
    `$INSTANCE_NAME`_PM_ACT_CFG_REG &= ~`$INSTANCE_NAME`_ACT_PWR_EN;

    /* Disable CAN block in Alternate Active (Standby) mode template */
    `$INSTANCE_NAME`_PM_STBY_CFG_REG &= ~`$INSTANCE_NAME`_STBY_PWR_EN;
    
    CyExitCriticalSection(enableInterrupts);

    return(result);
}


/*******************************************************************************
* FUNCTION NAME:   `$INSTANCE_NAME`_GlobalIntEnable
********************************************************************************
*
* Summary:
*  This function enables Global Interrupts from CAN Core.
*
* Parameters:
*  None.
*
* Return:
*  Indication whether register is written and verified.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_GlobalIntEnable(void) `=ReentrantKeil($INSTANCE_NAME . "_GlobalIntEnable")`
{
    uint8 result = `$INSTANCE_NAME`_FAIL;

    `$INSTANCE_NAME`_INT_EN_REG.byte[0u] |= `$INSTANCE_NAME`_GLOBAL_INT_MASK;

    /* Verify that bit is set */
    if ((`$INSTANCE_NAME`_INT_EN_REG.byte[0u] & `$INSTANCE_NAME`_GLOBAL_INT_MASK) == `$INSTANCE_NAME`_GLOBAL_INT_MASK)
    {
        result = CYRET_SUCCESS;
    }

    return(result);
}


/*******************************************************************************
* FUNCTION NAME:   `$INSTANCE_NAME`_GlobalIntDisable
********************************************************************************
*
* Summary:
*  This function disables Global Interrupts from CAN Core.
*
* Parameters:
*  None.
*
* Return:
*  Indication whether register is written and verified.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_GlobalIntDisable(void) `=ReentrantKeil($INSTANCE_NAME . "_GlobalIntDisable")`
{
    uint8 result = `$INSTANCE_NAME`_FAIL;

    `$INSTANCE_NAME`_INT_EN_REG.byte[0u] &= ~`$INSTANCE_NAME`_GLOBAL_INT_MASK;

    /* Verify that bit is cleared */
    if ((`$INSTANCE_NAME`_INT_EN_REG.byte[0u] & `$INSTANCE_NAME`_GLOBAL_INT_MASK) == 0u)
    {
        result = CYRET_SUCCESS;
    }

    return(result);
}


/*******************************************************************************
* FUNCTION NAME:   `$INSTANCE_NAME`_SetPreScaler
********************************************************************************
*
* Summary:
*  This function sets PreScaler for generation the time quantum which defines
*  the time quanta. Value between 0x0 and 0x7FFF are valid.
*
* Parameters:
*  bitrate: PreScaler value.
*
* Return:
*  Indication whether register is written and verified.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_SetPreScaler(uint16 bitrate) `=ReentrantKeil($INSTANCE_NAME . "_SetPreScaler")`
{
    uint8 result = `$INSTANCE_NAME`_OUT_OF_RANGE;

    if (bitrate <= `$INSTANCE_NAME`_BITRATE_MASK)
    {
        result = `$INSTANCE_NAME`_FAIL;

        /* Set prescaler */
        CY_SET_REG16((reg16 *) &`$INSTANCE_NAME`_CFG_REG.byte[2u], bitrate);

        /* Verify that prescaler is set */
        if (CY_GET_REG16((reg16 *) &`$INSTANCE_NAME`_CFG_REG.byte[2u]) == bitrate)
        {
            result = CYRET_SUCCESS;
        }
    }

    return(result);
}


/*******************************************************************************
* FUNCTION NAME:   `$INSTANCE_NAME`_SetArbiter
********************************************************************************
*
* Summary:
*  This function sets arbitration type for transmit mailboxes. Types of
*  arbiters are Round Robin and Fixed priority. Value 0 and 1 are valid.
*
* Parameters:
*  arbiter: Arbitaration type for transmit mailboxes.
*
* Return:
*  Indication whether register is written and verified.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_SetArbiter(uint8 arbiter) `=ReentrantKeil($INSTANCE_NAME . "_SetArbiter")`
{
    uint8 result = `$INSTANCE_NAME`_FAIL;

    if (arbiter == `$INSTANCE_NAME`_ROUND_ROBIN)
    {
        `$INSTANCE_NAME`_CFG_REG.byte[1u] &= ~`$INSTANCE_NAME`_ARBITRATION_MASK;    /* Round Robin */

        /* Verify that bit is cleared */
        if ((`$INSTANCE_NAME`_CFG_REG.byte[1u] & `$INSTANCE_NAME`_ARBITRATION_MASK) == 0u)
        {
            result = CYRET_SUCCESS;
        }        
    }
    else
    {
        `$INSTANCE_NAME`_CFG_REG.byte[1u] |= `$INSTANCE_NAME`_ARBITRATION_MASK;    /* Fixed priority */

        /* Verify that bit is set */
        if ((`$INSTANCE_NAME`_CFG_REG.byte[1u] & `$INSTANCE_NAME`_ARBITRATION_MASK) ==
            `$INSTANCE_NAME`_ARBITRATION_MASK)
        {
            result = CYRET_SUCCESS;
        }
    }

    return(result);
}


/*******************************************************************************
* FUNCTION NAME: `$INSTANCE_NAME`_SetTsegSample
********************************************************************************
*
* Summary:
*  This function configures: Time segment 1, Time segment 2, Sampling Mode
*  and Synchronization Jump Width.
*
* Parameters:
*  cfgTseg1: Time segment 1, value between 0x2 and 0xF are valid;
*  cfgTseg2: Time segment 2, value between 0x1 and 0x7 are valid;
*  sjw: Synchronization Jump Width, value between 0x0 and 0x3 are valid;
*  sm: Sampling Mode, one or three sampling points are used.
*
* Return:
*  Indication whether register is written and verified.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_SetTsegSample(uint8 cfgTseg1, uint8 cfgTseg2, uint8 sjw, uint8 sm)
                                     `=ReentrantKeil($INSTANCE_NAME . "_SetTsegSample")`
{
    uint8 result = `$INSTANCE_NAME`_OUT_OF_RANGE;
    uint8 cfgTemp;

    if ((cfgTseg1 >= `$INSTANCE_NAME`_CFG_REG_TSEG1_LOWER_LIMIT) && (cfgTseg1 <=
         `$INSTANCE_NAME`_CFG_REG_TSEG1_UPPER_LIMIT))
    {
        if (((cfgTseg2 >= `$INSTANCE_NAME`_CFG_REG_TSEG2_LOWER_LIMIT) &&
             (cfgTseg2 <= `$INSTANCE_NAME`_CFG_REG_TSEG2_UPPER_LIMIT)) || ((sm == `$INSTANCE_NAME`_ONE_SAMPLE_POINT) &&
             (cfgTseg2 == `$INSTANCE_NAME`_CFG_REG_TSEG2_EXCEPTION)))
        {
            if ((sjw <= `$INSTANCE_NAME`_CFG_REG_SJW_LOWER_LIMIT) && (sjw <= cfgTseg1) && (sjw <= cfgTseg2))
            {
                result = `$INSTANCE_NAME`_FAIL;

                cfgTemp = `$INSTANCE_NAME`_CFG_REG.byte[1];
                cfgTemp &= ~`$INSTANCE_NAME`_CFG_REG_TSEG1_MASK;
                cfgTemp |= cfgTseg1;

                /* write register byte 1 */
                `$INSTANCE_NAME`_CFG_REG.byte[1u] = cfgTemp;

                /* Verify 2nd byte of `$INSTANCE_NAME`_CFG_REG register */
                if (`$INSTANCE_NAME`_CFG_REG.byte[1u] == cfgTemp)
                {
                    /* read and clean bits */
                    cfgTemp = `$INSTANCE_NAME`_CFG_REG.byte[0u];
                    cfgTemp &= ~(`$INSTANCE_NAME`_CFG_REG_TSEG2_MASK | `$INSTANCE_NAME`_CFG_REG_SJW_MASK |
                                  `$INSTANCE_NAME`_SAMPLE_MODE_MASK);

                    cfgTemp = 0u;
                    /* Set appropriate bits */
                    if (sm != `$INSTANCE_NAME`_ONE_SAMPLE_POINT)
                    {
                        cfgTemp |= `$INSTANCE_NAME`_SAMPLE_MODE_MASK;
                    }
                    cfgTemp |= ((cfgTseg2 << `$INSTANCE_NAME`_CFG_REG_TSEG2_SHIFT) |
                                 (sjw << `$INSTANCE_NAME`_CFG_REG_SJW_SHIFT));

                    /* write register byte 0 */
                    `$INSTANCE_NAME`_CFG_REG.byte[0u] = cfgTemp;

                    /* Verify 1st byte of `$INSTANCE_NAME`_CFG_REG register */
                    if (`$INSTANCE_NAME`_CFG_REG.byte[0u] == cfgTemp)
                    {
                        result = CYRET_SUCCESS;
                    }
                }
            }
        }
    }

    return(result);
}


/*******************************************************************************
* FUNCTION NAME:   `$INSTANCE_NAME`_SetRestartType
********************************************************************************
*
* Summary:
*  This function sets Reset type. Types of Reset are Automatic and Manual.
*  Manual Reset is recommended setting. Value 0 and 1 are valid.
*
* Parameters:
*  reset: Reset Type.
*
* Return:
*  Indication whether register is written and verified.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_SetRestartType(uint8 reset) `=ReentrantKeil($INSTANCE_NAME . "_SetRestartType")`
{
    uint8 result = `$INSTANCE_NAME`_FAIL;

    if (reset == `$INSTANCE_NAME`_RESTART_BY_HAND)
    {
        `$INSTANCE_NAME`_CFG_REG.byte[0u] &= ~`$INSTANCE_NAME`_RESET_MASK;   /* Manual reset */

        /* Verify that bit is cleared */
        if ((`$INSTANCE_NAME`_CFG_REG.byte[0u] & `$INSTANCE_NAME`_RESET_MASK) == 0u)
        {
            result = CYRET_SUCCESS;
        }
    }
    else
    {
        `$INSTANCE_NAME`_CFG_REG.byte[0u] |= `$INSTANCE_NAME`_RESET_MASK;    /* Automatic restart */

        /* Verify that bit is set */
        if ((`$INSTANCE_NAME`_CFG_REG.byte[0u] & `$INSTANCE_NAME`_RESET_MASK) == `$INSTANCE_NAME`_RESET_MASK)
        {
            result = CYRET_SUCCESS;
        }
    }

    return(result);
}


/*******************************************************************************
* FUNCTION NAME:   `$INSTANCE_NAME`_SetEdgeMode
********************************************************************************
*
* Summary:
*  This function sets Edge Mode. Modes are 'R' to 'D'(Reccessive to Dominant)
*  and Both edges are used. Value 0 and 1 are valid.
*
* Parameters:
*  edge: Edge Mode.
*
* Return:
*  Indication whether register is written and verified.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_SetEdgeMode(uint8 edge) `=ReentrantKeil($INSTANCE_NAME . "_SetEdgeMode")`
{
    uint8 result = `$INSTANCE_NAME`_FAIL;

    if (edge == `$INSTANCE_NAME`_EDGE_R_TO_D)
    {
        /* Recessive to Dominant is used for synchronization */
        `$INSTANCE_NAME`_CFG_REG.byte[0u] &= ~`$INSTANCE_NAME`_EDGE_MODE_MASK;

        /* Verify that bit is cleared */
        if ((`$INSTANCE_NAME`_CFG_REG.byte[0u] & `$INSTANCE_NAME`_EDGE_MODE_MASK) == 0u)
        {
            result = CYRET_SUCCESS;
        }                      
    }
    else
    {
        /* Both edges are to be used */
        `$INSTANCE_NAME`_CFG_REG.byte[0u] |= `$INSTANCE_NAME`_EDGE_MODE_MASK;

        /* Verify that bit is set */
        if ((`$INSTANCE_NAME`_CFG_REG.byte[0u] & `$INSTANCE_NAME`_EDGE_MODE_MASK) == `$INSTANCE_NAME`_EDGE_MODE_MASK)
        {
            result = CYRET_SUCCESS;
        }
    }

    return(result);
}


/*******************************************************************************
* FUNCTION NAME:   `$INSTANCE_NAME`_SetOpMode
********************************************************************************
*
* Summary:
*  This function sets Operation Mode. Operations Modes are Active of Listen
*  Only. Value 0 and 1 are valid.
*
* Parameters:
*  opMode: Operation Mode value.
*
* Return:
*  Indication whether register is written and verified.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_SetOpMode(uint8 opMode) `=ReentrantKeil($INSTANCE_NAME . "_SetOpMode")`
{
    uint8 result = `$INSTANCE_NAME`_FAIL;

    if (opMode == `$INSTANCE_NAME`_ACTIVE_MODE)
    {
        /* CAN in Active mode */
        `$INSTANCE_NAME`_CMD_REG.byte[0u] &= ~`$INSTANCE_NAME`_OPMODE_MASK;

        /* Verify that bit is cleared */
        if ((`$INSTANCE_NAME`_CMD_REG.byte[0u] & `$INSTANCE_NAME`_OPMODE_MASK) == 0u)
        {
            result = CYRET_SUCCESS;
        }                
    }
    else
    {
        /* CAN listen only */
        `$INSTANCE_NAME`_CMD_REG.byte[0u] |= `$INSTANCE_NAME`_OPMODE_MASK;

        /* Verify that bit is set */
        if ((`$INSTANCE_NAME`_CMD_REG.byte[0u] & `$INSTANCE_NAME`_OPMODE_MASK) == `$INSTANCE_NAME`_OPMODE_MASK)
        {
            result = CYRET_SUCCESS;
        }
    }

    return(result);
}


/*******************************************************************************
* FUNCTION NAME:   `$INSTANCE_NAME`_RXRegisterInit
********************************************************************************
*
* Summary:
*  This function writes only receive CAN registers.
*
* Parameters:
*  regAddr: Pointer to CAN receive register;
*  config:  Value that will be written in register.
*
* Return:
*  Indication whether register is written and verified.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_RXRegisterInit(reg32 *regAddr, uint32 config)
                                      `=ReentrantKeil($INSTANCE_NAME . "_RXRegisterInit")`
{
    uint8 result = `$INSTANCE_NAME`_OUT_OF_RANGE;    
    
    if ( (((uint32) regAddr & 0x0000FFFFu) >= ((uint32)`$INSTANCE_NAME`_RX_FIRST_REGISTER_PTR & 0x0000FFFFu)) &&
         ((((uint32) regAddr & 0x0000FFFFu)) <= ((uint32)`$INSTANCE_NAME`_RX_LAST_REGISTER_PTR & 0x0000FFFFu)) )
    {
        result = `$INSTANCE_NAME`_FAIL;
        
        if ((((uint32) regAddr & 0x0000FFFFu) % `$INSTANCE_NAME`_RX_CMD_REG_WIDTH) == 0u)
        {
            config |= `$INSTANCE_NAME`_RX_WPN_SET;
            
`$ISRDisable`
            /* Write defined RX CMD registers */
            CY_SET_REG32(regAddr, config);
            
`$ISREnable`
            /* Verify register */
            if ( (CY_GET_REG32(regAddr) & `$INSTANCE_NAME`_RX_READ_BACK_MASK) ==
                (config & `$INSTANCE_NAME`_RX_READ_BACK_MASK) )
            {
                result = CYRET_SUCCESS;
            }
        }
        /* All registers except RX CMD*/
        else
        {            
`$ISRDisable`
            /* Write defined CAN receive register */
            CY_SET_REG32(regAddr, config);
            
`$ISREnable`
            /* Verify register */
            if (CY_GET_REG32(regAddr) == config)
            {
                result = CYRET_SUCCESS;
            }
        }
    }
    
    return (result);
}


/*******************************************************************************
* FUNCTION NAME:   `$INSTANCE_NAME`_SetIrqMask
********************************************************************************
*
* Summary:
*  This function sets to enable/disable particular interrupt sources. Interrupt
*  Mask directly write to CAN Interrupt Enable register.
*
* Parameters:
*  mask: Interrupt enable/disable request. 1 bit per interrupt source.
*
* Return:
*  Indication whether register is written and verified.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_SetIrqMask(uint16 mask) `=ReentrantKeil($INSTANCE_NAME . "_SetIrqMask")`
{
    uint8 result = `$INSTANCE_NAME`_FAIL;

    /* Write byte 0 and 1 of `$INSTANCE_NAME`_INT_EN_REG register */
    CY_SET_REG16((reg16 *) &`$INSTANCE_NAME`_INT_EN_REG, mask);

    /* Verify `$INSTANCE_NAME`_INT_EN_REG register */
    if (CY_GET_REG16((reg16 *) &`$INSTANCE_NAME`_INT_EN_REG) == mask)
    {
        result = CYRET_SUCCESS;
    }

    return(result);
}


/*******************************************************************************
* FUNCTION NAME:   `$INSTANCE_NAME`_GetTXErrorflag
********************************************************************************
*
* Summary:
*  This function returns the bit that indicates if the number of TX errors
*  exceeds 0x60.
*
* Parameters:
*  None.
*
* Return:
*  Indication whether the number of TX errors exceeds 0x60.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_GetTXErrorFlag(void) `=ReentrantKeil($INSTANCE_NAME . "_GetTXErrorFlag")`
{
    /* Get the state of the transmit error flag */
    return(((`$INSTANCE_NAME`_ERR_SR_REG.byte[2u] & `$INSTANCE_NAME`_TX_ERROR_FLAG_MASK) ==
             `$INSTANCE_NAME`_TX_ERROR_FLAG_MASK) ? 1u : 0u);
}


/*******************************************************************************
* FUNCTION NAME:   `$INSTANCE_NAME`_GetRXErrorFlag
********************************************************************************
*
* Summary:
*  This function returns the bit that indicates if the number of RX errors
*  exceeds 0x60.
*
* Parameters:
*  None.
*
* Return:
*  Indication whether the number of TX errors exceeds 0x60.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_GetRXErrorFlag(void) `=ReentrantKeil($INSTANCE_NAME . "_GetRXErrorFlag")`
{
    /* Get the state of the receive error flag */
    return(((`$INSTANCE_NAME`_ERR_SR_REG.byte[2u] & `$INSTANCE_NAME`_RX_ERROR_FLAG_MASK) ==
             `$INSTANCE_NAME`_RX_ERROR_FLAG_MASK) ? 1u : 0u);
}


/*******************************************************************************
* FUNCTION NAME:   `$INSTANCE_NAME`_TXErrorCount
********************************************************************************
*
* Summary:
*  This function returns the number of Transmit Errors.
*
* Parameters:
*  None.
*
* Return:
*  Number of Transmit Errors.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_GetTXErrorCount(void) `=ReentrantKeil($INSTANCE_NAME . "_GetTXErrorCount")`
{
    /* Get the state of the transmit error count */
    return(`$INSTANCE_NAME`_ERR_SR_REG.byte[0u]);    /* bits 7-0 */
}


/*******************************************************************************
* FUNCTION NAME:   `$INSTANCE_NAME`_RXErrorCount
********************************************************************************
*
* Summary:
*  This function returns the number of Receive Errors.
*
* Parameters:
*  None.
*
* Return:
*  Number of Receive Errors.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_GetRXErrorCount(void) `=ReentrantKeil($INSTANCE_NAME . "_GetRXErrorCount")`
{
    /* Get the state of the receive error count */
    return(`$INSTANCE_NAME`_ERR_SR_REG.byte[1u]);    /* bits 15-8 */
}


/*******************************************************************************
* FUNCTION NAME:   `$INSTANCE_NAME`_GetErrorState
********************************************************************************
*
* Summary:
*  This function returns error status of CAN Component.
*
* Parameters:
*  None.
*
* Return:
*  Error status.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_GetErrorState(void) `=ReentrantKeil($INSTANCE_NAME . "_GetErrorState")`
{
    /* Get the error state of the receiver */
    return(`$INSTANCE_NAME`_ERR_SR_REG.byte[2u] & `$INSTANCE_NAME`_ERROR_STATE_MASK);
}


/*******************************************************************************
* FUNCTION NAME:   `$INSTANCE_NAME`_RxBufConfig
********************************************************************************
*
* Summary:
*  This function configures all receive registers for particular mailbox.
*
* Parameters:
*  rxConfig: Pointer to structure that contain all required values to configure
*  all receive registers for particular mailbox.
*
* Return:
*  Indication if particular configuration has been accepted or rejected.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_RxBufConfig(`$INSTANCE_NAME`_RX_CFG *rxConfig) `=ReentrantKeil($INSTANCE_NAME . "_RxBufConfig")`
{
    uint8 result = `$INSTANCE_NAME`_FAIL;

    /* Write RX CMD Register */
    CY_SET_REG32((reg32 *) &`$INSTANCE_NAME`_RX[rxConfig->rxmailbox].rxcmd, (rxConfig->rxcmd |
                 `$INSTANCE_NAME`_RX_WPN_SET));
    if ((CY_GET_REG32((reg32 *) & `$INSTANCE_NAME`_RX[rxConfig->rxmailbox].rxcmd) &
         `$INSTANCE_NAME`_RX_READ_BACK_MASK) == (rxConfig->rxcmd & `$INSTANCE_NAME`_RX_WPN_CLEAR))
    {
        /* Write RX AMR Register */
        CY_SET_REG32((reg32 *) &`$INSTANCE_NAME`_RX[rxConfig->rxmailbox].rxamr, rxConfig->rxamr);
        if (CY_GET_REG32((reg32 *) &`$INSTANCE_NAME`_RX[rxConfig->rxmailbox].rxamr) == rxConfig->rxamr)
        {
            /* Write RX ACR Register */
            CY_SET_REG32((reg32 *) &`$INSTANCE_NAME`_RX[rxConfig->rxmailbox].rxacr, rxConfig->rxacr);
            if (CY_GET_REG32((reg32 *) &`$INSTANCE_NAME`_RX[rxConfig->rxmailbox].rxacr) == rxConfig->rxacr)
            {
                /* Write RX AMRD Register */
                CY_SET_REG32((reg32 *) &`$INSTANCE_NAME`_RX[rxConfig->rxmailbox].rxamrd, 0xFFFFFFFFu);
                if (CY_GET_REG32((reg32 *) &`$INSTANCE_NAME`_RX[rxConfig->rxmailbox].rxamrd) == 0xFFFFFFFFu)
                {
                    /* Write RX ACRD Register */
                    CY_SET_REG32((reg32 *) &`$INSTANCE_NAME`_RX[rxConfig->rxmailbox].rxacrd, 0x00000000u);
                    if (CY_GET_REG32((reg32 *) &`$INSTANCE_NAME`_RX[rxConfig->rxmailbox].rxacrd) == 0x00000000u)
                    {
                            result = CYRET_SUCCESS;
                    }
                }
            }
        }
    }

    return(result);
}


/*******************************************************************************
* FUNCTION NAME:   `$INSTANCE_NAME`_TxBufConfig
********************************************************************************
*
* Summary:
*  This function configures all transmit registers for particular mailbox.
*  Mailbox number contains `$INSTANCE_NAME`_TX_CFG structure.
*
* Parameters:
*  txConfig: Pointer to structure that contain all required values to configure
*  all transmit registers for particular mailbox.
*
* Return:
*  Indication if particular configuration has been accepted or rejected.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_TxBufConfig(`$INSTANCE_NAME`_TX_CFG *txConfig) `=ReentrantKeil($INSTANCE_NAME . "_TxBufConfig")`
{
    uint8 result = `$INSTANCE_NAME`_FAIL;

    /* Write TX CMD Register */
    CY_SET_REG32((reg32 *) &`$INSTANCE_NAME`_TX[txConfig->txmailbox].txcmd, (txConfig->txcmd |
                 `$INSTANCE_NAME`_TX_WPN_SET));
    if ((CY_GET_REG32((reg32 *) & `$INSTANCE_NAME`_TX[txConfig->txmailbox].txcmd) &
        `$INSTANCE_NAME`_TX_READ_BACK_MASK) == (txConfig->txcmd & `$INSTANCE_NAME`_TX_WPN_CLEAR))
    {
        /* Write TX ID Register */
        CY_SET_REG32( (reg32 *) &`$INSTANCE_NAME`_TX[txConfig->txmailbox].txid, txConfig->txid);
        if (CY_GET_REG32( (reg32 *) &`$INSTANCE_NAME`_TX[txConfig->txmailbox].txid) == txConfig->txid)
        {
            result = CYRET_SUCCESS;
        }
    }

    return(result);
}


/* [] END OF FILE */
