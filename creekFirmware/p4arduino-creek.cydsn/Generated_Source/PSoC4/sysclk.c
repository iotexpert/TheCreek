/*******************************************************************************
* File Name: sysclk.c
* Version 1.0
*
* Description:
*  This file provides the source code to the API for the sysclk
*  component
*
* Note:
*  None
*
********************************************************************************
* Copyright 2013, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "sysclk.h"
#include "CyLib.h"

uint8 sysclk_initVar = 0u;


/*******************************************************************************
* Function Name: sysclk_Init
********************************************************************************
*
* Summary:
*  Initialize/Restore default sysclk configuration.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void sysclk_Init(void)
{

    /* Set values from customizer to CTRL */
    #if (sysclk__QUAD == sysclk_CONFIG)
        sysclk_CONTROL_REG =
        (((uint32)(sysclk_QUAD_ENCODING_MODES     << sysclk_QUAD_MODE_SHIFT))       |
         ((uint32)(sysclk_CONFIG                  << sysclk_MODE_SHIFT)));
    #endif  /* (sysclk__QUAD == sysclk_CONFIG) */

    #if (sysclk__PWM_SEL == sysclk_CONFIG)
        sysclk_CONTROL_REG =
        (((uint32)(sysclk_PWM_STOP_EVENT          << sysclk_PWM_STOP_KILL_SHIFT))    |
         ((uint32)(sysclk_PWM_OUT_INVERT          << sysclk_INV_OUT_SHIFT))         |
         ((uint32)(sysclk_PWM_OUT_N_INVERT        << sysclk_INV_COMPL_OUT_SHIFT))     |
         ((uint32)(sysclk_PWM_MODE                << sysclk_MODE_SHIFT)));

        #if (sysclk__PWM_PR == sysclk_PWM_MODE)
            sysclk_CONTROL_REG |=
            ((uint32)(sysclk_PWM_RUN_MODE         << sysclk_ONESHOT_SHIFT));

            sysclk_WriteCounter(sysclk_PWM_PR_INIT_VALUE);
        #else
            sysclk_CONTROL_REG |=
            (((uint32)(sysclk_PWM_ALIGN           << sysclk_UPDOWN_SHIFT))          |
             ((uint32)(sysclk_PWM_KILL_EVENT      << sysclk_PWM_SYNC_KILL_SHIFT)));
        #endif  /* (sysclk__PWM_PR == sysclk_PWM_MODE) */

        #if (sysclk__PWM_DT == sysclk_PWM_MODE)
            sysclk_CONTROL_REG |=
            ((uint32)(sysclk_PWM_DEAD_TIME_CYCLE  << sysclk_PRESCALER_SHIFT));
        #endif  /* (sysclk__PWM_DT == sysclk_PWM_MODE) */

        #if (sysclk__PWM == sysclk_PWM_MODE)
            sysclk_CONTROL_REG |=
            ((uint32)sysclk_PWM_PRESCALER         << sysclk_PRESCALER_SHIFT);
        #endif  /* (sysclk__PWM == sysclk_PWM_MODE) */
    #endif  /* (sysclk__PWM_SEL == sysclk_CONFIG) */

    #if (sysclk__TIMER == sysclk_CONFIG)
        sysclk_CONTROL_REG =
        (((uint32)(sysclk_TC_PRESCALER            << sysclk_PRESCALER_SHIFT))   |
         ((uint32)(sysclk_TC_COUNTER_MODE         << sysclk_UPDOWN_SHIFT))      |
         ((uint32)(sysclk_TC_RUN_MODE             << sysclk_ONESHOT_SHIFT))     |
         ((uint32)(sysclk_TC_COMP_CAP_MODE        << sysclk_MODE_SHIFT)));
    #endif  /* (sysclk__TIMER == sysclk_CONFIG) */

    /* Set values from customizer to CTRL1 */
    #if (sysclk__QUAD == sysclk_CONFIG)
        sysclk_TRIG_CONTROL1_REG  =
        (((uint32)(sysclk_QUAD_PHIA_SIGNAL_MODE   << sysclk_COUNT_SHIFT))       |
         ((uint32)(sysclk_QUAD_INDEX_SIGNAL_MODE  << sysclk_RELOAD_SHIFT))      |
         ((uint32)(sysclk_QUAD_STOP_SIGNAL_MODE   << sysclk_STOP_SHIFT))        |
         ((uint32)(sysclk_QUAD_PHIB_SIGNAL_MODE   << sysclk_START_SHIFT)));
    #endif  /* (sysclk__QUAD == sysclk_CONFIG) */

    #if (sysclk__PWM_SEL == sysclk_CONFIG)
        sysclk_TRIG_CONTROL1_REG  =
        (((uint32)(sysclk_PWM_SWITCH_SIGNAL_MODE  << sysclk_CAPTURE_SHIFT))     |
         ((uint32)(sysclk_PWM_COUNT_SIGNAL_MODE   << sysclk_COUNT_SHIFT))       |
         ((uint32)(sysclk_PWM_RELOAD_SIGNAL_MODE  << sysclk_RELOAD_SHIFT))      |
         ((uint32)(sysclk_PWM_STOP_SIGNAL_MODE    << sysclk_STOP_SHIFT))        |
         ((uint32)(sysclk_PWM_START_SIGNAL_MODE   << sysclk_START_SHIFT)));
    #endif  /* (sysclk__PWM_SEL == sysclk_CONFIG) */

    #if (sysclk__TIMER == sysclk_CONFIG)
        sysclk_TRIG_CONTROL1_REG  =
        (((uint32)(sysclk_TC_CAPTURE_SIGNAL_MODE  << sysclk_CAPTURE_SHIFT))     |
         ((uint32)(sysclk_TC_COUNT_SIGNAL_MODE    << sysclk_COUNT_SHIFT))       |
         ((uint32)(sysclk_TC_RELOAD_SIGNAL_MODE   << sysclk_RELOAD_SHIFT))      |
         ((uint32)(sysclk_TC_STOP_SIGNAL_MODE     << sysclk_STOP_SHIFT))        |
         ((uint32)(sysclk_TC_START_SIGNAL_MODE    << sysclk_START_SHIFT)));
    #endif  /* (sysclk__TIMER == sysclk_CONFIG) */

    /* Set values from customizer to INTR */
    #if (sysclk__QUAD == sysclk_CONFIG)
        sysclk_SetInterruptMode(sysclk_QUAD_INTERRUPT_MASK);
    #endif  /* (sysclk__QUAD == sysclk_CONFIG) */

    #if (sysclk__PWM_SEL == sysclk_CONFIG)
        sysclk_SetInterruptMode(sysclk_PWM_INTERRUPT_MASK);
    #endif  /* (sysclk__PWM_SEL == sysclk_CONFIG) */

    #if (sysclk__TIMER == sysclk_CONFIG)
        sysclk_SetInterruptMode(sysclk_TC_INTERRUPT_MASK);
    #endif  /* (sysclk__TIMER == sysclk_CONFIG) */

    /* Set other values from customizer */
    #if (sysclk__TIMER == sysclk_CONFIG)
        sysclk_WritePeriod(sysclk_TC_PERIOD_VALUE );
        #if (sysclk__COMPARE == sysclk_TC_COMP_CAP_MODE)
            sysclk_WriteCompare(sysclk_TC_COMPARE_VALUE);

            #if (1u == sysclk_TC_COMPARE_SWAP)
                sysclk_SetCompareSwap(1u);
                sysclk_WriteCompareBuf(sysclk_TC_COMPARE_BUF_VALUE);
            #endif  /* (1u == sysclk_TC_COMPARE_SWAP) */
        #endif  /* (sysclk__COMPARE == sysclk_TC_COMP_CAP_MODE) */
    #endif  /* (sysclk__TIMER == sysclk_CONFIG) */

    #if (sysclk__PWM_SEL == sysclk_CONFIG)
        sysclk_WritePeriod(sysclk_PWM_PERIOD_VALUE );
        sysclk_WriteCompare(sysclk_PWM_COMPARE_VALUE);

        #if (1u == sysclk_PWM_COMPARE_SWAP)
            sysclk_SetCompareSwap(1u);
            sysclk_WriteCompareBuf(sysclk_PWM_COMPARE_BUF_VALUE);
        #endif  /* (1u == sysclk_PWM_COMPARE_SWAP) */

        #if (1u == sysclk_PWM_PERIOD_SWAP)
            sysclk_SetPeriodSwap(1u);
            sysclk_WritePeriodBuf(sysclk_PWM_PERIOD_BUF_VALUE);
        #endif  /* (1u == sysclk_PWM_PERIOD_SWAP) */

        /* Set values from customizer to CTRL2 */
        #if (sysclk__PWM_PR == sysclk_PWM_MODE)
            sysclk_TRIG_CONTROL2_REG =
                    (sysclk_CC_MATCH_NO_CHANGE    |
                    sysclk_OVERLOW_NO_CHANGE      |
                    sysclk_UNDERFLOW_NO_CHANGE);
        #else
            #if (sysclk__LEFT == sysclk_PWM_ALIGN)
                sysclk_TRIG_CONTROL2_REG = sysclk_PWM_MODE_LEFT;
            #endif  /* ( sysclk_PWM_LEFT == sysclk_PWM_ALIGN) */

            #if (sysclk__RIGHT == sysclk_PWM_ALIGN)
                sysclk_TRIG_CONTROL2_REG = sysclk_PWM_MODE_RIGHT;
            #endif  /* ( sysclk_PWM_RIGHT == sysclk_PWM_ALIGN) */

            #if (sysclk__CENTER == sysclk_PWM_ALIGN)
                sysclk_TRIG_CONTROL2_REG = sysclk_PWM_MODE_CENTER;
            #endif  /* ( sysclk_PWM_CENTER == sysclk_PWM_ALIGN) */

            #if (sysclk__ASYMMETRIC == sysclk_PWM_ALIGN)
                sysclk_TRIG_CONTROL2_REG = sysclk_PWM_MODE_ASYM;
            #endif  /* (sysclk__ASYMMETRIC == sysclk_PWM_ALIGN) */
        #endif  /* (sysclk__PWM_PR == sysclk_PWM_MODE) */
    #endif  /* (sysclk__PWM_SEL == sysclk_CONFIG) */
}


