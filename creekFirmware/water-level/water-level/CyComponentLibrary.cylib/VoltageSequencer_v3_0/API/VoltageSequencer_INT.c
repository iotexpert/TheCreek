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


/*******************************************************************************
* Custom Declarations and Variables
* - add user include files, prototypes and variables between the following
*   #START and #END tags
*******************************************************************************/
/* `#START VoltageSequencer_SYS_VAR`  */

/* `#END`  */


/*******************************************************************************
*       Macros to accelerate processing of 32-bit variables
*******************************************************************************/
#if(`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS <= 8u)
    #define `$INSTANCE_NAME`_CLEAR_BIT(var, grp, msk) ((var) &= ~(msk))
    #define `$INSTANCE_NAME`_SET_BIT(  var, grp, msk) ((var) |=  (msk))
    #define `$INSTANCE_NAME`_TEST_BIT( var, grp, msk) ((var) &   (msk))
    #define `$INSTANCE_NAME`_GET_GROUP(var, grp)       (var)
#else
    #define `$INSTANCE_NAME`_CLEAR_BIT(var, grp, msk) ((((uint8 *)&(var))[grp]) &= ~(msk))
    #define `$INSTANCE_NAME`_SET_BIT(  var, grp, msk) ((((uint8 *)&(var))[grp]) |=  (msk))
    #define `$INSTANCE_NAME`_TEST_BIT( var, grp, msk) ((((uint8 *)&(var))[grp]) &   (msk))
    #define `$INSTANCE_NAME`_GET_GROUP(var, grp)       (((uint8 *)&(var))[grp])
#endif

/*******************************************************************************
*       Define register lists based on the converter number
*******************************************************************************/
#if(`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS <= 8u)
    reg8 * enRegList[`$INSTANCE_NAME`_NUMBER_OF_GROUPS] = {`$INSTANCE_NAME`_EN_CTL1_PTR};
    reg8 * pgRegList[`$INSTANCE_NAME`_NUMBER_OF_GROUPS] = {`$INSTANCE_NAME`_PGOOD_MON1_PTR};
#elif(`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS <= 16u)
    reg8 * enRegList[`$INSTANCE_NAME`_NUMBER_OF_GROUPS] = {
        `$INSTANCE_NAME`_EN_CTL1_PTR, `$INSTANCE_NAME`_EN_CTL2_PTR
    };
    reg8 * pgRegList[`$INSTANCE_NAME`_NUMBER_OF_GROUPS] = {
        `$INSTANCE_NAME`_PGOOD_MON1_PTR, `$INSTANCE_NAME`_PGOOD_MON2_PTR
    };
