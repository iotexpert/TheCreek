/*******************************************************************************
* File Name: `$INSTANCE_NAME`_INT.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides all Interrupt Service Routine (ISR) for the Voltage
*  Sequencer component.
*
* Note:
*  None
*
********************************************************************************
* Copyright 2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"
#include <device.h>

/* Macros to accelerate processing of 32-bit variables */
#define `$INSTANCE_NAME`_CLEAR_BIT(val, group, mask)    ((((uint8 *)&(val))[group]) &= ~(mask))
#define `$INSTANCE_NAME`_SET_BIT(val, group, mask)      ((((uint8 *)&(val))[group]) |= (mask))
#define `$INSTANCE_NAME`_TEST_BIT(val, group, mask)     ((((uint8 *)&(val))[group]) & (mask))
#define `$INSTANCE_NAME`_GET_GROUP(val, group)           (((uint8 *)&(val))[group])

#if(`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS <= 8u)    
    #define `$INSTANCE_NAME`_CMP_MASK(val, mask)        (((uint8)(val) & (uint8)(mask)) == (uint8)(mask))
#elif(`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS <= 16u)
    #define `$INSTANCE_NAME`_CMP_MASK(val, mask)        (((uint16)(val) & (uint16)(mask)) == (uint16)(mask))
#else /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 24u */
    #define `$INSTANCE_NAME`_CMP_MASK(val, mask)        (((uint32)(val) & (uint32)(mask)) == (uint32)(mask))
#endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS <= 8u */  

/* Define register lists based on the converter number */ 
#if(`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS <= 8u)    
    reg8 * enRegList[`$INSTANCE_NAME`_NUMBER_OF_GROUPS] = {`$INSTANCE_NAME`_EN_CTL1_PTR};
    reg8 * onRegList[`$INSTANCE_NAME`_NUMBER_OF_GROUPS] = {`$INSTANCE_NAME`_ON_CTL1_PTR};
    reg8 * pgRegList[`$INSTANCE_NAME`_NUMBER_OF_GROUPS] = {`$INSTANCE_NAME`_PGOOD_MON1_PTR};
#elif(`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS <= 16u)
    reg8 * enRegList[`$INSTANCE_NAME`_NUMBER_OF_GROUPS] = {
        `$INSTANCE_NAME`_EN_CTL1_PTR, `$INSTANCE_NAME`_EN_CTL2_PTR
    };
    reg8 * onRegList[`$INSTANCE_NAME`_NUMBER_OF_GROUPS] = {
        `$INSTANCE_NAME`_ON_CTL1_PTR, `$INSTANCE_NAME`_ON_CTL2_PTR
    };
    reg8 * pgRegList[`$INSTANCE_NAME`_NUMBER_OF_GROUPS] = {
        `$INSTANCE_NAME`_PGOOD_MON1_PTR, `$INSTANCE_NAME`_PGOOD_MON2_PTR
    };       
#elif(`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS <= 24u)
    reg8 * enRegList[`$INSTANCE_NAME`_NUMBER_OF_GROUPS] = {
        `$INSTANCE_NAME`_EN_CTL1_PTR, `$INSTANCE_NAME`_EN_CTL2_PTR,
        `$INSTANCE_NAME`_EN_CTL3_PTR
    };
    reg8 *onRegList[`$INSTANCE_NAME`_NUMBER_OF_GROUPS] = {
        `$INSTANCE_NAME`_ON_CTL1_PTR, `$INSTANCE_NAME`_ON_CTL2_PTR, 
        `$INSTANCE_NAME`_ON_CTL3_PTR
    };
    reg8 * pgRegList[`$INSTANCE_NAME`_NUMBER_OF_GROUPS] = {
        `$INSTANCE_NAME`_PGOOD_MON1_PTR, `$INSTANCE_NAME`_PGOOD_MON2_PTR,
        `$INSTANCE_NAME`_PGOOD_MON3_PTR
    };
#else /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS <= 32u */
    reg8 * enRegList[`$INSTANCE_NAME`_NUMBER_OF_GROUPS] = {
        `$INSTANCE_NAME`_EN_CTL1_PTR, `$INSTANCE_NAME`_EN_CTL2_PTR,
        `$INSTANCE_NAME`_EN_CTL3_PTR, `$INSTANCE_NAME`_EN_CTL4_PTR
    };
    reg8 * onRegList[`$INSTANCE_NAME`_NUMBER_OF_GROUPS] = {
        `$INSTANCE_NAME`_ON_CTL1_PTR, `$INSTANCE_NAME`_ON_CTL2_PTR, 
        `$INSTANCE_NAME`_ON_CTL3_PTR, `$INSTANCE_NAME`_ON_CTL4_PTR
    };
    reg8 * pgRegList[`$INSTANCE_NAME`_NUMBER_OF_GROUPS] = {
        `$INSTANCE_NAME`_PGOOD_MON1_PTR, `$INSTANCE_NAME`_PGOOD_MON2_PTR,
        `$INSTANCE_NAME`_PGOOD_MON3_PTR, `$INSTANCE_NAME`_PGOOD_MON4_PTR
    };    
#endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS <= 8u */  

/* Power Up Configuration Variables */
extern uint32   `$INSTANCE_NAME`_onCmdPrereqMask;
extern uint32   `$INSTANCE_NAME`_enPinPrereqMask;
extern uint32   `$INSTANCE_NAME`_pgoodPrereqList[`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS];
extern uint16   `$INSTANCE_NAME`_tonDelayList[`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS];
extern uint16   `$INSTANCE_NAME`_tonMaxDelayList[`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS];
extern uint16   `$INSTANCE_NAME`_sysStableTime;

/* Control Input Configuration Variables */
#if (`$INSTANCE_NAME`_NUMBER_OF_CTL_INPUTS > 0u)
    extern uint8            `$INSTANCE_NAME`_ctlPrereqList[`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS];
    extern uint8            `$INSTANCE_NAME`_ctlPolarity;
    extern uint8            `$INSTANCE_NAME`_ctlFaultReseqCfg[`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS];
    extern uint32           `$INSTANCE_NAME`_ctlGroupShutdownMask[`$INSTANCE_NAME`_NUMBER_OF_CTL_INPUTS];
    extern uint8            `$INSTANCE_NAME`_ctlShutdownMaskList[`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS];
    extern volatile uint8   `$INSTANCE_NAME`_ctlStatus;
#endif /* `$INSTANCE_NAME`_NUMBER_OF_CTL_INPUTS > 0u */

/* Power Down Configaration Variables */
extern uint16   `$INSTANCE_NAME`_toffDelayList[];
extern uint16   `$INSTANCE_NAME`_toffMaxDelayList[];