/*******************************************************************************
* Function Name: sysclk_Enable
********************************************************************************
*
* Summary:
*  Enables the sysclk.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void sysclk_Enable(void)
{
    uint8 enableInterrupts;

    enableInterrupts = CyEnterCriticalSection();
    sysclk_BLOCK_CONTROL_REG |= sysclk_MASK;
    CyExitCriticalSection(enableInterrupts);

    /* Statr Timer or PWM if start input is absent */
    #if (sysclk__PWM_SEL == sysclk_CONFIG)
        #if (0u == sysclk_PWM_START_SIGNAL_PRESENT)
            sysclk_TriggerCommand(sysclk_MASK, sysclk_CMD_START);
        #endif /* (0u == sysclk_PWM_START_SIGNAL_PRESENT) */
    #endif /* (sysclk__PWM_SEL == sysclk_CONFIG) */

    #if (sysclk__TIMER == sysclk_CONFIG)
        #if (0u == sysclk_TC_START_SIGNAL_PRESENT)
            sysclk_TriggerCommand(sysclk_MASK, sysclk_CMD_START);
        #endif /* (0u == sysclk_TC_START_SIGNAL_PRESENT) */
    #endif /* (sysclk__TIMER == sysclk_CONFIG) */
}


/*******************************************************************************
* Function Name: sysclk_Start
********************************************************************************
*
* Summary:
*  Initialize the sysclk with default customizer
*  values when called the first time and enables the sysclk.
*  For subsequent calls the configuration is left unchanged and the component is
*  just enabled.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global variables:
*  sysclk_initVar: global variable is used to indicate initial
*  configuration of this component.  The variable is initialized to zero and set
*  to 1 the first time sysclk_Start() is called. This allows
*  enable/disable component without re-initialization in all subsequent calls
*  to the sysclk_Start() routine.
*
*******************************************************************************/
void sysclk_Start(void)
{
    if (0u == sysclk_initVar)
    {
        sysclk_Init();
        sysclk_initVar = 1u;
    }

    sysclk_Enable();
}