#elif(`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS <= 24u)
    reg8 * enRegList[`$INSTANCE_NAME`_NUMBER_OF_GROUPS] = {
        `$INSTANCE_NAME`_EN_CTL1_PTR, `$INSTANCE_NAME`_EN_CTL2_PTR,
        `$INSTANCE_NAME`_EN_CTL3_PTR
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
    reg8 * pgRegList[`$INSTANCE_NAME`_NUMBER_OF_GROUPS] = {
        `$INSTANCE_NAME`_PGOOD_MON1_PTR, `$INSTANCE_NAME`_PGOOD_MON2_PTR,
        `$INSTANCE_NAME`_PGOOD_MON3_PTR, `$INSTANCE_NAME`_PGOOD_MON4_PTR
    };
#endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS <= 8u */

/*******************************************************************************
*       Power Up Configuration Variables
*******************************************************************************/
extern `$SeqBitsType`   `$INSTANCE_NAME`_pgoodPrereqList[`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS];
extern uint16   `$INSTANCE_NAME`_tonDelayList   [`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS];
extern uint16   `$INSTANCE_NAME`_tonMaxDelayList[`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS];
extern uint16   `$INSTANCE_NAME`_sysStableTime;
#if(`$INSTANCE_NAME`_ENABLE_PGOOD_ON != 0u)
    extern uint16 `$INSTANCE_NAME`_pgoodOnThresholdList[`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS];
#endif /* `$INSTANCE_NAME`_ENABLE_PGOOD_ON != 0u */

/*******************************************************************************
*       Control Input Configuration Variables
*******************************************************************************/
#if(`$INSTANCE_NAME`_NUMBER_OF_CTL_INPUTS > 0u)
    extern uint8          `$INSTANCE_NAME`_ctlPrereqList       [`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS];
    extern uint8          `$INSTANCE_NAME`_ctlFaultReseqCfg    [`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS];
    extern `$SeqBitsType`         `$INSTANCE_NAME`_ctlGroupShutdownMask[`$INSTANCE_NAME`_NUMBER_OF_CTL_INPUTS];
    extern uint8          `$INSTANCE_NAME`_ctlShutdownMaskList [`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS];
    extern volatile uint8 `$INSTANCE_NAME`_ctlStatus;
#endif /* `$INSTANCE_NAME`_NUMBER_OF_CTL_INPUTS > 0u */

/*******************************************************************************
*      Power Down Configaration Variables
*******************************************************************************/
extern uint16   `$INSTANCE_NAME`_toffDelayList[];
extern uint16   `$INSTANCE_NAME`_toffMaxDelayList[];
#if(`$INSTANCE_NAME`_ENABLE_PGOOD_OFF != 0u)
    extern uint16 `$INSTANCE_NAME`_pgoodOffThresholdList[`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS];
#endif /* `$INSTANCE_NAME`_ENABLE_PGOOD_OFF != 0u */

/*******************************************************************************
*       Fault Response and Resequence Configuration Variables
*******************************************************************************/
extern uint16 `$INSTANCE_NAME`_globalReseqDelay;
extern `$SeqBitsType` `$INSTANCE_NAME`_pgoodShutdownMaskList [`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS];
extern `$SeqBitsType` `$INSTANCE_NAME`_pgoodGroupShutdownMask[`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS];
extern uint8  `$INSTANCE_NAME`_tonMaxFaultReseqCfg   [`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS];
extern uint8  `$INSTANCE_NAME`_pgoodFaultReseqCfg    [`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS];
extern uint8  `$INSTANCE_NAME`_ovFaultReseqCfg       [`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS];
extern uint8  `$INSTANCE_NAME`_uvFaultReseqCfg       [`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS];
extern uint8  `$INSTANCE_NAME`_ocFaultReseqCfg       [`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS];
extern uint8  `$INSTANCE_NAME`_faultReseqSrcList     [`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS];

/* Fault type variables */
`$SeqBitsType` `$INSTANCE_NAME`_tonMaxFaultStatus = 0u;
#if(`$INSTANCE_NAME`_INIT_UV_FAULT_ENABLE != 0u)
    `$SeqBitsType` `$INSTANCE_NAME`_uvFaultStatus = 0u;
#endif /* `$INSTANCE_NAME`_INIT_UV_FAULT_ENABLE != 0u */
#if(`$INSTANCE_NAME`_INIT_OV_FAULT_ENABLE != 0u)
    `$SeqBitsType` `$INSTANCE_NAME`_ovFaultStatus = 0u;
#endif /* `$INSTANCE_NAME`_INIT_OV_FAULT_ENABLE != 0u */
#if(`$INSTANCE_NAME`_INIT_OC_FAULT_ENABLE != 0u)
    `$SeqBitsType` `$INSTANCE_NAME`_ocFaultStatus = 0u;
#endif /* `$INSTANCE_NAME`_INIT_OC_FAULT_ENABLE != 0u */

/* Converter Resequence counters */
`$reseqCnt`

/* Used to determine the reason for entry to PEND_RESEQ state */
volatile `$SeqBitsType` `$INSTANCE_NAME`_faultCond;

/*******************************************************************************
*       Sequencer Command and Status Registers
*******************************************************************************/
extern volatile `$SeqBitsType`  `$INSTANCE_NAME`_forceOnCmdReg;
extern volatile `$SeqBitsType`  `$INSTANCE_NAME`_forceOffCmdReg;
extern volatile `$SeqBitsType`  `$INSTANCE_NAME`_forceOffModeReg;
extern volatile `$SeqBitsType`  `$INSTANCE_NAME`_faultStatus;
extern volatile `$SeqBitsType`  `$INSTANCE_NAME`_warnStatus;

/*******************************************************************************
*      Output Configuration Registers
*******************************************************************************/
extern `$SeqBitsType`   `$INSTANCE_NAME`_faultMask;
extern uint8    `$INSTANCE_NAME`_faultEnable;
extern `$SeqBitsType`   `$INSTANCE_NAME`_warnMask;
#if (`$INSTANCE_NAME`_ENABLE_WARNINGS != 0u)
    extern uint8 `$INSTANCE_NAME`_warnEnable;
#endif /* `$INSTANCE_NAME`_ENABLE_WARNINGS != 0u */

#ifdef `$INSTANCE_NAME`_GENERATE_STATUS
    extern `$SeqBitsType` `$INSTANCE_NAME`_stsPgoodMaskList    [`$INSTANCE_NAME`_NUMBER_OF_STS_OUTPUTS];
    extern `$SeqBitsType` `$INSTANCE_NAME`_stsPgoodPolarityList[`$INSTANCE_NAME`_NUMBER_OF_STS_OUTPUTS];
#endif /* `$INSTANCE_NAME`_GENERATE_STATUS */

/*******************************************************************************
*       Sequencer State Machine Global Variables
*******************************************************************************/
extern CYPDATA volatile uint8   `$INSTANCE_NAME`_state[`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS];
/* Indicates that all power converters are in ON state */
static CYDATA uint8 `$INSTANCE_NAME`_allConvertersOn;
/* TRESEQ_DELAY timer */
volatile uint16 reseqTimer = 0u;
/* Enable pin last sample. Initialized in `$INSTANCE_NAME`_Enable() API */
volatile CYBIT `$INSTANCE_NAME`_enPinLast; 

/*******************************************************************************
* Sequencer State Machine Timer[]. Delay timers for each converter
* (usage depends on state).
*******************************************************************************/
CYPDATA uint16 `$INSTANCE_NAME`_delayTimer[`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS] = {0u};

#ifdef __C51__
    #pragma OT(speed)
#endif


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
    /*************************************************************************** 
    * Enable pin sampling
    ***************************************************************************/
    CYBIT enPinState;
    CYBIT enPinRisingEdge;
    CYBIT enPinFallingEdge;
    
    `$SeqBitsType` forcedAllOn;  /* Forced ON Condition for all converters  */
    `$SeqBitsType` forcedAllOff; /* Forced OFF Condition for all converters */
    `$SeqBitsType` powerOffMode; /* Shutdown mode for all converters */
    
    /***************************************************************************
    * Enable bits are buffered while walking all rails and then applied to 
    * physical enable bits before exit from ISR.
    ***************************************************************************/
    `$SeqBitsType` goOnBits  = 0u;
    `$SeqBitsType` goOffBits = 0u;
    CYBIT railGoesOn  = 0u; /* Assume no rails will go ON  */
    CYBIT railGoesOff = 0u; /* Assume no rails will go OFF */
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
    * any subsequent rails are aware.
    *******************************************************************************/
    static CYBIT isPowerDownLast = 0u;
    static CYBIT isPowerDown;

    CYDATA uint8  converterNum;             /* converter index */
    CYDATA uint8  volatile CYPDATA *pstate; /* ptr to rail states (faster) */
    CYDATA uint16 CYPDATA *ptimer;          /* ptr to rail timers (faster) */
    
    #if(`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS > 8u)
        CYDATA uint8 group = `$INSTANCE_NAME`_GROUP1;
    #endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS > 8u */
    
    reg8 * onReg = `$INSTANCE_NAME`_ON_CTL1_PTR;    /* pointer to group ON register */
    uint8  pgReg = `$INSTANCE_NAME`_PGOOD_MON1_REG; /* group pgood status register  */
    CYDATA uint8 converterMask  = 1u;    
    CYDATA uint8 forcedOff;
	CYDATA uint8 ctlStatus;
    `$SeqBitsType` pgStatus;
    
    #if(CY_PSOC3)
        uint8 int_en = EA;
        CyGlobalIntEnable;
    #endif /* CY_PSOC3 */
    
    /* Determine power converter pgood status for all converters */
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
    
    /* Sample enable and ctl[x] pins */
    ctlStatus = `$INSTANCE_NAME`_CTL_MON_REG;

    /* Determine enable signal toggling */
    enPinState = ((ctlStatus & `$INSTANCE_NAME`_EN_PIN) != 0u) ? 1u : 0u;
    enPinRisingEdge = !`$INSTANCE_NAME`_enPinLast && enPinState;
    enPinFallingEdge = `$INSTANCE_NAME`_enPinLast && !enPinState;
    `$INSTANCE_NAME`_enPinLast = enPinState;
    
    #if(`$INSTANCE_NAME`_NUMBER_OF_CTL_INPUTS > 0u)
        /* Isolate ctl[x] pins */
        ctlStatus &= `$INSTANCE_NAME`_CTL_MASK;
    #endif /* `$INSTANCE_NAME`_NUMBER_OF_CTL_INPUTS > 0u */
    
    /* Forced ON condition via a command over a communications interface */
    forcedAllOn = `$INSTANCE_NAME`_forceOnCmdReg;
    /* Clear Forced ON Register to prevent subsequent ONs without a host request */
    `$INSTANCE_NAME`_forceOnCmdReg = 0u;

    /* Forced ON condition by toggling the enable signal from low to high */
    if(enPinRisingEdge != 0u)
    {
        forcedAllOn = `$INSTANCE_NAME`_CONVERTER_MASK;
    }

    /***************************************************************************
    * Forced OFF condition via a command over a communications interface or by
    * de-asserting of the enable signal.
    *
    * Disable Fault ISR to make update of forceOffCmdReg and powerOffMode atomic
    ***************************************************************************/
    if(enPinFallingEdge != 0u)
    {
        `$INSTANCE_NAME`_IntDisable(`$INSTANCE_NAME`_FAULT_ISR_NUMBER);
        /* Disable all converters */
        forcedAllOff = `$INSTANCE_NAME`_CONVERTER_MASK;
        /*********************************************************************** 
        * A soft shutdown is the only option when the Forced OFF is caused by
        * de-asserting the enable pin.
        ***********************************************************************/
        powerOffMode = `$INSTANCE_NAME`_CONVERTER_MASK;        
        /***********************************************************************
        * Clear Forced OFF Registers to prevent subsequent OFFs without a host 
        * request.
        ***********************************************************************/
        `$INSTANCE_NAME`_forceOffCmdReg  = 0u;
        `$INSTANCE_NAME`_forceOffModeReg = 0u;
        `$INSTANCE_NAME`_IntEnable(`$INSTANCE_NAME`_FAULT_ISR_NUMBER);
    }
    else if(`$INSTANCE_NAME`_forceOffCmdReg != 0ul)
    {
        `$INSTANCE_NAME`_IntDisable(`$INSTANCE_NAME`_FAULT_ISR_NUMBER);
        forcedAllOff = `$INSTANCE_NAME`_forceOffCmdReg;
        powerOffMode = `$INSTANCE_NAME`_forceOffModeReg;
        /***********************************************************************
        * Clear Forced OFF Registers to prevent subsequent OFFs without a host 
        * request.
        ***********************************************************************/
        `$INSTANCE_NAME`_forceOffCmdReg  = 0u;
        `$INSTANCE_NAME`_forceOffModeReg = 0u;
        `$INSTANCE_NAME`_IntEnable(`$INSTANCE_NAME`_FAULT_ISR_NUMBER);
    }
    else
    {
        /* No rails are being forced off */
        forcedAllOff = 0u;
        powerOffMode = 0u;
    }

    /***************************************************************************
    * Indicates that all power converters are in ON state. Decrementing
    * each time a converter is in ON state and is tested against 0s to
    * generate sys_up status output.
    ***************************************************************************/
    `$INSTANCE_NAME`_allConvertersOn = `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS;
    /***************************************************************************
    *  The "isPowerDown" variable is gathered while walking all rails. It needs
    *  to be reset each time before polling all rails.
    ***************************************************************************/
    isPowerDown = 0u;

    /* Power Converter State Machine Handling */
    ptimer = &`$INSTANCE_NAME`_delayTimer[0u];   /* pointer shortcut to this Timer */
    pstate = &`$INSTANCE_NAME`_state[0u];        /* pointer shortcut to this State */    

    forcedOff = `$INSTANCE_NAME`_GET_GROUP(forcedAllOff, `$INSTANCE_NAME`_GROUP1);

    for(converterNum = 0u; converterNum < `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS; converterNum++)
    {
        /***********************************************************************
        * Update all variables that need to be updated once per converter group
        * (8 converters). This makes state machine state transitions faster.
        ***********************************************************************/
        #if(`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS > 8u)
            /* Converters 1 through 8 variables are set during initialization */
            #if(`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS <= 16u)
                if(converterNum == 8u) /* Converters  9 through 16 */
                {
                    group = `$INSTANCE_NAME`_GROUP2;
                    onReg = `$INSTANCE_NAME`_ON_CTL2_PTR;
                    pgReg = `$INSTANCE_NAME`_PGOOD_MON2_REG;
                    converterMask  = 1u;
                    forcedOff = `$INSTANCE_NAME`_GET_GROUP(forcedAllOff, `$INSTANCE_NAME`_GROUP2);
                }
            #elif(`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS <= 24u)
                switch(converterNum)
                {
                    case 8u: /* Converters  9 through 16 */
                        group = `$INSTANCE_NAME`_GROUP2;
                        onReg = `$INSTANCE_NAME`_ON_CTL2_PTR;
                        pgReg = `$INSTANCE_NAME`_PGOOD_MON2_REG;
                        converterMask = 1u;
                        forcedOff = `$INSTANCE_NAME`_GET_GROUP(forcedAllOff, `$INSTANCE_NAME`_GROUP2);
                        break;
                    case 16u:
                         /* Converters 17 through 24 */
                        group = `$INSTANCE_NAME`_GROUP3;
                        onReg = `$INSTANCE_NAME`_ON_CTL3_PTR;
                        pgReg = `$INSTANCE_NAME`_PGOOD_MON3_REG;
                        converterMask = 1u;
                        forcedOff = `$INSTANCE_NAME`_GET_GROUP(forcedAllOff, `$INSTANCE_NAME`_GROUP3);
                        break;
                    default:
                        break;
                }
            #else /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS <= 32u */
                switch(converterNum)
                {
                    case 8u: /* Converters  9 through 16 */
                        group = `$INSTANCE_NAME`_GROUP2;
                        onReg = `$INSTANCE_NAME`_ON_CTL2_PTR;
                        pgReg = `$INSTANCE_NAME`_PGOOD_MON2_REG;
                        converterMask = 1u;
                        forcedOff = `$INSTANCE_NAME`_GET_GROUP(forcedAllOff, `$INSTANCE_NAME`_GROUP2);
                        break;
                    case 16u:
                         /* Converters 17 through 24 */
                        group = `$INSTANCE_NAME`_GROUP3;
                        onReg = `$INSTANCE_NAME`_ON_CTL3_PTR;
                        pgReg = `$INSTANCE_NAME`_PGOOD_MON3_REG;
                        converterMask = 1u;
                        forcedOff = `$INSTANCE_NAME`_GET_GROUP(forcedAllOff, `$INSTANCE_NAME`_GROUP3);
                        break;
                    case 24u:
                        /* Converters 25 through 32 */
                        group = `$INSTANCE_NAME`_GROUP4;
                        onReg = `$INSTANCE_NAME`_ON_CTL4_PTR;
                        pgReg = `$INSTANCE_NAME`_PGOOD_MON4_REG;
                        converterMask = 1u;
                        forcedOff = `$INSTANCE_NAME`_GET_GROUP(forcedAllOff, `$INSTANCE_NAME`_GROUP4);
                        break;
                    default:
                        break;
                }
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS <= 16u */
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS > 8u */

        switch(*pstate)
        {
        case `$INSTANCE_NAME`_OFF:

            /*******************************************************************
            * Converter is OFF (steady state)                   state = #0
            * Converter may be moved to PEND_ON with outside intervention.
            *******************************************************************/
            if(`$INSTANCE_NAME`_TEST_BIT(forcedAllOn, group, converterMask) != 0u)
            {
                /***************************************************************
                * If the TrimMargin component is used in the design,
                * this is the appropriate time to change the PWM duty
                * cycle to the pre-run setting.
                * Add your custom code between the following #START
                * and #END tags
                ***************************************************************/
                /* `#START TRIM_CFG_PreRun_SECTION1` */

                /* `#END` */

                *pstate = `$INSTANCE_NAME`_PEND_ON;
                `$INSTANCE_NAME`_reseqCnt[converterNum] = `$INSTANCE_NAME`_INIT_RESEQ_CNT;
            }
            else
            {
                allConvertersOff--;
            }
            break;

        case `$INSTANCE_NAME`_PEND_ON:

            /*******************************************************************
            * Converter is OFF (transient or steady state)      state = #1
            * Converter will begin the power-on sequence when its start
            * prerequisites are met.
            *******************************************************************/
            if((forcedOff & converterMask) != 0u)
            {
                /* Load the TOFF_MAX_WARN_LIMIT period */
                *ptimer = `$INSTANCE_NAME`_toffMaxDelayList[converterNum];
                *pstate = `$INSTANCE_NAME`_TOFF_MAX_WARN_LIMIT;
                /***************************************************************
                * If the PowerMonitor or the VoltageFaultDetector components
                * generate the pgood[x] inputs to the VoltageSequencer,
                * this is the appropriate time to lower the UV threshold to
                * ~12.5% to detect when the rail voltage has decayed to "OFF".
                * Add your custom code between the following #START
                * and #END tags
                ***************************************************************/
                /* `#START TRIM_CFG_POff_SECTION1` */

                /* `#END` */
            }
            else
            {
                if(enPinState != 0u)
                {
                    #if(`$INSTANCE_NAME`_NUMBER_OF_CTL_INPUTS > 0u)
                        if((ctlStatus & `$INSTANCE_NAME`_ctlPrereqList[converterNum]) != 0u)
                        {
                            break;
                        }
                    #endif

                    if((pgStatus & `$INSTANCE_NAME`_pgoodPrereqList[converterNum]) ==
                                   `$INSTANCE_NAME`_pgoodPrereqList[converterNum])
                    {
                       *pstate = `$INSTANCE_NAME`_TON_DELAY;
                       *ptimer = `$INSTANCE_NAME`_tonDelayList[converterNum];
                    }
                }
                else
                {
                    /* If the "enable" pin is low, no converter can power up */
                }
            }
            break;

        case `$INSTANCE_NAME`_TON_DELAY:
        
            /*******************************************************************
            * Converter is OFF (transient state)                state = #2
            * Converter will imminently power ON after this short TON delay.
            *******************************************************************/
            if((forcedOff & converterMask) != 0u)
            {
                /* Load the TOFF_MAX_WARN_LIMIT period */
                *ptimer = `$INSTANCE_NAME`_toffMaxDelayList[converterNum];
                *pstate = `$INSTANCE_NAME`_TOFF_MAX_WARN_LIMIT;
                /***************************************************************
                * If the PowerMonitor or the VoltageFaultDetector components
                * generate the pgood[x] inputs to the VoltageSequencer,
                * this is the appropriate time to lower the UV threshold to
                * ~12.5% to detect when the rail voltage has decayed to "OFF".
                * Add your custom code between the following #START
                * and #END tags
                ***************************************************************/
                /* `#START TRIM_CFG_POff_SECTION2` */

                /* `#END` */
            }
            else
            {
                if(*ptimer != 0u)
                {
                    --*ptimer;
                }
                else
                {
                    /***********************************************************
                    * If the TrimMargin component is used in the design,
                    * this is the appropriate time to change the PWM duty
                    * cycle from the pre-run setting to the run setting.
                    * Add your custom code between the following #START
                    * and #END tags
                    ***********************************************************/
                    /* `#START TRIM_CFG_Run_SECTION` */

                    /* `#END` */

                    /***********************************************************
                    * If the PowerMonitor or the VoltageFaultDetector components
                    * generate the pgood[x] inputs to the VoltageSequencer,
                    * this is the appropriate time to set the desired PGood
                    * threshold to detect when the rail voltage reaches "ON".
                    * Add your custom code between the following #START
                    * and #END tags
                    ***********************************************************/
                    /* `#START TRIM_CFG_PGood_SECTION` */

                    /* `#END` */

                    /* Enable the associated power converter */
                    `$INSTANCE_NAME`_SET_BIT(goOnBits, group, converterMask);
                    railGoesOn = 1u;
                    /* Load the TON_MAX_DELAY period */
                    *ptimer = `$INSTANCE_NAME`_tonMaxDelayList[converterNum];
                    *pstate = `$INSTANCE_NAME`_TON_MAX;
                }
            }
            break;

        case `$INSTANCE_NAME`_TON_MAX:
	
            /*******************************************************************
            * Converter is ON (transient state)                 state = #3
            * Converter is ramping up to either ON or a Fault
            *******************************************************************/
            if((forcedOff & converterMask) != 0u)
            {
                if(`$INSTANCE_NAME`_TEST_BIT(powerOffMode, group, converterMask) == 0u)
                {
                    /* An immediate shutdown */
                    `$INSTANCE_NAME`_SET_BIT(goOffBits, group, converterMask);
                    railGoesOff = 1u;
                    *pstate = `$INSTANCE_NAME`_TOFF_MAX_WARN_LIMIT;
                    *ptimer = `$INSTANCE_NAME`_toffMaxDelayList[converterNum];
                    /***********************************************************
                    * If the PowerMonitor or the VoltageFaultDetector components
                    * generate the pgood[x] inputs to the VoltageSequencer,
                    * this is the appropriate time to lower the UV threshold to
                    * ~12.5% to detect when the rail voltage has decayed to "OFF".
                    * Add your custom code between the following #START
                    * and #END tags
                    ***********************************************************/
                    /* `#START TRIM_CFG_POff_SECTION3` */

                    /* `#END` */
                 }
                else
                {
                    /* A soft shutdown */
                    *pstate = `$INSTANCE_NAME`_TOFF_DELAY;
                    /* Load the TOFF_DELAY period */
                    *ptimer = `$INSTANCE_NAME`_toffDelayList[converterNum];
                }
            }
            else
            {
                if((pgReg & converterMask) != 0u)
                {
                    /**********************************************************
                    * If the PowerMonitor or the VoltageFaultDetector
                    * components are used to generate the pgood[x] inputs
                    * to the VoltageSequencer, this is the appropriate time
                    * to lower the UV threshold to the normal operating PFault
                    * threshold to add hysteresis between PGood and PFault
                    * Add your custom code between the following #START
                    * and #END tags
                    ***********************************************************/
                    /* `#START TRIM_CFG_PFault_SECTION` */

                    /* `#END` */

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
                        `$SeqBitsType` faultGroup;
                        CYBIT offMode;
                        CYDATA uint8 reseqCt;
                        `$SeqBitsType` offBits = 0u;
                        CYDATA uint8 sIdx;
                        /* Disable the associated power converter */
                        *enRegList[(converterNum >> 3u)] &= ~converterMask;
                        /* Update fault status register */
                        `$INSTANCE_NAME`_SET_BIT(`$INSTANCE_NAME`_faultStatus, group, converterMask);
                        `$INSTANCE_NAME`_SET_BIT(`$INSTANCE_NAME`_tonMaxFaultStatus, group, converterMask);
                        /* Establish which converters should go off */
                        faultGroup = `$INSTANCE_NAME`_pgoodGroupShutdownMask[converterNum];
                        /*******************************************************
                        * Set fault flag that will indicate the reason of
                        *  entering PEND_RESEQ for the associated converter.
                        *******************************************************/
                        `$INSTANCE_NAME`_faultCond |= faultGroup;
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
                                #if(`$INSTANCE_NAME`_NUMBER_OF_GROUPS > 1u)
                                    /* Go to next group bits */
                                    offBits >>= `$INSTANCE_NAME`_GROUP_SIZE;
                                #endif /* `$INSTANCE_NAME`_NUMBER_OF_GROUPS > 1u */
                            }
                            `$INSTANCE_NAME`_IntDisable(`$INSTANCE_NAME`_FAULT_ISR_NUMBER);
                            /* Request for power down fault group */
                            `$INSTANCE_NAME`_forceOffCmdReg |= faultGroup;
                            `$INSTANCE_NAME`_IntEnable(`$INSTANCE_NAME`_FAULT_ISR_NUMBER);
                        }
                        else
                        {
                            `$INSTANCE_NAME`_IntDisable(`$INSTANCE_NAME`_FAULT_ISR_NUMBER);
                            /* Request for power down fault group */
                            `$INSTANCE_NAME`_forceOffCmdReg   |= faultGroup;
                            `$INSTANCE_NAME`_forceOffModeReg  |= faultGroup;
                            `$INSTANCE_NAME`_IntEnable(`$INSTANCE_NAME`_FAULT_ISR_NUMBER);
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
                                    /******************************************* 
                                    * The counter needs to be updated only if
                                    * the new fault has a lower re-seq value 
                                    * than the previous fault.
                                    *******************************************/
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
            break;

         case `$INSTANCE_NAME`_ON:

            /*******************************************************************
            * Converter is ON (steady state)                    state = #4
            * Converter normally spends its life here.
            *******************************************************************/
            if((forcedOff & converterMask) != 0u)
            {
                #if(`$INSTANCE_NAME`_NUMBER_OF_CTL_INPUTS > 0u)
                    /* Disable ctl fault monitoring for the associated power converter */
                    `$INSTANCE_NAME`_CTL_MON_MASK_REG &= ~`$INSTANCE_NAME`_ctlShutdownMaskList[converterNum];
                #endif /* `$INSTANCE_NAME`_NUMBER_OF_CTL_INPUTS > 0u */

                if(`$INSTANCE_NAME`_TEST_BIT(powerOffMode, group, converterMask) == 0u)
                {
                    /* An immediate shutdown */
                    *onReg &= ~converterMask;
                    `$INSTANCE_NAME`_SET_BIT(goOffBits, group, converterMask);
                    railGoesOff = 1u;
                    *pstate = `$INSTANCE_NAME`_TOFF_MAX_WARN_LIMIT;
                    /***********************************************************
                    * If the PowerMonitor or the VoltageFaultDetector components
                    * generate the pgood[x] inputs to the VoltageSequencer,
                    * this is the appropriate time to lower the UV threshold to
                    * ~12.5% to detect when the rail voltage has decayed to "OFF".
                    * Add your custom code between the following #START
                    * and #END tags
                    ***********************************************************/
                    /* `#START TRIM_CFG_POff_SECTION4` */

                    /* `#END` */
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
            else
            {
                `$INSTANCE_NAME`_allConvertersOn--;
                if((*onReg & converterMask) == 0u)
                {
                    *onReg |= converterMask;

                    #if(`$INSTANCE_NAME`_NUMBER_OF_CTL_INPUTS > 0u)
                        /* Enable ctl fault monitoring for the associated power converter */
                        `$INSTANCE_NAME`_CTL_MON_MASK_REG |= `$INSTANCE_NAME`_ctlShutdownMaskList[converterNum];
                    #endif /* `$INSTANCE_NAME`_NUMBER_OF_CTL_INPUTS > 0u */
                }
            }            
            break;

        case `$INSTANCE_NAME`_TOFF_DELAY:

            /*******************************************************************
            * Converter is ON (transient state)                 state = #5
            * Converter *will* imminently power OFF
            *******************************************************************/
            isPowerDown = 1u;

            if(((forcedOff & converterMask) != 0u) || (*ptimer == 0))
            {
                `$INSTANCE_NAME`_SET_BIT(goOffBits, group, converterMask);
                railGoesOff = 1u;
                /* Load the TOFF_MAX_WARN_LIMIT timer */
                *ptimer = `$INSTANCE_NAME`_toffMaxDelayList[converterNum];
                *pstate = `$INSTANCE_NAME`_TOFF_MAX_WARN_LIMIT;
                /***************************************************************
                * If the PowerMonitor or the VoltageFaultDetector components
                * generate the pgood[x] inputs to the VoltageSequencer,
                * this is the appropriate time to lower the UV threshold to
                * ~12.5% to detect when the rail voltage has decayed to "OFF".
                * Add your custom code between the following #START
                * and #END tags
                ***************************************************************/
                /* `#START TRIM_CFG_POff_SECTION5` */

                /* `#END` */
            }            
            else
            {
                --*ptimer;
            }
            break;

        case `$INSTANCE_NAME`_TOFF_MAX_WARN_LIMIT:

            /*******************************************************************
            * Converter is OFF (transient state)                state = #6
            * Waiting for either voltage to decay or a timeout
            *******************************************************************/
            isPowerDown = 1u;

            /* Check if the associated converter is shutdown */
            if ((pgReg & converterMask) == 0u)
            {
                *pstate = `$INSTANCE_NAME`_PEND_RESEQ;
                *ptimer = 0u; /* Reset timer */
            }
            else
            {
                if (*ptimer != 0u)
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

            /*******************************************************************
            * Converter is OFF (transient or steady state)      state = #7
            * Waiting for all converters to reach a steady state.
            *******************************************************************/
            if(`$INSTANCE_NAME`_TEST_BIT(`$INSTANCE_NAME`_faultCond, group, converterMask) == 0u)
            {
                /* Converter entered PEND_RESEQ due to forced OFF condition */
                *pstate = `$INSTANCE_NAME`_OFF;
            }
            else 
            {
                /* Converter entered PEND_RESEQ due to fault condition */
                if(`$INSTANCE_NAME`_reseqCnt[converterNum] == 0u)
                {
                    *pstate = `$INSTANCE_NAME`_OFF;
                    `$INSTANCE_NAME`_CLEAR_BIT(`$INSTANCE_NAME`_faultCond, group, converterMask);
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
                        /******************************************************* 
                        * No state machine can make transition to TRESEQ_DELAY
                        * if any other state machine is currently still in the
                        * process of powering down.
                        *******************************************************/
                    }
                }
            }
            break;

        case `$INSTANCE_NAME`_TRESEQ_DELAY:

            /*******************************************************************
            * Converter is OFF (transient state)                state = #8
            * Delay before entering PEND_ON state
            *******************************************************************/
            if((forcedOff & converterMask) != 0u)
            {
                /* Load the TOFF_MAX_WARN_LIMIT timer */
                *ptimer = `$INSTANCE_NAME`_toffMaxDelayList[converterNum];
                *pstate = `$INSTANCE_NAME`_TOFF_MAX_WARN_LIMIT;
                /***************************************************************
                * If the PowerMonitor or the VoltageFaultDetector components
                * generate the pgood[x] inputs to the VoltageSequencer,
                * this is the appropriate time to lower the UV threshold to
                * ~12.5% to detect when the rail voltage has decayed to "OFF".
                * Add your custom code between the following #START
                * and #END tags
                ***************************************************************/
                /* `#START TRIM_CFG_POff_SECTION6` */

                /* `#END` */
            }
            else
            {
                if (reseqTimer == 0u)
                {
                    /*******************************************************
                    * If the TrimMargin component is used in the design,
                    * this is the appropriate time to change the PWM duty
                    * cycle back to the pre-run setting from the run setting.

                    * Add your custom code between the following #START
                    * and #END tags
                    *******************************************************/
                    /* `#START TRIM_CFG_PreRun_SECTION2` */

                    /* `#END`  */
                    *pstate = `$INSTANCE_NAME`_PEND_ON;
                }
                else
                {
                    /* Wait until RESEQ_DELAY expires */
                }
            }
            break;
            
        default:
            break;
        }
        pstate++;               /* Go to next converter */
        ptimer++;               /* Set primer ptr to associated timer */
        converterMask <<= 1u;   /* Update converter mask */
    }

    /***************************************************************************
    *
    * If converter(s) will be enabled or disabled
    *
    ***************************************************************************/
    if((railGoesOn == 1u) || (railGoesOff == 1u))
    {
        /***********************************************************************
        * Apply buffered enable bits to physical enable bits
        ***********************************************************************/
        {
            `$INSTANCE_NAME`_IntDisable(`$INSTANCE_NAME`_FAULT_ISR_NUMBER);
            /* Mask fault rail(s) */
            goOnBits &= ~`$INSTANCE_NAME`_faultCond;
            
            /* Enable  new ON  rail(s) */
            `$INSTANCE_NAME`_SET_BIT(    goOnBits, `$INSTANCE_NAME`_GROUP1, `$INSTANCE_NAME`_EN_CTL1_REG);
            #if(`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS > 8u)
                `$INSTANCE_NAME`_SET_BIT(goOnBits, `$INSTANCE_NAME`_GROUP2, `$INSTANCE_NAME`_EN_CTL2_REG);
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS > 8u */
            #if(`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS > 16u)
                `$INSTANCE_NAME`_SET_BIT(goOnBits, `$INSTANCE_NAME`_GROUP3, `$INSTANCE_NAME`_EN_CTL3_REG);
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS > 16u */
            #if(`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS > 24u)
                `$INSTANCE_NAME`_SET_BIT(goOnBits, `$INSTANCE_NAME`_GROUP4, `$INSTANCE_NAME`_EN_CTL4_REG);
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS > 16u */
 
            /* Disable new OFF rail(s) */
            goOnBits &= ~goOffBits;
            /* Update Enables Port */
            `$INSTANCE_NAME`_EN_CTL1_REG = `$INSTANCE_NAME`_GET_GROUP(    goOnBits, `$INSTANCE_NAME`_GROUP1);            
            #if(`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS > 8u)
                `$INSTANCE_NAME`_EN_CTL2_REG = `$INSTANCE_NAME`_GET_GROUP(goOnBits, `$INSTANCE_NAME`_GROUP2);
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS > 8u */
            #if(`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS > 16u)
                `$INSTANCE_NAME`_EN_CTL3_REG = `$INSTANCE_NAME`_GET_GROUP(goOnBits, `$INSTANCE_NAME`_GROUP3);
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS > 16u */
            #if(`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS > 24u)
                `$INSTANCE_NAME`_EN_CTL4_REG = `$INSTANCE_NAME`_GET_GROUP(goOnBits, `$INSTANCE_NAME`_GROUP4);
            #endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS > 24u */
            `$INSTANCE_NAME`_IntEnable(`$INSTANCE_NAME`_FAULT_ISR_NUMBER);
        }
    }

    /* Updated system powering down status */
    isPowerDownLast = isPowerDown;

    /***************************************************************************
    * Calculate the value for each of the programmable status output bits.
    * A status output bit can be asserted for any combination of PGood input
    * values.  Specified via a mask and polarity.
    ***************************************************************************/
    #ifdef `$INSTANCE_NAME`_GENERATE_STATUS
    {
        CYDATA uint8 stsNum;
        CYDATA uint8 sts = 0u;
        CYDATA uint8 stsBit = 1u;
        for(stsNum = 0u; stsNum < `$INSTANCE_NAME`_NUMBER_OF_STS_OUTPUTS; stsNum++)
        {
            if(((pgStatus ^ `$INSTANCE_NAME`_stsPgoodPolarityList[stsNum]) &
                            `$INSTANCE_NAME`_stsPgoodMaskList[stsNum]) == 0u)
            {
                sts |= stsBit;
            }
            stsBit <<= 1u;
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
        #if (`$INSTANCE_NAME`_ENABLE_WARNINGS != 0u)
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
        #endif /* `$INSTANCE_NAME`_ENABLE_WARNINGS != 0u */

        /* Generation of fault output */
        if(`$INSTANCE_NAME`_faultEnable != 0u)
        {
            if((`$INSTANCE_NAME`_faultStatus & `$INSTANCE_NAME`_faultMask) != 0ul)
            {
                `$INSTANCE_NAME`_SYSTEM_STATUS_REG |= `$INSTANCE_NAME`_FAULT_MASK;
            }
            else
            {
                /* De-assert fault output signal */
                `$INSTANCE_NAME`_SYSTEM_STATUS_REG &= ~`$INSTANCE_NAME`_FAULT_MASK;
            }
        }
        else
        {
            /* Generation of fault output signal is disabled */
        }

    #endif /* `$INSTANCE_NAME`_VSeq_SystemStsReg__REMOVED */

    /*******************************************************
    * Optional custom user ISR exit code. For example,
    * setting a global boolean to indicate this ISR has
    * executed as part of an overall system-wide watchdog
    * timer design.
    *******************************************************/
    /* `#START CUSTOM_ISR_SECTION` */

    /* `#END`  */
    
    #if(CY_PSOC3)
        EA = int_en;
    #endif /* CY_PSOC3 */
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

    #if(CY_PSOC3)
        uint8 int_en = EA;
        CyGlobalIntEnable;
    #endif /* CY_PSOC3 */
    
    /*************************************************************************** 
    * `$INSTANCE_NAME`_allConvertersOn is indicating that system is in ON 
    * state. Implemented as decrement counter. Therefore its value is zero when
    * the system is ON.
    ****************************************************************************/
    sysState = (`$INSTANCE_NAME`_allConvertersOn == 0u) ? 1u : 0u;
    /* Reset sysStable status if not all converters are in ON state */
    if (sysState == 0u)
    {
        sysStable = 0u;
    }
    /* Reload System Stable Timer if system goes to ON state */
    if ((sysStateLast == 0u) && (sysState != 0u))
    {
        stableTimer = `$INSTANCE_NAME`_sysStableTime;
    }

    if (sysStable == 0u)  /* System has not been up for the specified time */
    {
        if ((sysState != 0u) && (stableTimer != 0))
        {
            stableTimer--;
        }
        else
        {
            if (sysState != 0u) /* Sys is ON and system stable time has expired */
            {
                sysStable = 1u;  /* Declare that system is stable */

                #ifndef `$INSTANCE_NAME`_VSeq_SystemStsReg__REMOVED
                    `$INSTANCE_NAME`_SYSTEM_STATUS_REG |= `$INSTANCE_NAME`_SYS_STABLE_MASK;
                #endif

                for (converterNum = 0u; converterNum < `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS; converterNum++)
                {
                    /* Reset re-sequence counters */
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
    if (reseqTimer != 0u)
    {
        reseqTimer--;
    }

    /*******************************************************
    * Optional custom user ISR exit code. For example,
    * setting a global boolean to indicate this ISR has
    * executed as part of an overall system-wide watchdog
    * timer design.
    *******************************************************/
    /* `#START CUSTOM_STABLE_ISR_SECTION` */

    /* `#END`  */
    
    #if(CY_PSOC3)
        EA = int_en;
    #endif /* CY_PSOC3 */
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
    `$SeqBitsType` faultGroup;
    `$SeqBitsType` offBits = 0u;
    CYDATA uint8 fault;
    CYDATA uint8 reseqCt = 0u;
    CYBIT offMode = 0u;
    
    #if(CY_PSOC3)
        uint8 int_en = EA;
        CyGlobalIntEnable;
    #endif /* CY_PSOC3 */
    
    /***************************************************************************
    * Custom Declarations and Variables
    * - add user variables between the following #START and #END tags
    ***************************************************************************/
    /* `#START FaultHandler_VAR`  */

    /* `#END`  */

    /***************************************************************************
    * Handle de-assertion of PGood
    ***************************************************************************/
    groupOffset = 0u;
    for (groupNum = 0u; groupNum < `$INSTANCE_NAME`_NUMBER_OF_GROUPS; groupNum++)
    {
        `$SeqBitsType` faultMaster;
        uint8 pgReg;        /* group pgood status */
        enReg =  enRegList[groupNum];
        pgReg = *pgRegList[groupNum];
        /* For fault converters en[x] is asserted and pgood status faulted */
        fault = ~(pgReg) & *enReg;
        if (fault != 0u)
        {
            *enReg &= ~fault;       /* Disable fault master(s)  */
            mIdx = groupOffset;     /* Find fault slaves        */
            while (fault != 0u)
            {
                if ((fault & 0x01) != 0u) /* If this pgood status faulted */
                {
                    uint8 faultSrc = 0u;
                    faultMaster = ((`$SeqBitsType`)1u << mIdx);
                    `$INSTANCE_NAME`_faultStatus |= faultMaster;  /* Set fault status */

                    /***********************************************************
                    * PGood has been de-asserted for an unmasked fault
                    *  converter.  If the converter has OV, UV or OC fault
                    *  source enabled, then this is appropriate place to call
                    *  associated Power Monitor or Fault Detector
                    *  component APIs if different actions are desired.
                    *
                    *  Note that getting the fault type from the Power Monitor
                    *  or Fault Detector component APIs clears their "sticky"
                    *  bits, so if the fault type is also needed elsewhere, then
                    *  the fault type must be publicly shared from here.
                    *
                    *   Possible fault sources are:
                    *   - `$INSTANCE_NAME`_PGOOD_FAULT_SRC   0x0
                    *   - `$INSTANCE_NAME`_OV_FAULT_SRC      0x1
                    *   - `$INSTANCE_NAME`_UV_FAULT_SRC      0x2
                    *   - `$INSTANCE_NAME`_OC_FAULT_SRC      0x4
                    ***********************************************************/
                    if (`$INSTANCE_NAME`_faultReseqSrcList[mIdx] != `$INSTANCE_NAME`_PGOOD_FAULT_SRC)
                    {
                        /*******************************************************
                        * Add your custom ISR code between the following #START
                        * and #END tags to determine the fault source.
                        *******************************************************/
                        /* `#START FAULT_HANDLER_ISR` */

                        /* `#END`  */
                    }
                    else
                    {
                        faultSrc = `$INSTANCE_NAME`_PGOOD_FAULT_SRC;
                    }
                    /***********************************************************
                    * Get Reseq Count for Master Fault
                    ***********************************************************/
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

                    /***********************************************************
                    * Mask fault groups and force Sequencer State machine to
                    * make corresponding state transition.
                    ***********************************************************/
                    faultGroup = `$INSTANCE_NAME`_pgoodGroupShutdownMask[mIdx];
                    `$INSTANCE_NAME`_faultCond      |= faultGroup;
                    `$INSTANCE_NAME`_forceOffCmdReg |= faultGroup;

                    /* Process shutdown mode */
                    if (offMode == `$INSTANCE_NAME`_IMMEDIATE_OFF)
                    {
                        offBits = faultGroup;
                        for (groupNum = 0u; groupNum < `$INSTANCE_NAME`_NUMBER_OF_GROUPS; groupNum++)
                        {
                            /* Disable all converters configured for immediate off mode */
                            *enRegList[groupNum] &= ~((uint8)(offBits));
                            #if(`$INSTANCE_NAME`_NUMBER_OF_GROUPS > 1u)
                                /* Go to next group bits */
                                offBits >>= `$INSTANCE_NAME`_GROUP_SIZE;
                            #endif /* `$INSTANCE_NAME`_NUMBER_OF_GROUPS > 1u */
                        }
                    }
                    else
                    {
                        `$INSTANCE_NAME`_forceOffModeReg |= faultGroup;
                    }
                    /* Update Resequence counter for fault group */
                    for (sIdx = 0u; (sIdx < `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS) && (faultGroup != 0ul); sIdx++)
                    {
                        if ((faultGroup & 0x01ul) != 0u)
                        {
                            if (reseqCt < `$INSTANCE_NAME`_reseqCnt[sIdx])
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
        }
        groupOffset += `$INSTANCE_NAME`_GROUP_SIZE;
    }

    #if(`$INSTANCE_NAME`_NUMBER_OF_CTL_INPUTS > 0u)
    /***************************************************************************
    * Handle de-assertion of ctl[x] inputs
    ***************************************************************************/
    {
        CYDATA uint8 ctlNum;
        CYDATA uint8 ctlMask;
        /* Determine which power converter has ctl[x] fault detection enabled */
        ctlMask = `$INSTANCE_NAME`_CTL_MON_MASK_REG & `$INSTANCE_NAME`_CTL_MASK;
        /* Determine which ctl[x] pin caused a fault condition */
        fault   = `$INSTANCE_NAME`_CTL_MON_REG & ctlMask;
        /* Mask faulted ctl[x] pins */
        `$INSTANCE_NAME`_CTL_MON_MASK_REG &= ~fault;
        /* Provide ctlStatus to main code */
        `$INSTANCE_NAME`_ctlStatus |= fault;
        ctlMask = 1u;
        for (ctlNum = 0u; (ctlNum < `$INSTANCE_NAME`_NUMBER_OF_CTL_INPUTS) && (fault != 0u); ctlNum++)
        {
            if ((fault & 0x01) != 0u)    /* If this Seq Ctl Pin faulted */
            {
                `$SeqBitsType` faultSlave = 1u;
                faultGroup = `$INSTANCE_NAME`_ctlGroupShutdownMask[ctlNum];
                `$INSTANCE_NAME`_faultCond      |= faultGroup;
                `$INSTANCE_NAME`_forceOffCmdReg |= faultGroup;
                for (sIdx = 0u; (sIdx < `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS) && (faultGroup != 0ul); sIdx++)
                {
                    if ((faultGroup & 0x01) != 0ul)
                    {
                        reseqCt = `$INSTANCE_NAME`_ctlFaultReseqCfg[sIdx];
                        offMode = ((reseqCt & `$INSTANCE_NAME`_SHUTDOWN_MODE) == 0u) ? 0u : 1u;
                        reseqCt &= `$INSTANCE_NAME`_RESEQ_CNT; /* Isolate count */

                        /* Process shutdown mode */
                        if (offMode == `$INSTANCE_NAME`_IMMEDIATE_OFF)
                        {
                            offBits |= faultSlave;  /* Immediate shutdown converter mask */
                        }
                        else /* Soft off */
                        {
                            `$INSTANCE_NAME`_forceOffModeReg |= faultSlave;
                        }
                        /* Update Resequence counters */
                        if (reseqCt < `$INSTANCE_NAME`_reseqCnt[sIdx])
                        {
                            `$INSTANCE_NAME`_reseqCnt[sIdx] = reseqCt;
                        }
                    }
                    faultGroup >>= 1u;
                    faultSlave <<= 1u;
                }
                for (groupNum = 0u; groupNum < `$INSTANCE_NAME`_NUMBER_OF_GROUPS; groupNum++)
                {
                    /* Disable all converters configured for immediate off mode */
                    *enRegList[groupNum] &= ~((uint8)(offBits));
                    #if(`$INSTANCE_NAME`_NUMBER_OF_GROUPS > 1u)
                        /* Go to next group bits */
                        offBits >>= `$INSTANCE_NAME`_GROUP_SIZE;
                    #endif /* `$INSTANCE_NAME`_NUMBER_OF_GROUPS > 1u */
                }
                ctlMask <<= 1u;
            }
            fault >>= 1u;
        }
    }
    #endif /* `$INSTANCE_NAME`_NUMBER_OF_CTL_INPUTS > 0u */

    #if(CY_PSOC3)
        EA = int_en;
    #endif /* CY_PSOC3 */
}


/* [] END OF FILE */