/* Fault Response and Resuquencing Configuration Variables */
extern uint16   `$INSTANCE_NAME`_globalReseqDelay;
extern uint32   `$INSTANCE_NAME`_pgoodShutdownMaskList[`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS];
extern uint32   `$INSTANCE_NAME`_pgoodGroupShutdownMask[`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS];
extern uint8    `$INSTANCE_NAME`_tonMaxFaultReseqCfg[`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS];
extern uint8    `$INSTANCE_NAME`_pgoodFaultReseqCfg[`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS];
extern uint8    `$INSTANCE_NAME`_ovFaultReseqCfg[`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS];
extern uint8    `$INSTANCE_NAME`_uvFaultReseqCfg[`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS];
extern uint8    `$INSTANCE_NAME`_ocFaultReseqCfg[`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS];
extern uint8    `$INSTANCE_NAME`_faultReseqSrcList[`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS];
/* Converter Resequence counters */
`$reseqCnt`

/* Used to determine the reason for entry to PEND_RESEQ state */
volatile uint32         `$INSTANCE_NAME`_faultCond;

/* Sequncer Command and Status Registers */
extern volatile uint32  `$INSTANCE_NAME`_forceOnCmdReg;
extern volatile uint32  `$INSTANCE_NAME`_forceOffCmdReg;
extern volatile uint32  `$INSTANCE_NAME`_powerOffMode;
extern volatile uint32  `$INSTANCE_NAME`_faultStatus;
extern volatile uint32  `$INSTANCE_NAME`_warnStatus;

/* Output Configuration Registers */
extern uint32   `$INSTANCE_NAME`_faultMask;
extern uint8    `$INSTANCE_NAME`_faultEnable;
extern uint32   `$INSTANCE_NAME`_warnMask;
#if (`$INSTANCE_NAME`_DISABLE_WARNINGS == 0u)
    extern uint8 `$INSTANCE_NAME`_warnEnable;
#endif /* `$INSTANCE_NAME`_DISABLE_WARNINGS == 0u */ 

#ifdef `$INSTANCE_NAME`_GENERATE_STATUS
    extern uint32   `$INSTANCE_NAME`_stsPgoodMaskList[`$INSTANCE_NAME`_NUMBER_OF_STS_OUTPUTS];
    extern uint32   `$INSTANCE_NAME`_stsPgoodPolarityList[`$INSTANCE_NAME`_NUMBER_OF_STS_OUTPUTS];
#endif /* `$INSTANCE_NAME`_GENERATE_STATUS */