/*******************************************************************************
* Function Name: sysclk_Stop
********************************************************************************
*
* Summary:
*  Disables the sysclk.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void sysclk_Stop(void)
{
    uint8 enableInterrupts;

    enableInterrupts = CyEnterCriticalSection();

    sysclk_BLOCK_CONTROL_REG &= (uint32)~sysclk_MASK;

    CyExitCriticalSection(enableInterrupts);
}


/*******************************************************************************
* Function Name: sysclk_SetMode
********************************************************************************
*
* Summary:
*  Sets the operation mode of the sysclk. This function is used when
*  configured as a generic sysclk and the actual mode of operation is
*  set at runtime. The mode must be set while the component is disabled.
*
* Parameters:
*  mode: Mode for the sysclk to operate in
*   Values:
*   - sysclk_MODE_TIMER_COMPARE - Timer / Counter with
*                                                 compare capability
*         - sysclk_MODE_TIMER_CAPTURE - Timer / Counter with
*                                                 capture capability
*         - sysclk_MODE_QUAD - Quadrature decoder
*         - sysclk_MODE_PWM - PWM
*         - sysclk_MODE_PWM_DT - PWM with dead time
*         - sysclk_MODE_PWM_PR - PWM with pseudo random capability
*
* Return:
*  None
*
*******************************************************************************/
void sysclk_SetMode(uint32 mode)
{
    uint8 enableInterrupts;

    enableInterrupts = CyEnterCriticalSection();

    sysclk_CONTROL_REG &= (uint32)~sysclk_MODE_MASK;
    sysclk_CONTROL_REG |= mode;

    CyExitCriticalSection(enableInterrupts);
}


/*******************************************************************************
* Function Name: sysclk_SetQDMode
********************************************************************************
*
* Summary:
*  Sets the the Quadrature Decoder to one of 3 supported modes.
*  Is functionality is only applicable to Quadrature Decoder operation.
*
* Parameters:
*  qdMode: Quadrature Decoder mode
*   Values:
*         - sysclk_MODE_X1 - Counts on phi 1 rising
*         - sysclk_MODE_X2 - Counts on both edges of phi1 (2x faster)
*         - sysclk_MODE_X4 - Counts on both edges of phi1 and phi2
*                                        (4x faster)
*
* Return:
*  None
*
*******************************************************************************/
void sysclk_SetQDMode(uint32 qdMode)
{
    uint8 enableInterrupts;

    enableInterrupts = CyEnterCriticalSection();

    sysclk_CONTROL_REG &= (uint32)~sysclk_QUAD_MODE_MASK;
    sysclk_CONTROL_REG |= qdMode;

    CyExitCriticalSection(enableInterrupts);
}


/*******************************************************************************
* Function Name: sysclk_SetPrescaler
********************************************************************************
*
* Summary:
*  Sets the prescaler value that is applied to the clock input.  Not applicable
*  to PWM with dead time mode or Quadrature Decoder mode.
*
* Parameters:
*  prescaler: Prescaler divider value
*   Values:
*         - sysclk_PRESCALE_DIVBY1    - Divide by 1 (no prescaling)
*         - sysclk_PRESCALE_DIVBY2    - Divide by 2
*         - sysclk_PRESCALE_DIVBY4    - Divide by 4
*         - sysclk_PRESCALE_DIVBY8    - Divide by 8
*         - sysclk_PRESCALE_DIVBY16   - Divide by 16
*         - sysclk_PRESCALE_DIVBY32   - Divide by 32
*         - sysclk_PRESCALE_DIVBY64   - Divide by 64
*         - sysclk_PRESCALE_DIVBY128  - Divide by 128
*
* Return:
*  None
*
*******************************************************************************/
void sysclk_SetPrescaler(uint32 prescaler)
{
    uint8 enableInterrupts;

    enableInterrupts = CyEnterCriticalSection();

    sysclk_CONTROL_REG &= (uint32)~sysclk_PRESCALER_MASK;
    sysclk_CONTROL_REG |= prescaler;

    CyExitCriticalSection(enableInterrupts);
}


/*******************************************************************************
* Function Name: sysclk_SetOneShot
********************************************************************************
*
* Summary:
*  Writes the register that controls whether the sysclk runs
*  continuously or stops when terminal count is reached.  By default the
*  sysclk operates in continuous mode.
*
* Parameters:
*  oneShotEnable
*   Values:
*     - 0 - Continuous
*     - 1 - Enable One Shot
*
* Return:
*  None
*
*******************************************************************************/
void sysclk_SetOneShot(uint32 oneShotEnable)
{
    uint8 enableInterrupts;

    enableInterrupts = CyEnterCriticalSection();

    sysclk_CONTROL_REG &= (uint32)~sysclk_ONESHOT_MASK;
    sysclk_CONTROL_REG |= ((uint32)((oneShotEnable & sysclk_1BIT_MASK) <<
                                                               sysclk_ONESHOT_SHIFT));

    CyExitCriticalSection(enableInterrupts);
}


/*******************************************************************************
* Function Name: sysclk_SetPWMMode
********************************************************************************
*
* Summary:
*  Writes the control register that determines what mode of operation the PWM
*  output lines are driven in.  There is a setting for what to do on a
*  comparison match (CC_MATCH), on an overflow (OVERFLOW) and on an underflow
*  (UNDERFLOW).  The value for each of the 3 must be ORed together to form the
*  mode.
*
* Parameters:
*  modeMask: Combination of the 3 mode settings.  Mask must include a value for
*  each of the 3 or use one of the preconfigured PWM settings.
*   Values:
*     - CC_MATCH_SET        - Set on comparison match
*     - CC_MATCH_CLEAR      - Clear on comparison match
*     - CC_MATCH_INVERT     - Invert on comparison match
*     - CC_MATCH_NO_CHANGE  - No change on comparison match
*     - OVERLOW_SET         - Set on overflow
*     - OVERLOW_CLEAR       - Clear on overflow
*     - OVERLOW_INVERT      - Invert on overflow
*     - OVERLOW_NO_CHANGE   - No change on overflow
*     - UNDERFLOW_SET       - Set on underflow
*     - UNDERFLOW_CLEAR     - Clear on underflow
*     - UNDERFLOW_INVERT    - Invert on underflow
*     - UNDERFLOW_NO_CHANGE - No change on underflow
*     - PWM_MODE_LEFT       - Setting for left aligned PWM.  Should be combined
*                             with up counting mode
*     - PWM_MODE_RIGHT      - Setting for right aligned PWM.  Should be combined
*                             with down counting mode
*     - PWM_MODE_CENTER     - Setting for center aligned PWM.  Should be 
*                             combined with up/down 0 mode
*     - PWM_MODE_ASYM       - Setting for asymmetric PWM.  Should be combined
*                             with up/down 1 mode
*
* Return:
*  None
*
*******************************************************************************/
void sysclk_SetPWMMode(uint32 modeMask)
{
    sysclk_TRIG_CONTROL2_REG = (modeMask & sysclk_6BIT_MASK);
}



/*******************************************************************************
* Function Name: sysclk_SetPWMSyncKill
********************************************************************************
*
* Summary:
*  Writes the register that controls whether the PWM kill signal (stop input)
*  causes an asynchronous or synchronous kill operation.  By default the kill
*  operation is asynchronous.  This functionality is only applicable to PWM and
*  PWM with dead time modes.
*
*  For Synchronous mode the kill signal disables both the line and line_n
*  signals until the next terminal count.
*
*  For Asynchronous mode the kill signal disables both the line and line_n
*  signals when the kill signal is present.  This mode should only be used
*  when the kill signal (stop input) is configured in pass through mode
*  (Level sensitive signal).

*
* Parameters:
*  syncKillEnable
*   Values:
*     - 0 - Asynchronous
*     - 1 - Synchronous
*
* Return:
*  None
*
*******************************************************************************/
void sysclk_SetPWMSyncKill(uint32 syncKillEnable)
{
    uint8 enableInterrupts;

    enableInterrupts = CyEnterCriticalSection();

    sysclk_CONTROL_REG &= (uint32)~sysclk_PWM_SYNC_KILL_MASK;
    sysclk_CONTROL_REG |= ((uint32)((syncKillEnable & sysclk_1BIT_MASK)  <<
                                               sysclk_PWM_SYNC_KILL_SHIFT));

    CyExitCriticalSection(enableInterrupts);
}