/* Sequencer State Machine State List */
extern CYPDATA volatile uint8   `$INSTANCE_NAME`_state[`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS];
/* Indicates that all power converters are in ON state */
static CYDATA uint8 `$INSTANCE_NAME`_allConvertersOn;
/* TRESEQ_DELAY timer */
volatile uint16 reseqTimer = 0u;

/************************************************************************** 
* Sequencer State Machine Timer[]. Delay timers for each converter
* (usage depends on state).
**************************************************************************/
CYPDATA uint16 `$INSTANCE_NAME`_delayTimer[`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS] = {0u};
    
    
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SeqStateMachineIsr
********************************************************************************
*
* Summary:
*  The Sequencer State Machine ISR gets invoked every 250 us to handle the 
*  state machine transitions for every power converter.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
CY_ISR(`$INSTANCE_NAME`_SeqStateMachineIsr)
{     
    
    /* Enable pin sampling */
    uint8 enPinState;
    static uint8 enPinLast = 0u;
    uint8 enPinRisingEdge;
    uint8 enPinFallingEdge;
    uint32 forcedAllOn;    /* Forced ON Condition for all converters */
    uint32 forcedAllOff;   /* Forced OFF Condition for all converters */

    /*************************************************************************** 
    * Indicates that all power converters are in OFF state. Decrementing
    * each time a converter is in OFF state and is tested against 0s to 
    * generate sys_dn status output.
    ***************************************************************************/
    uint8 allConvertersOff = `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS;
    
    /***************************************************************************
    * Used to determine whether any rail is in TOFF_MAX or TOFF_DELAY.  
    * This holds resequencing rails in PEND_RESEQ until ALL resequencing rails
    * have transited through OFF and arrived at PEND_RESEQ.
    *
    * The "isPowerDown" variable is gathered while walking all rails
    * The "isPowerDownLast" is the result of the prior walk-all-rail tick
    * When entering TOFF_MAX or TOFF_DELAY, both the variables are set to ensure
    * any subseqent rails are aware.
    *******************************************************************************/
    static uint8 isPowerDownLast = 0u;
    static uint8 isPowerDown = 0u;
    
    CYDATA uint8  converterNum;      /* Converter Index */
    CYDATA uint8  volatile CYPDATA *pstate;   /* ptr to rail states (faster) */
    CYDATA uint16 CYPDATA *ptimer;   /* ptr to rail timers (faster) */
    CYDATA uint8 group = 0;
    CYDATA uint8 converterMask = 1u;
    CYDATA uint8 forcedOff = 0u;
    reg8 * enReg = 0u;   /* pointer to group EN register */
    reg8 * onReg = 0u;   /* pointer to group ON register */
    uint8  pgReg = 0u;   /* group pgood status register  */

    uint32 pgStatus;    /* Live pgood status for all converters */

    /* Determine power converter pgood status */ 
    #if(`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS <= 8u)
        pgStatus = `$INSTANCE_NAME`_PGOOD_MON1_REG;
    #elif(`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS <= 16u)
        pgStatus = (((uint16)`$INSTANCE_NAME`_PGOOD_MON2_REG) << 8u) |
                             `$INSTANCE_NAME`_PGOOD_MON1_REG;
    #elif(`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS <= 24u)
        pgStatus = (((uint32)`$INSTANCE_NAME`_PGOOD_MON3_REG) << 16u) |
                   (((uint16)`$INSTANCE_NAME`_PGOOD_MON2_REG) <<  8u) |
                             `$INSTANCE_NAME`_PGOOD_MON1_REG; 
    #else /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS <= 32u */
        pgStatus = (((uint32)`$INSTANCE_NAME`_PGOOD_MON4_REG) << 24u) |
                   (((uint32)`$INSTANCE_NAME`_PGOOD_MON3_REG) << 16u) |
                   (((uint16)`$INSTANCE_NAME`_PGOOD_MON2_REG) << 8u)  |
                             `$INSTANCE_NAME`_PGOOD_MON1_REG; 
    #endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS <= 8u */
    
    /* Forced ON condition via a command over a communications interface */
    forcedAllOn = `$INSTANCE_NAME`_onCmdPrereqMask & `$INSTANCE_NAME`_forceOnCmdReg;
    /* Clear Forced ON Register to prevent subsequent ONs without a host request */
    `$INSTANCE_NAME`_forceOnCmdReg = 0u;
    
    /* Determine enable signal toggling */
    enPinState = ((`$INSTANCE_NAME`_CTL_LO_REG & `$INSTANCE_NAME`_EN_PIN) != 0u) ? 1u : 0u;
    enPinRisingEdge = !enPinLast && enPinState;
    enPinFallingEdge = enPinLast && !enPinState;
    enPinLast = enPinState;   
    
    /* Forced ON condition by toggling the enable signal from low to high */
    if(enPinRisingEdge != 0u)
    {
        forcedAllOn |= `$INSTANCE_NAME`_enPinPrereqMask;
    }
    
    /*************************************************************************** 
    * Forced OFF condition via a command over a communications interface or by
    * de-asserting of the enable signal.
    *
    * Disable Fault ISR to make update of forceOffCmdReg and powerOffMode atomic
    ***************************************************************************/
    CyIntDisable(`$INSTANCE_NAME`_FAULT_ISR_NUMBER);
    if(enPinFallingEdge != 0u)
    {
        forcedAllOff = `$INSTANCE_NAME`_CONVERTER_MASK; /* Disable all converters */
        /* A soft shutdown is the only option when the Forced OFF is caused by 
        * de-asserting the enable pin.
        */
        `$INSTANCE_NAME`_powerOffMode = `$INSTANCE_NAME`_CONVERTER_MASK; 
    }
    else
    {
        forcedAllOff = `$INSTANCE_NAME`_forceOffCmdReg;
    }
    /* Clear Forced OFF Register to prevent subsequent OFFs without a host request */
    `$INSTANCE_NAME`_forceOffCmdReg = 0u;
    CyIntEnable(`$INSTANCE_NAME`_FAULT_ISR_NUMBER);
    /*************************************************************************** 
    * Indicates that all power converters are in ON state. Decrementing
    * each time a converter is in ON state and is tested against 0s to 
    * generate sys_up status output.
    ***************************************************************************/
    `$INSTANCE_NAME`_allConvertersOn = `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS;    
    /*************************************************************************** 
    *  The "isPowerDown" variable is gathered while walking all rails. It needs
    *  to be reseted each time before polling all rails.
    ***************************************************************************/
    isPowerDown = 0u;
    
    /* Power Converter State Machine Handling */    
    ptimer = &`$INSTANCE_NAME`_delayTimer[0];   /* pointer shortcut to this Timer */
    pstate = &`$INSTANCE_NAME`_state[0];        /* pointer shortcut to this State */
    for(converterNum = 0u; converterNum < `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS; converterNum++)
    {
        /*************************************************************************** 
        * Update all variables that need to be updated once per converter group
        * (8 converters). This makes state machine state transitions faster.
        ***************************************************************************/
        #if(`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS <= 8u)
            /* Converters  1 through  8 */
            if(converterNum == 0u)
            {
                enReg = `$INSTANCE_NAME`_EN_CTL1_PTR;
                onReg = `$INSTANCE_NAME`_ON_CTL1_PTR;
                pgReg = *`$INSTANCE_NAME`_PGOOD_MON1_PTR;
                group  = `$INSTANCE_NAME`_GROUP1;
                converterMask = 1u;
                forcedOff = `$INSTANCE_NAME`_GET_GROUP(forcedAllOff, `$INSTANCE_NAME`_GROUP1);
            }
            else
            {
                converterMask <<= 1u;
            }
        #elif(`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS <= 16u)
            switch(converterNum)
            {
                case 0u: /* Converters  1 through  8 */
                    enReg = `$INSTANCE_NAME`_EN_CTL1_PTR;
                    onReg = `$INSTANCE_NAME`_ON_CTL1_PTR;
                    pgReg = *`$INSTANCE_NAME`_PGOOD_MON1_PTR;
                    group  = `$INSTANCE_NAME`_GROUP1;
                    converterMask = 1u;
                    forcedOff = `$INSTANCE_NAME`_GET_GROUP(forcedAllOff, `$INSTANCE_NAME`_GROUP1);
                    break;
                case 8u: /* Converters  9 through 16 */
                    enReg = `$INSTANCE_NAME`_EN_CTL2_PTR;
                    onReg = `$INSTANCE_NAME`_ON_CTL2_PTR;
                    pgReg = *`$INSTANCE_NAME`_PGOOD_MON2_PTR;
                    group  = `$INSTANCE_NAME`_GROUP2;
                    converterMask = 1u;
                    forcedOff = `$INSTANCE_NAME`_GET_GROUP(forcedAllOff, `$INSTANCE_NAME`_GROUP2);
                    break;
                default:
                    converterMask <<= 1u;
                    break;
            }
        #elif(`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS <= 24u)
            switch(converterNum)
            {
                case 0u: /* Converters  1 through  8 */
                    enReg = `$INSTANCE_NAME`_EN_CTL1_PTR;
                    onReg = `$INSTANCE_NAME`_ON_CTL1_PTR;
                    pgReg = *`$INSTANCE_NAME`_PGOOD_MON1_PTR;
                    group  = `$INSTANCE_NAME`_GROUP1;
                    converterMask = 1u;
                    forcedOff = `$INSTANCE_NAME`_GET_GROUP(forcedAllOff, `$INSTANCE_NAME`_GROUP1);
                    break;
                case 8u: /* Converters  9 through 16 */
                    enReg = `$INSTANCE_NAME`_EN_CTL2_PTR;
                    onReg = `$INSTANCE_NAME`_ON_CTL2_PTR;
                    pgReg = *`$INSTANCE_NAME`_PGOOD_MON2_PTR;
                    group  = `$INSTANCE_NAME`_GROUP2;
                    converterMask = 1u;
                    forcedOff = `$INSTANCE_NAME`_GET_GROUP(forcedAllOff, `$INSTANCE_NAME`_GROUP2);
                    break;
                case 16u:
                     /* Converters 17 through 24 */
                    enReg = `$INSTANCE_NAME`_EN_CTL3_PTR;
                    onReg = `$INSTANCE_NAME`_ON_CTL3_PTR;
                    pgReg = *`$INSTANCE_NAME`_PGOOD_MON3_PTR;                
                    group  = `$INSTANCE_NAME`_GROUP3;
                    converterMask = 1u;
                    forcedOff = `$INSTANCE_NAME`_GET_GROUP(forcedAllOff, `$INSTANCE_NAME`_GROUP3);
                    break;
                default:
                    converterMask <<= 1u;
                    break;
            }    
        #else /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS <= 32u */
            switch(converterNum)
            {
                case 0u: /* Converters  1 through  8 */
                    enReg = `$INSTANCE_NAME`_EN_CTL1_PTR;
                    onReg = `$INSTANCE_NAME`_ON_CTL1_PTR;
                    pgReg = *`$INSTANCE_NAME`_PGOOD_MON1_PTR;
                    group  = `$INSTANCE_NAME`_GROUP1;
                    converterMask = 1u;
                    forcedOff = `$INSTANCE_NAME`_GET_GROUP(forcedAllOff, `$INSTANCE_NAME`_GROUP1);
                    break;
                case 8u: /* Converters  9 through 16 */
                    enReg = `$INSTANCE_NAME`_EN_CTL2_PTR;
                    onReg = `$INSTANCE_NAME`_ON_CTL2_PTR;
                    pgReg = *`$INSTANCE_NAME`_PGOOD_MON2_PTR;
                    group  = `$INSTANCE_NAME`_GROUP2;
                    converterMask = 1u;
                    forcedOff = `$INSTANCE_NAME`_GET_GROUP(forcedAllOff, `$INSTANCE_NAME`_GROUP2);
                    break;
                case 16u:
                     /* Converters 17 through 24 */
                    enReg = `$INSTANCE_NAME`_EN_CTL3_PTR;
                    onReg = `$INSTANCE_NAME`_ON_CTL3_PTR;
                    pgReg = *`$INSTANCE_NAME`_PGOOD_MON3_PTR;
                    group  = `$INSTANCE_NAME`_GROUP3;
                    converterMask = 1u;
                    forcedOff = `$INSTANCE_NAME`_GET_GROUP(forcedAllOff, `$INSTANCE_NAME`_GROUP3);
                    break;
                case 24u:
                    /* Converters 25 through 32 */
                    enReg = `$INSTANCE_NAME`_EN_CTL4_PTR;
                    onReg = `$INSTANCE_NAME`_ON_CTL4_PTR;
                    pgReg = *`$INSTANCE_NAME`_PGOOD_MON4_PTR;
                    group  = `$INSTANCE_NAME`_GROUP4;
                    converterMask = 1u;
                    forcedOff = `$INSTANCE_NAME`_GET_GROUP(forcedAllOff, `$INSTANCE_NAME`_GROUP4);
                    break;
                default:
                    converterMask <<= 1u;
                    break;
            }
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS <= 8u */
        
        switch(*pstate)
        {
        case `$INSTANCE_NAME`_OFF:

            if(`$INSTANCE_NAME`_TEST_BIT(forcedAllOn, group, converterMask) != 0u)
            {
                *pstate = `$INSTANCE_NAME`_PEND_ON;
                `$INSTANCE_NAME`_reseqCnt[converterNum] = `$INSTANCE_NAME`_INIT_RESEQ_CNT;
            }
            else
            {
                allConvertersOff--;
            }
            break;
                
        case `$INSTANCE_NAME`_PEND_ON:
            
            if((forcedOff & converterMask) == 0u)
            {                    
                #if(`$INSTANCE_NAME`_NUMBER_OF_CTL_INPUTS > 0u)
                    if(((`$INSTANCE_NAME`_CTL_LO_REG  ^ `$INSTANCE_NAME`_ctlPolarity) & \
                       `$INSTANCE_NAME`_ctlPrereqList[converterNum]) != 0u)
                    {
                        break;
                    }
                #endif
                                   
                if(`$INSTANCE_NAME`_CMP_MASK(pgStatus, `$INSTANCE_NAME`_pgoodPrereqList[converterNum]))
                {
                   *pstate = `$INSTANCE_NAME`_TON_DELAY;
                }
            }
            else
            {
                /* Load the TOFF_MAX_WARN_LIMIT period */
                *ptimer = `$INSTANCE_NAME`_toffMaxDelayList[converterNum];
                *pstate = `$INSTANCE_NAME`_TOFF_MAX_WARN_LIMIT;
            }
            break;
                
        case `$INSTANCE_NAME`_TON_DELAY: 

            if((forcedOff & converterMask) == 0u)
            {
                if(*ptimer < `$INSTANCE_NAME`_tonDelayList[converterNum])
                {
                    ++*ptimer;
                }
                else
                {
                    /* Enable the associated power converter */
                    *enReg |= converterMask;
                    /* Load the TON_MAX_DELAY period */
                    *ptimer = `$INSTANCE_NAME`_tonMaxDelayList[converterNum];
                    *pstate = `$INSTANCE_NAME`_TON_MAX;
                    /******************************************************
                    * If the TrimMargin component is used in the design,
                    * this is the appropriate time to change the PWM duty
                    * cycle from the pre-run setting to the run setting.
                    * Add your custom code between the following #START
                    * and #END tags
                    *******************************************************/
                    /* `#START TRIM_CFG_RUN_SECTION` */

                    /* `#END`  */
                }
            }
            else
            {
                /* Load the TOFF_MAX_WARN_LIMIT period */
                *ptimer = `$INSTANCE_NAME`_toffMaxDelayList[converterNum];
                *pstate = `$INSTANCE_NAME`_TOFF_MAX_WARN_LIMIT;
            }                    
            break;
            
        case `$INSTANCE_NAME`_TON_MAX:
            
            if((forcedOff & converterMask) == 0u)
            {
                if((pgReg & converterMask) != 0u)
                {
                    *pstate = `$INSTANCE_NAME`_ON;
                    /* Load the TOFF_MAX_WARN_LIMIT period */
                    *ptimer = `$INSTANCE_NAME`_toffMaxDelayList[converterNum];
                }
                else
                {
                    if(*ptimer != 0)
                    {
                        --*ptimer;
                    }
                    else /* Fault condition */
                    {   
                        uint32 faultGroup;
                        CYBIT offMode;
                        CYDATA uint8 reseqCt;
                        uint32 offBits = 0u;
                        CYDATA uint8 sIdx;
                        /* Disable the associated power converter */
                        *enReg &= ~converterMask;
                        /* Update fault status register */
                        `$INSTANCE_NAME`_SET_BIT(`$INSTANCE_NAME`_faultStatus, group, converterMask);
                        /* Establish which converters should go off */ 
                        faultGroup = `$INSTANCE_NAME`_pgoodGroupShutdownMask[converterNum];
                        /* Set fault flag that will indicate the reason of 
                        *  entering PEND_RESEQ for the associated converter.
                        */
                        `$INSTANCE_NAME`_faultCond |= faultGroup;
                        /* Request for power down fault group */
                        `$INSTANCE_NAME`_forceOffCmdReg |= faultGroup;
                        /* Update Resequence counter */
                        reseqCt = `$INSTANCE_NAME`_tonMaxFaultReseqCfg[converterNum];
                        /* Get fault group shutdown mode */
                        offMode = ((reseqCt & `$INSTANCE_NAME`_SHUTDOWN_MODE) == 0u) ? 0u : 1u;
                        reseqCt &= `$INSTANCE_NAME`_RESEQ_CNT; /* Isolate count */                            
                        /* Process fault group */                    
                        if(offMode == 0u)
                        {   
                            uint8 groupNum;
                            offBits = faultGroup;
                            for(groupNum = 0u; groupNum < `$INSTANCE_NAME`_NUMBER_OF_GROUPS; groupNum++)
                            {
                                /* Disable all converters configured for immediate off mode */                
                                *enRegList[groupNum] &= ~((uint8)(offBits));
                                offBits >>= `$INSTANCE_NAME`_GROUP_SIZE;
                            }
                        }
                        else
                        {
                            `$INSTANCE_NAME`_powerOffMode |= faultGroup; 
                        }                           
                        /* Update Resequence counter for fault group */
                        for(sIdx = 0u; (sIdx < `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS) && 
                           (faultGroup != 0ul); sIdx++)
                        {
                            if((faultGroup & 0x01ul) != 0u)
                            {
                                if(reseqCt < `$INSTANCE_NAME`_reseqCnt[sIdx])
                                {
                                    `$INSTANCE_NAME`_reseqCnt[sIdx] = reseqCt;
                                }
                                else
                                {
                                    /* The counter needs to be updated only if the
                                    *  new fault has a lower re-seq value than the
                                    *  previous fault.
                                    */
                                }
                            }
                            else
                            {
                                /* The associated converter should not go off */
                            }
                            faultGroup >>= 1u;
                        }
                    }
                }
            }
            else
            {
                if(`$INSTANCE_NAME`_TEST_BIT(`$INSTANCE_NAME`_powerOffMode, group, converterMask) == 0u)
                {
                    /* An immediate shutdown */
                    *enReg &= ~converterMask; /* Disable the associated power converter */
                    *pstate = `$INSTANCE_NAME`_TOFF_MAX_WARN_LIMIT;
                    *ptimer = `$INSTANCE_NAME`_toffMaxDelayList[converterNum];
                    
                    /******************************************************
                    * If the PowerMonitor or the VoltageFaultDetector 
                    * components are used to generate the pgood[x] inputs
                    * to the VoltageSequencer, this is the appropriate time
                    * to lower the UV threshold for pgood detection from the
                    * power on detect threshold to the normal operating UV 
                    * fault threshold to avoid generating an immediate
                    * fault.
                    * Add your custom code between the following #START
                    * and #END tags.
                    *******************************************************/
                    /* `#START THRESHOLD_CFG_SECTION` */

                    /* `#END`  */                        
                }
                else
                {
                    /* A soft shutdown */
                    *pstate = `$INSTANCE_NAME`_TOFF_DELAY;
                    /* Load the TOFF_DELAY period */
                    *ptimer = `$INSTANCE_NAME`_toffDelayList[converterNum];
                }
            }
            break;
                
         case `$INSTANCE_NAME`_ON:
          
            if((forcedOff & converterMask) == 0u)
            {
                if((*onReg & converterMask) != 0u)
                {
                    `$INSTANCE_NAME`_allConvertersOn--;
                }
                else
                {
                    *onReg |= converterMask;
                    `$INSTANCE_NAME`_allConvertersOn--;
                    
                    #if(`$INSTANCE_NAME`_NUMBER_OF_CTL_INPUTS > 0u)
                        /* Enable ctl fault monitoring for the associated power converter */
                        `$INSTANCE_NAME`_CTL_LO_MASK_REG |= ~`$INSTANCE_NAME`_ctlPolarity & \
                                                             `$INSTANCE_NAME`_ctlShutdownMaskList[converterNum];
                        `$INSTANCE_NAME`_CTL_HI_MASK_REG |=  `$INSTANCE_NAME`_ctlPolarity & \
                                                             `$INSTANCE_NAME`_ctlShutdownMaskList[converterNum];
                    #endif /* `$INSTANCE_NAME`_NUMBER_OF_CTL_INPUTS > 0u */
                }
            }
            else
            {
                #if(`$INSTANCE_NAME`_NUMBER_OF_CTL_INPUTS > 0u)
                    /* Disable ctl fault monitoring for the associated power converter */
                    `$INSTANCE_NAME`_CTL_LO_MASK_REG &= ~(~`$INSTANCE_NAME`_ctlPolarity & \
                                                           `$INSTANCE_NAME`_ctlShutdownMaskList[converterNum]);
                    `$INSTANCE_NAME`_CTL_HI_MASK_REG &=  ~(`$INSTANCE_NAME`_ctlPolarity & \
                                                           `$INSTANCE_NAME`_ctlShutdownMaskList[converterNum]);
                #endif /* `$INSTANCE_NAME`_NUMBER_OF_CTL_INPUTS > 0u */
                
                if(`$INSTANCE_NAME`_TEST_BIT(`$INSTANCE_NAME`_powerOffMode, group, converterMask) == 0u)
                {
                    /* An immediate shutdown */
                    *onReg &= ~converterMask;
                    *enReg &= ~converterMask; /* Disable the associated power converter */
                    *pstate = `$INSTANCE_NAME`_TOFF_MAX_WARN_LIMIT;
                }
                else
                {
                    /* A soft shutdown */
                    *onReg &= ~converterMask;
                    *pstate = `$INSTANCE_NAME`_TOFF_DELAY;
                    /* Load the TOFF_DELAY period */
                    *ptimer = `$INSTANCE_NAME`_toffDelayList[converterNum];
                }
            }
            break;
                
        case `$INSTANCE_NAME`_TOFF_DELAY:

            isPowerDown = 1u;
            
            if(((forcedOff & converterMask) == 0u) && (*ptimer != 0))
            {
                --*ptimer;
            }
            else
            {
                *enReg &= ~converterMask; /* Disable the associated power converter */
                /* Load the TOFF_MAX_WARN_LIMIT timer */
                *ptimer = `$INSTANCE_NAME`_toffMaxDelayList[converterNum];
                *pstate = `$INSTANCE_NAME`_TOFF_MAX_WARN_LIMIT;                    
            }
            break;
                
            case `$INSTANCE_NAME`_TOFF_MAX_WARN_LIMIT:

            /***************************************************************
            * If the PowerMonitor or the VoltageFaultDetector components 
            * are used to generate the pgood[x] inputs, this is the 
            * appropriate time to lower the UV threshold to be 12.5% of Vnom
            * to enable detection that the power converter has truly turned
            * off.
            * Add your custom code between the following #START
            * and #END tags
            *******************************************************/
            /* `#START THRESHOLD_LOW_SECTION` */

            /* `#END`  */ 
            isPowerDown = 1u;
            
            /* Check if the associated converter is shutdown */
            if((pgReg & converterMask) == 0u)
            {
                *pstate = `$INSTANCE_NAME`_PEND_RESEQ;
                *ptimer = 0u; /* Reset timer */
            }
            else
            {                    
                if(*ptimer != 0u)
                {
                    --*ptimer;
                }
                else
                {
                    *pstate = `$INSTANCE_NAME`_PEND_RESEQ;
                    `$INSTANCE_NAME`_SET_BIT(`$INSTANCE_NAME`_warnStatus, group, converterMask);
                }
            }
            break;
            
        case `$INSTANCE_NAME`_PEND_RESEQ:
            
            if((`$INSTANCE_NAME`_reseqCnt[converterNum] == 0u) ||
              (`$INSTANCE_NAME`_TEST_BIT(`$INSTANCE_NAME`_faultCond, group, converterMask) == 0u))
            {
                *pstate = `$INSTANCE_NAME`_OFF;
            }
            else
            {
                /* Check if any state machine is powering down */
                if((isPowerDownLast == 0u) && (isPowerDown == 0u))
                {
                    *pstate = `$INSTANCE_NAME`_TRESEQ_DELAY;
                    reseqTimer = `$INSTANCE_NAME`_globalReseqDelay;
                    `$INSTANCE_NAME`_CLEAR_BIT(`$INSTANCE_NAME`_faultCond, group, converterMask);
                    if(`$INSTANCE_NAME`_reseqCnt[converterNum] != `$INSTANCE_NAME`_INFINITE_RESEQUENCING)
                    {
                        `$INSTANCE_NAME`_reseqCnt[converterNum]--;
                    }
                    else
                    {
                        /* Ininite re-sequencing */
                    }
                }
                else
                {
                    /* No state machine can make transition to TRESEQ_DELAY
                    *  if any other state machine is currently still in the 
                    *  process of powering down.
                    */
                }
            }
            break;
                
        case `$INSTANCE_NAME`_TRESEQ_DELAY:
            
            if((forcedOff & converterMask) != 0u)
            {
                /* Load the TOFF_MAX_WARN_LIMIT timer */
                *ptimer = `$INSTANCE_NAME`_toffMaxDelayList[converterNum];
                *pstate = `$INSTANCE_NAME`_TOFF_MAX_WARN_LIMIT;                  
            }
            else
            {
                if(reseqTimer == 0u)
                {
                    /*******************************************************
                    * If the TrimMargin component is used in the design, 
                    * this is the appropriate time to to change the PWM duty
                    * cycle back to the pre-run setting from the run setting 
                    * are used to generate the pgood[x] inputs, this is the
                    * appropriate time to lower the UV threshold to be 12.5% of Vnom
                    * to enable detection that the power converter has truly turned
                    * off.
                    * Add your custom code between the following #START
                    * and #END tags
                    *******************************************************/
                    /* `#START TRIM_CFG_PRERUN_SECTION` */

                    /* `#END`  */
                    *pstate = `$INSTANCE_NAME`_PEND_ON;
                }
                else
                {
                    /* Wait until RESEQ_DELAY expires */
                }
            }                                
        default:
            break;
        }
        pstate++;   /* Go to next converter */
        ptimer++;   /* Set primer ptr to associated timer */
    }
    
    /* Updated system powering down status */
    isPowerDownLast = isPowerDown;  


    /***************************************************************************
    * Calculate the value for each of the programmable status output bits.
    * A status output bit can be asserted for any combination of PGood input
    * values.  Specified via a mask and polarity
    ***************************************************************************/
    #ifdef `$INSTANCE_NAME`_GENERATE_STATUS
    {
        uint8 stsNum;
        uint8 sts = 0;
        for(stsNum = 0; stsNum < `$INSTANCE_NAME`_NUMBER_OF_STS_OUTPUTS; stsNum++)
        {         
            if(((pgStatus ^ `$INSTANCE_NAME`_stsPgoodPolarityList[stsNum]) &
                            `$INSTANCE_NAME`_stsPgoodMaskList[stsNum]) == 0u)
            {
                sts |= (1u << stsNum);
            }
        }
        `$INSTANCE_NAME`_STS_OUT_REG = ~(sts ^ `$INSTANCE_NAME`_INIT_STS_POLARITY);
    }
    #endif /* `$INSTANCE_NAME`_GENERATE_STATUS */

    /* Generation of sys_up and sys_stable outputs */
    
    #ifndef `$INSTANCE_NAME`_VSeq_SystemStsReg__REMOVED
    
        if(`$INSTANCE_NAME`_allConvertersOn == 0u)
        {
            `$INSTANCE_NAME`_SYSTEM_STATUS_REG |= `$INSTANCE_NAME`_SYS_UP_MASK;
        }
        else
        {
            /* Deassert sys_up and sys_stable */
            `$INSTANCE_NAME`_SYSTEM_STATUS_REG &= ~`$INSTANCE_NAME`_SYS_UP_MASK;
            `$INSTANCE_NAME`_SYSTEM_STATUS_REG &= ~`$INSTANCE_NAME`_SYS_STABLE_MASK;  
        }
    
        /* Generation of sys_dn output */
        if(allConvertersOff == 0u)
        {
            `$INSTANCE_NAME`_SYSTEM_STATUS_REG |= `$INSTANCE_NAME`_SYS_DN_MASK;
        }
        else
        {
            `$INSTANCE_NAME`_SYSTEM_STATUS_REG &= ~`$INSTANCE_NAME`_SYS_DN_MASK;
        }

        /* Generation of warn output */    
        #if (`$INSTANCE_NAME`_DISABLE_WARNINGS == 0u)
            if(`$INSTANCE_NAME`_warnEnable != 0u)
            {
                if((`$INSTANCE_NAME`_warnStatus & `$INSTANCE_NAME`_warnMask) != 0ul)
                {
                    `$INSTANCE_NAME`_SYSTEM_STATUS_REG |= `$INSTANCE_NAME`_WARN_MASK;
                }
                else
                {
                    /* De-assert warn output signal */
                    `$INSTANCE_NAME`_SYSTEM_STATUS_REG &= ~`$INSTANCE_NAME`_WARN_MASK;
                }
            }
            else
            {
                /* Generation of warn signal is disabled */
            }
        #endif /* `$INSTANCE_NAME`_DISABLE_WARNINGS == 0u */

        /* Generation of fault output */    
        if(`$INSTANCE_NAME`_faultEnable != 0u)
        {
            if((`$INSTANCE_NAME`_faultStatus & `$INSTANCE_NAME`_faultMask) != 0ul)
            {
                `$INSTANCE_NAME`_SYSTEM_STATUS_REG |= `$INSTANCE_NAME`_FAULT_MASK;
            }
            else
            {
                /* De-assert warn output signal */
                `$INSTANCE_NAME`_SYSTEM_STATUS_REG &= ~`$INSTANCE_NAME`_FAULT_MASK;
            }
        }
        else
        {
            /* Generation of warn signal is disabled */
        }
    
    #endif /* `$INSTANCE_NAME`_VSeq_SystemStsReg__REMOVED */

}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SysStableTimerIsr
********************************************************************************
*
* Summary:
*  The system stable timer is a 16-bit timer with an 8 ms tick time. It is used 
*  to measure how long all power converters in the system have been in the ON 
*  state. This is useful to distinguish between a stable power system and a 
*  system that is in the process of recovering from re-sequencing in response to
*  a fault condition.
*
* Parameters:
*  None
*
* Return:
*  None
*
*
*******************************************************************************/
CY_ISR(`$INSTANCE_NAME`_SysStableTimerIsr)
{      
    uint8 converterNum;
    static uint16 stableTimer = 0u;
    static uint8 sysStable = 0u;     /* Sys stable status */   
    static uint8 sysStateLast = 0u;  /* Store the last system state */ 
    uint8 sysState;                  /* Live system state */ 
    
    /* `$INSTANCE_NAME`_allConvertersOn is indicating that system is 
    *  in ON state. Implemented as decrement counter. Therefore its value
    *  zero when the system is ON.
    */
    sysState = (`$INSTANCE_NAME`_allConvertersOn == 0u) ? 1u : 0u;
    /* Reset sysStable status if not all converters are in ON state */
    if(sysState == 0u)
    {
        sysStable = 0u;
    }     
    /* Reload System Stable Timer if system goes to ON state */    
    if((sysStateLast == 0u) && (sysState != 0u))
    {
        stableTimer = `$INSTANCE_NAME`_sysStableTime; 
    }
    
    if(sysStable == 0u)  /* System has not been up for the specified time */
    {
        if((sysState != 0u) && (stableTimer != 0))
        {
            stableTimer--;
        }
        else
        {
            if(sysState != 0u) /* Sys is ON and system stable time has expired */
            {
                sysStable = 1u;  /* Declare that system is stable */
                
                #ifndef `$INSTANCE_NAME`_VSeq_SystemStsReg__REMOVED
                    `$INSTANCE_NAME`_SYSTEM_STATUS_REG |= `$INSTANCE_NAME`_SYS_STABLE_MASK;
                #endif
                
                for(converterNum = 0u; converterNum < `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS; converterNum++)
                {
                    /* Reset re-sequnce counters */
                    `$INSTANCE_NAME`_reseqCnt[converterNum] = `$INSTANCE_NAME`_INIT_RESEQ_CNT;
                }
            }
            else
            {
                /* System is not in ON state */
            }
        }
    }    
    else
    {
        /* System has been already declared stable */
    }
    sysStateLast = sysState;
    
    /********************************************************** 
    * RESEQ_DELAY timer is set in PEND_RESEQ state and used in 
    * TRESEQ_DELAY state of Sequencer State machine.
    **********************************************************/      
    if(reseqTimer != 0u)
    {
        reseqTimer--;
    }
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_FaultHandlerIsr
********************************************************************************
*
* Summary:
*  The Fault Handler ISR immediately disables the en[x] output for the
*  associated power converter. It also immediately disables the en[x] output
*  for any associated Fault Slaves that are configured for immediate shutdown.
*  It sets a flag indicating a fault condition occurred that will be recognized
*  by the Sequencer State Machine ISR when the next 250 us tick timer interrupt
*  occurs so that it can manage appropriate state transitions.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
CY_ISR(`$INSTANCE_NAME`_FaultHandlerIsr)
{       
    CYDATA uint8 mIdx;         /* Fault master index */
    CYDATA uint8 sIdx;         /* Fault slave index */
    /* Divide converters in groups to scan through */
    /* Group offset is used to point to first converter in group */
    uint8 groupOffset; 
    CYDATA uint8 groupNum;     /* The number of groups */
    reg8 * enReg;              /* pointer to group en[x] registers */
    /* Fault group that should go off with fault converter */
    uint32 faultGroup;
    uint32 offBits = 0u;
    CYDATA uint8 fault;
    CYDATA uint8 reseqCt = 0u;
    CYBIT offMode = 0u;
    
    /* Check if the fault has been generated due to de-assertion of pgood */
    groupOffset = 0u;
    for(groupNum = 0u; groupNum < `$INSTANCE_NAME`_NUMBER_OF_GROUPS; groupNum++)
    {
        uint32 faultMaster;
        uint8 pgReg;        /* group pgood status */
        enReg = enRegList[groupNum];
        pgReg = *pgRegList[groupNum];
        /* For fault converters en[x] is asserted and pgood status faulted */
        fault = ~(pgReg) & *enReg;
        if(fault != 0u)
        {
            /* Disable fault master(s) */
            *enReg &= ~fault;
            /* Find fault slaves */
            mIdx = groupOffset;
            while(fault != 0u)
            {
                if((fault & 0x01) != 0u) /* If this pgood status faulted */
                {
                    uint8 faultSrc = 0u;
                    faultMaster = (1u << mIdx);
                    `$INSTANCE_NAME`_faultStatus |= faultMaster;  /* Set fault status */

                    /* At this point we have indication that fault has occurred due to 
                    *  de-assertion of pgood[x] signal and fault converter mask. So this is
                    *  appropriate place to check which fault re-suquence sources are 
                    *  enabled for this power converter. In case if the power converter
                    *  has OV, UV or OC fault source enabled this is appropriate place 
                    *  to call associated APIs of Power Monitor or Fault Detector 
                    *  component.
                    *
                    *   Possible fault sources are:
                    *   - `$INSTANCE_NAME`_PGOOD_FAULT_SRC   0x0
                    *   - `$INSTANCE_NAME`_OV_FAULT_SRC      0x1
                    *   - `$INSTANCE_NAME`_UV_FAULT_SRC      0x2
                    *   - `$INSTANCE_NAME`_OC_FAULT_SRC      0x4
                    */
                    if(`$INSTANCE_NAME`_faultReseqSrcList[mIdx] != `$INSTANCE_NAME`_PGOOD_FAULT_SRC)
                    {                        
                        /***************************************************************************
                        * Add your custom ISR code between the following #START and #END tags 
                        * to determine the fault source.
                        ***************************************************************************/
                        /* `#START FAULT_HANDLER_ISR` */
                        
                        /* `#END`  */
                    }
                    else
                    {
                        faultSrc = `$INSTANCE_NAME`_PGOOD_FAULT_SRC;
                    }
                    /* Get Reseq Count for Master Fault */
                    switch(faultSrc)
                    {
                        case `$INSTANCE_NAME`_OV_FAULT_SRC:
                            reseqCt = `$INSTANCE_NAME`_ovFaultReseqCfg[mIdx];
                            break;                            
                        case `$INSTANCE_NAME`_UV_FAULT_SRC:
                            reseqCt = `$INSTANCE_NAME`_uvFaultReseqCfg[mIdx];
                            break;
                        case `$INSTANCE_NAME`_OC_FAULT_SRC:
                            reseqCt = `$INSTANCE_NAME`_ocFaultReseqCfg[mIdx];
                            break;
                        default:
                            reseqCt = `$INSTANCE_NAME`_pgoodFaultReseqCfg[mIdx];
                            break;
                    }
                    offMode = ((reseqCt & `$INSTANCE_NAME`_SHUTDOWN_MODE) == 0u) ? 0u : 1u;
                    reseqCt &= `$INSTANCE_NAME`_RESEQ_CNT; /* Isolate count */
                    
                    /* Mask fault groups and force Sequencer State machine to
                    *  make corresponding state transition.
                    */
                    faultGroup = `$INSTANCE_NAME`_pgoodGroupShutdownMask[mIdx];
                    `$INSTANCE_NAME`_faultCond      |= faultGroup;
                    `$INSTANCE_NAME`_forceOffCmdReg |= faultGroup;                    
                    
                    /* Process shutdown mode */                    
                    if(offMode == `$INSTANCE_NAME`_IMMEDIATE_OFF)
                    {
                        offBits = faultGroup;
                        for(groupNum = 0u; groupNum < `$INSTANCE_NAME`_NUMBER_OF_GROUPS; groupNum++)
                        {
                            /* Disable all converters configured for immediate off mode */                
                            *enRegList[groupNum] &= ~((uint8)(offBits));
                            offBits >>= `$INSTANCE_NAME`_GROUP_SIZE;
                        }
                    }
                    else
                    {
                        `$INSTANCE_NAME`_powerOffMode |= faultGroup; 
                    }
                    /* Update Resequence counter for fault group */
                    for(sIdx = 0u; (sIdx < `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS) && (faultGroup != 0ul); sIdx++)
                    {
                        if((faultGroup & 0x01ul) != 0u)
                        {
                            if(reseqCt < `$INSTANCE_NAME`_reseqCnt[sIdx])
                            {
                                `$INSTANCE_NAME`_reseqCnt[sIdx] = reseqCt;
                            }
                            else
                            {
                                /* The counter needs to be updated only if the
                                *  new fault has a lower re-seq value than the
                                *  previous fault.
                                */
                            }
                        }
                        else
                        {
                            /* The associated converter should not be off */
                        }
                        faultGroup >>= 1u;
                    }
                }
                fault >>= 1u;
                mIdx++;
            }
            groupOffset += `$INSTANCE_NAME`_GROUP_SIZE;
        }
    }
    
    #if(`$INSTANCE_NAME`_NUMBER_OF_CTL_INPUTS > 0u)
    /* Handling fault condition due to de-assertion of ctl[x] inputs */
    {
        CYDATA uint8 ctlNum;
        CYDATA uint8 ctlMask;
        ctlMask = (`$INSTANCE_NAME`_CTL_LO_MASK_REG | `$INSTANCE_NAME`_CTL_HI_MASK_REG) & `$INSTANCE_NAME`_CTL_MASK;
        fault = (`$INSTANCE_NAME`_CTL_LO_REG ^ `$INSTANCE_NAME`_ctlPolarity) & ctlMask;
        /* Provide ctlStatus to main code */
        `$INSTANCE_NAME`_ctlStatus = fault;
        ctlMask = 1u;
        for(ctlNum = 0u; (ctlNum < `$INSTANCE_NAME`_NUMBER_OF_CTL_INPUTS) && (fault != 0u); ctlNum++)
        {            
            if((fault & 0x01) != 0u)    /* If this Seq Ctl Pin faulted */
            {
                uint32 faultSlave = 1u;
                faultGroup = `$INSTANCE_NAME`_ctlGroupShutdownMask[ctlNum];
                `$INSTANCE_NAME`_faultCond      |= faultGroup;
                `$INSTANCE_NAME`_forceOffCmdReg |= faultGroup;
                for(sIdx = 0u; (sIdx < `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS) && (faultGroup != 0ul); sIdx++)
                {
                    if((faultGroup & 0x01) != 0ul)
                    {
                        reseqCt = `$INSTANCE_NAME`_ctlFaultReseqCfg[sIdx];
                        offMode = ((reseqCt & `$INSTANCE_NAME`_SHUTDOWN_MODE) == 0u) ? 0u : 1u;
                        reseqCt &= `$INSTANCE_NAME`_RESEQ_CNT; /* Isolate count */
                    
                        /* Process shutdown mode */
                        if(offMode == `$INSTANCE_NAME`_IMMEDIATE_OFF)
                        {
                            offBits |= faultSlave;  /* Immediate shutdown converter mask */
                        }
                        else /* Soft off */
                        {
                            `$INSTANCE_NAME`_powerOffMode |= faultSlave; 
                        }
                        /* Update Resequence counters */
                        if(reseqCt < `$INSTANCE_NAME`_reseqCnt[sIdx])
                        {
                            `$INSTANCE_NAME`_reseqCnt[sIdx] = reseqCt;
                        }
                    }
                    faultGroup >>= 1u;
                    faultSlave <<= 1u;
                }
                for(groupNum = 0u; groupNum < `$INSTANCE_NAME`_NUMBER_OF_GROUPS; groupNum++)
                {
                    /* Disable all converters configured for immediate off mode */                
                    *enRegList[groupNum] &= ~((uint8)(offBits));
                    offBits >>= `$INSTANCE_NAME`_GROUP_SIZE;
                }
                /* Mask fault ctl inputs */
                `$INSTANCE_NAME`_CTL_LO_MASK_REG &= ~ctlMask;
                `$INSTANCE_NAME`_CTL_HI_MASK_REG &= ~ctlMask;
                ctlMask <<= 1u;
            }
            fault >>= 1u;
        }
    }
    #endif /* `$INSTANCE_NAME`_NUMBER_OF_CTL_INPUTS > 0u */

}


/* [] END OF FILE */