/*******************************************************************************
* Function Name: sysclk_SetPWMStopOnKill
********************************************************************************
*
* Summary:
*  Writes the register that controls whether the PWM kill signal (stop input)
*  causes the PWM counter to stop.  By default the kill operation does not stop
*  the counter.  This functionality is only applicable to the three PWM modes.
*
*
* Parameters:
*  stopOnKillEnable
*   Values:
*     - 0 - Don't stop
*     - 1 - Stop
*
* Return:
*  None
*
*******************************************************************************/
void sysclk_SetPWMStopOnKill(uint32 stopOnKillEnable)
{
    uint8 enableInterrupts;

    enableInterrupts = CyEnterCriticalSection();

    sysclk_CONTROL_REG &= (uint32)~sysclk_PWM_STOP_KILL_MASK;
    sysclk_CONTROL_REG |= ((uint32)((stopOnKillEnable & sysclk_1BIT_MASK)  <<
                                                         sysclk_PWM_STOP_KILL_SHIFT));

    CyExitCriticalSection(enableInterrupts);
}


/*******************************************************************************
* Function Name: sysclk_SetPWMDeadTime
********************************************************************************
*
* Summary:
*  Writes the dead time control value.  This value delays the rising edge of
*  both the line and line_n signals the designated number of cycles resulting
*  in both signals being inactive for that many cycles.  This functionality is
*  only applicable to the PWM in dead time mode.

*
* Parameters:
*  Dead time to insert
*   Values: 0 to 255
*
* Return:
*  None
*
*******************************************************************************/
void sysclk_SetPWMDeadTime(uint32 deadTime)
{
    uint8 enableInterrupts;

    enableInterrupts = CyEnterCriticalSection();

    sysclk_CONTROL_REG &= (uint32)~sysclk_PRESCALER_MASK;
    sysclk_CONTROL_REG |= ((uint32)((deadTime & sysclk_8BIT_MASK) <<
                                                          sysclk_PRESCALER_SHIFT));

    CyExitCriticalSection(enableInterrupts);
}


/*******************************************************************************
* Function Name: sysclk_SetPWMInvert
********************************************************************************
*
* Summary:
*  Writes the bits that control whether the line and line_n outputs are
*  inverted from their normal output values.  This functionality is only
*  applicable to the three PWM modes.
*
* Parameters:
*  mask: Mask of outputs to invert.
*   Values:
*         - sysclk_INVERT_LINE   - Inverts the line output
*         - sysclk_INVERT_LINE_N - Inverts the line_n output
*
* Return:
*  None
*
*******************************************************************************/
void sysclk_SetPWMInvert(uint32 mask)
{
    uint8 enableInterrupts;

    enableInterrupts = CyEnterCriticalSection();

    sysclk_CONTROL_REG &= (uint32)~sysclk_INV_OUT_MASK;
    sysclk_CONTROL_REG |= mask;

    CyExitCriticalSection(enableInterrupts);
}



/*******************************************************************************
* Function Name: sysclk_WriteCounter
********************************************************************************
*
* Summary:
*  Writes a new 16bit counter value directly into the counter register, thus
*  setting the counter (not the period) to the value written. It is not
*  advised to write to this field when the counter is running.
*
* Parameters:
*  count: value to write
*
* Return:
*  None
*
*******************************************************************************/
void sysclk_WriteCounter(uint32 count)
{
    sysclk_COUNTER_REG = (count & sysclk_16BIT_MASK);
}


/*******************************************************************************
* Function Name: sysclk_ReadCounter
********************************************************************************
*
* Summary:
*  Reads the current counter value.
*
* Parameters:
*  None
*
* Return:
*  Current counter value
*
*******************************************************************************/
uint32 sysclk_ReadCounter(void)
{
    return (sysclk_COUNTER_REG & sysclk_16BIT_MASK);
}


/*******************************************************************************
* Function Name: sysclk_SetCounterMode
********************************************************************************
*
* Summary:
*  Sets the counter mode.  Applicable to all modes except Quadrature Decoder
*  and PWM with pseudo random output.
*
* Parameters:
*  counterMode: Enumerated couner type values
*   Values:
*     - sysclk_COUNT_UP       - Counts up
*     - sysclk_COUNT_DOWN     - Counts down
*     - sysclk_COUNT_UPDOWN0  - Counts up and down. Terminal count
*                                         generated when counter reaches 0
*     - sysclk_COUNT_UPDOWN1  - Counts up and down. Terminal count
*                                         generated both when counter reaches 0
*                                         and period
*
* Return:
*  None
*
*******************************************************************************/
void sysclk_SetCounterMode(uint32 counterMode)
{
    uint8 enableInterrupts;

    enableInterrupts = CyEnterCriticalSection();

    sysclk_CONTROL_REG &= (uint32)~sysclk_UPDOWN_MASK;
    sysclk_CONTROL_REG |= counterMode;

    CyExitCriticalSection(enableInterrupts);
}


/*******************************************************************************
* Function Name: sysclk_WritePeriod
********************************************************************************
*
* Summary:
*  Writes the 16 bit period register with the new period value.
*  To cause the counter to count for N cycles this register should be written
*  with N-1 (counts from 0 to period inclusive).
*
* Parameters:
*  period: Period value
*
* Return:
*  None
*
*******************************************************************************/
void sysclk_WritePeriod(uint32 period)
{
    sysclk_PERIOD_REG = (period & sysclk_16BIT_MASK);
}


/*******************************************************************************
* Function Name: sysclk_ReadPeriod
********************************************************************************
*
* Summary:
*  Reads the 16 bit period register.
*
* Parameters:
*  None
*
* Return:
*  Period value
*
*******************************************************************************/
uint32 sysclk_ReadPeriod(void)
{
    return (sysclk_PERIOD_REG & sysclk_16BIT_MASK);
}


/*******************************************************************************
* Function Name: sysclk_SetCompareSwap
********************************************************************************
*
* Summary:
*  Writes the register that controls whether the compare registers are
*  swapped. When enabled in Timer/Counter mode(without capture) the swap occurs
*  at a TC event. In PWM mode the swap occurs at the next TC event following
*  a hardware switch event.
*
* Parameters:
*  swapEnable
*   Values:
*     - 0 - Disable swap
*     - 1 - Enable swap
*
* Return:
*  None
*
*******************************************************************************/
void sysclk_SetCompareSwap(uint32 swapEnable)
{
    uint8 enableInterrupts;

    enableInterrupts = CyEnterCriticalSection();

    sysclk_CONTROL_REG &= (uint32)~sysclk_RELOAD_CC_MASK;
    sysclk_CONTROL_REG |= (swapEnable & sysclk_1BIT_MASK);

    CyExitCriticalSection(enableInterrupts);
}


/*******************************************************************************
* Function Name: sysclk_WritePeriodBuf
********************************************************************************
*
* Summary:
*  Writes the 16 bit period buf register with the new period value.
*
* Parameters:
*  periodBuf: Period value
*
* Return:
*  None
*
*******************************************************************************/
void sysclk_WritePeriodBuf(uint32 periodBuf)
{
    sysclk_PERIOD_BUF_REG = (periodBuf & sysclk_16BIT_MASK);
}


/*******************************************************************************
* Function Name: sysclk_ReadPeriodBuf
********************************************************************************
*
* Summary:
*  Reads the 16 bit period buf register.
*
* Parameters:
*  None
*
* Return:
*  Period value
*
*******************************************************************************/
uint32 sysclk_ReadPeriodBuf(void)
{
    return (sysclk_PERIOD_BUF_REG & sysclk_16BIT_MASK);
}


/*******************************************************************************
* Function Name: sysclk_SetPeriodSwap
********************************************************************************
*
* Summary:
*  Writes the register that controls whether the period registers are
*  swapped. When enabled in Timer/Counter mode the swap occurs at a TC event.
*  In PWM mode the swap occurs at the next TC event following a hardware switch
*  event.
*
* Parameters:
*  swapEnable
*   Values:
*     - 0 - Disable swap
*     - 1 - Enable swap
*
* Return:
*  None
*
*******************************************************************************/
void sysclk_SetPeriodSwap(uint32 swapEnable)
{
    uint8 enableInterrupts;

    enableInterrupts = CyEnterCriticalSection();

    sysclk_CONTROL_REG &= (uint32)~sysclk_RELOAD_PERIOD_MASK;
    sysclk_CONTROL_REG |= ((uint32)((swapEnable & sysclk_1BIT_MASK) <<
                                                            sysclk_RELOAD_PERIOD_SHIFT));

    CyExitCriticalSection(enableInterrupts);
}


/*******************************************************************************
* Function Name: sysclk_WriteCompare
********************************************************************************
*
* Summary:
*  Writes the 16 bit compare register with the new compare value. Not
*  applicable for Timer/Counter with Capture or in Quadrature Decoder modes.
*
* Parameters:
*  compare: Compare value
*
* Return:
*  None
*
*******************************************************************************/
void sysclk_WriteCompare(uint32 compare)
{
    sysclk_COMP_CAP_REG = (compare & sysclk_16BIT_MASK);
}


/*******************************************************************************
* Function Name: sysclk_ReadCompare
********************************************************************************
*
* Summary:
*  Reads the compare register. Not applicable for Timer/Counter with Capture
*  or in Quadrature Decoder modes.
*
* Parameters:
*  None
*
* Return:
*  Compare value
*
*******************************************************************************/
uint32 sysclk_ReadCompare(void)
{
    return (sysclk_COMP_CAP_REG & sysclk_16BIT_MASK);
}


/*******************************************************************************
* Function Name: sysclk_WriteCompareBuf
********************************************************************************
*
* Summary:
*  Writes the 16 bit compare buffer register with the new compare value. Not
*  applicable for Timer/Counter with Capture or in Quadrature Decoder modes.
*
* Parameters:
*  compareBuf: Compare value
*
* Return:
*  None
*
*******************************************************************************/
void sysclk_WriteCompareBuf(uint32 compareBuf)
{
   sysclk_COMP_CAP_BUF_REG = (compareBuf & sysclk_16BIT_MASK);
}


/*******************************************************************************
* Function Name: sysclk_ReadCompareBuf
********************************************************************************
*
* Summary:
*  Reads the compare buffer register. Not applicable for Timer/Counter with
*  Capture or in Quadrature Decoder modes.
*
* Parameters:
*  None
*
* Return:
*  Compare buffer value
*
*******************************************************************************/
uint32 sysclk_ReadCompareBuf(void)
{
    return (sysclk_COMP_CAP_BUF_REG & sysclk_16BIT_MASK);
}


/*******************************************************************************
* Function Name: sysclk_ReadCapture
********************************************************************************
*
* Summary:
*  Reads the captured counter value. This API is applicable only for
*  Timer/Counter with capture mode and Quadrature Decoder modes.
*
* Parameters:
*  None
*
* Return:
*  Capture value
*
*******************************************************************************/
uint32 sysclk_ReadCapture(void)
{
    return (sysclk_COMP_CAP_REG & sysclk_16BIT_MASK);
}


/*******************************************************************************
* Function Name: sysclk_ReadCaptureBuf
********************************************************************************
*
* Summary:
*  Reads the capture buffer register. This API is applicable only for
*  Timer/Counter with capture mode and Quadrature Decoder modes.
*
* Parameters:
*  None
*
* Return:
*  Capture buffer value
*
*******************************************************************************/
uint32 sysclk_ReadCaptureBuf(void)
{
    return (sysclk_COMP_CAP_BUF_REG & sysclk_16BIT_MASK);
}


/*******************************************************************************
* Function Name: sysclk_SetCaptureMode
********************************************************************************
*
* Summary:
*  Sets the capture trigger mode. For PWM mode this is the switch input.
*  This input is not applicable to the Timer/Counter without Capture and
*  Quadrature Decoder modes.
*
* Parameters:
*  triggerMode: Enumerated trigger mode value
*   Values:
*     - sysclk_TRIG_LEVEL     - Level
*     - sysclk_TRIG_RISING    - Rising edge
*     - sysclk_TRIG_FALLING   - Falling edge
*     - sysclk_TRIG_BOTH      - Both rising and falling edge
*
* Return:
*  None
*
*******************************************************************************/
void sysclk_SetCaptureMode(uint32 triggerMode)
{
    uint8 enableInterrupts;

    enableInterrupts = CyEnterCriticalSection();

    sysclk_TRIG_CONTROL1_REG &= (uint32)~sysclk_CAPTURE_MASK;
    sysclk_TRIG_CONTROL1_REG |= triggerMode;

    CyExitCriticalSection(enableInterrupts);
}


/*******************************************************************************
* Function Name: sysclk_SetReloadMode
********************************************************************************
*
* Summary:
*  Sets the reload trigger mode. For Quadrature Decoder mode this is the index
*  input.
*
* Parameters:
*  triggerMode: Enumerated trigger mode value
*   Values:
*     - sysclk_TRIG_LEVEL     - Level
*     - sysclk_TRIG_RISING    - Rising edge
*     - sysclk_TRIG_FALLING   - Falling edge
*     - sysclk_TRIG_BOTH      - Both rising and falling edge
*
* Return:
*  None
*
*******************************************************************************/
void sysclk_SetReloadMode(uint32 triggerMode)
{
    uint8 enableInterrupts;

    enableInterrupts = CyEnterCriticalSection();

    sysclk_TRIG_CONTROL1_REG &= (uint32)~sysclk_RELOAD_MASK;
    sysclk_TRIG_CONTROL1_REG |= ((uint32)(triggerMode << sysclk_RELOAD_SHIFT));

    CyExitCriticalSection(enableInterrupts);
}


/*******************************************************************************
* Function Name: sysclk_SetStartMode
********************************************************************************
*
* Summary:
*  Sets the start trigger mode. For Quadrature Decoder mode this is the
*  phiB input.
*
* Parameters:
*  triggerMode: Enumerated trigger mode value
*   Values:
*     - sysclk_TRIG_LEVEL     - Level
*     - sysclk_TRIG_RISING    - Rising edge
*     - sysclk_TRIG_FALLING   - Falling edge
*     - sysclk_TRIG_BOTH      - Both rising and falling edge
*
* Return:
*  None
*
*******************************************************************************/
void sysclk_SetStartMode(uint32 triggerMode)
{
    uint8 enableInterrupts;

    enableInterrupts = CyEnterCriticalSection();

    sysclk_TRIG_CONTROL1_REG &= (uint32)~sysclk_START_MASK;
    sysclk_TRIG_CONTROL1_REG |= ((uint32)(triggerMode << sysclk_START_SHIFT));

    CyExitCriticalSection(enableInterrupts);
}


/*******************************************************************************
* Function Name: sysclk_SetStopMode
********************************************************************************
*
* Summary:
*  Sets the stop trigger mode. For PWM mode this is the kill input.
*
* Parameters:
*  triggerMode: Enumerated trigger mode value
*   Values:
*     - sysclk_TRIG_LEVEL     - Level
*     - sysclk_TRIG_RISING    - Rising edge
*     - sysclk_TRIG_FALLING   - Falling edge
*     - sysclk_TRIG_BOTH      - Both rising and falling edge
*
* Return:
*  None
*
*******************************************************************************/
void sysclk_SetStopMode(uint32 triggerMode)
{
    uint8 enableInterrupts;

    enableInterrupts = CyEnterCriticalSection();

    sysclk_TRIG_CONTROL1_REG &= (uint32)~sysclk_STOP_MASK;
    sysclk_TRIG_CONTROL1_REG |= ((uint32)(triggerMode << sysclk_STOP_SHIFT));

    CyExitCriticalSection(enableInterrupts);
}


/*******************************************************************************
* Function Name: sysclk_SetCountMode
********************************************************************************
*
* Summary:
*  Sets the count trigger mode. For Quadrature Decoder mode this is the phiA
*  input.
*
* Parameters:
*  triggerMode: Enumerated trigger mode value
*   Values:
*     - sysclk_TRIG_LEVEL     - Level
*     - sysclk_TRIG_RISING    - Rising edge
*     - sysclk_TRIG_FALLING   - Falling edge
*     - sysclk_TRIG_BOTH      - Both rising and falling edge
*
* Return:
*  None
*
*******************************************************************************/
void sysclk_SetCountMode(uint32 triggerMode)
{
    uint8 enableInterrupts;

    enableInterrupts = CyEnterCriticalSection();

    sysclk_TRIG_CONTROL1_REG &= (uint32)~sysclk_COUNT_MASK;
    sysclk_TRIG_CONTROL1_REG |= ((uint32)(triggerMode << sysclk_COUNT_SHIFT));

    CyExitCriticalSection(enableInterrupts);
}


/*******************************************************************************
* Function Name: sysclk_TriggerCommand
********************************************************************************
*
* Summary:
*  Triggers the designated command to occur on the designated TCPWM instances.
*  The mask can be used to apply this command simultaneously to more than one
*  instance.  This allows multiple TCPWM instances to be synchronized.
*
* Parameters:
*  mask: Combination of mask bits for each instance of the TCPWM that the
*        command should apply to.  This function from one instance can be used
*        to apply the command to any of the instances in the design.
*        The mask value for a specific instance is available with the MASK
*        define.
*  command: Enumerated command values. Capture command only applicable for
*           Timer/Counter with Capture and PWM modes.
*   Values:
*     - sysclk_CMD_CAPTURE    - Trigger Capture command
*     - sysclk_CMD_RELOAD     - Trigger Reload command
*     - sysclk_CMD_STOP       - Trigger Stop command
*     - sysclk_CMD_START      - Trigger Start command
*
* Return:
*  None
*
*******************************************************************************/
void sysclk_TriggerCommand(uint32 mask, uint32 command)
{
    uint8 enableInterrupts;

    enableInterrupts = CyEnterCriticalSection();

    sysclk_COMMAND_REG = ((uint32)(mask << command));

    CyExitCriticalSection(enableInterrupts);
}


/*******************************************************************************
* Function Name: sysclk_ReadStatus
********************************************************************************
*
* Summary:
*  Reads the status of the sysclk.
*
* Parameters:
*  None
*
* Return:
*  Status
*   Values:
*     - sysclk_STATUS_DOWN    - Set if counting down
*     - sysclk_STATUS_RUNNING - Set if counter is running
*
*******************************************************************************/
uint32 sysclk_ReadStatus(void)
{
    return ((sysclk_STATUS_REG >> sysclk_RUNNING_STATUS_SHIFT) |
            (sysclk_STATUS_REG & sysclk_STATUS_DOWN));
}


/*******************************************************************************
* Function Name: sysclk_SetInterruptMode
********************************************************************************
*
* Summary:
*  Sets the interrupt mask to control which interrupt
*  requests generate the interrupt signal.
*
* Parameters:
*   interruptMask: Mask of bits to be enabled
*   Values:
*     - sysclk_INTR_MASK_TC       - Terminal count mask
*     - sysclk_INTR_MASK_CC_MATCH - Compare count / capture mask
*
* Return:
*  None
*
*******************************************************************************/
void sysclk_SetInterruptMode(uint32 interruptMask)
{
    sysclk_INTERRUPT_MASK_REG =  interruptMask;
}


/*******************************************************************************
* Function Name: sysclk_GetInterruptSourceMasked
********************************************************************************
*
* Summary:
*  Gets the interrupt requests masked by the interrupt mask.
*
* Parameters:
*   None
*
* Return:
*  Masked interrupt source
*   Values:
*     - sysclk_INTR_MASK_TC       - Terminal count mask
*     - sysclk_INTR_MASK_CC_MATCH - Compare count / capture mask
*
*******************************************************************************/
uint32 sysclk_GetInterruptSourceMasked(void)
{
    return (sysclk_INTERRUPT_MASKED_REG);
}


/*******************************************************************************
* Function Name: sysclk_GetInterruptSource
********************************************************************************
*
* Summary:
*  Gets the interrupt requests (without masking).
*
* Parameters:
*  None
*
* Return:
*  Interrupt request value
*   Values:
*     - sysclk_INTR_MASK_TC       - Terminal count mask
*     - sysclk_INTR_MASK_CC_MATCH - Compare count / capture mask
*
*******************************************************************************/
uint32 sysclk_GetInterruptSource(void)
{
    return (sysclk_INTERRUPT_REQ_REG);
}


/*******************************************************************************
* Function Name: sysclk_ClearInterrupt
********************************************************************************
*
* Summary:
*  Clears the interrupt request.
*
* Parameters:
*   interruptMask: Mask of interrupts to clear
*   Values:
*     - sysclk_INTR_MASK_TC       - Terminal count mask
*     - sysclk_INTR_MASK_CC_MATCH - Compare count / capture mask
*
* Return:
*  None
*
*******************************************************************************/
void sysclk_ClearInterrupt(uint32 interruptMask)
{
    sysclk_INTERRUPT_REQ_REG = interruptMask;
}


/*******************************************************************************
* Function Name: sysclk_SetInterrupt
********************************************************************************
*
* Summary:
*  Sets a software interrupt request.
*
* Parameters:
*   interruptMask: Mask of interrupts to set
*   Values:
*     - sysclk_INTR_MASK_TC       - Terminal count mask
*     - sysclk_INTR_MASK_CC_MATCH - Compare count / capture mask
*
* Return:
*  None
*
*******************************************************************************/
void sysclk_SetInterrupt(uint32 interruptMask)
{
    sysclk_INTERRUPT_SET_REG = interruptMask;
}


/* [] END OF FILE */
