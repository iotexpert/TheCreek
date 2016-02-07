/*******************************************************************************
* File Name: timer.c
* Version 1.0
*
* Description:
*  This file provides the source code to the API for the timer
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

#include "timer.h"
#include "CyLib.h"

uint8 timer_initVar = 0u;


/*******************************************************************************
* Function Name: timer_Init
********************************************************************************
*
* Summary:
*  Initialize/Restore default timer configuration.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void timer_Init(void)
{

    /* Set values from customizer to CTRL */
    #if (timer__QUAD == timer_CONFIG)
        timer_CONTROL_REG =
        (((uint32)(timer_QUAD_ENCODING_MODES     << timer_QUAD_MODE_SHIFT))       |
         ((uint32)(timer_CONFIG                  << timer_MODE_SHIFT)));
    #endif  /* (timer__QUAD == timer_CONFIG) */

    #if (timer__PWM_SEL == timer_CONFIG)
        timer_CONTROL_REG =
        (((uint32)(timer_PWM_STOP_EVENT          << timer_PWM_STOP_KILL_SHIFT))    |
         ((uint32)(timer_PWM_OUT_INVERT          << timer_INV_OUT_SHIFT))         |
         ((uint32)(timer_PWM_OUT_N_INVERT        << timer_INV_COMPL_OUT_SHIFT))     |
         ((uint32)(timer_PWM_MODE                << timer_MODE_SHIFT)));

        #if (timer__PWM_PR == timer_PWM_MODE)
            timer_CONTROL_REG |=
            ((uint32)(timer_PWM_RUN_MODE         << timer_ONESHOT_SHIFT));

            timer_WriteCounter(timer_PWM_PR_INIT_VALUE);
        #else
            timer_CONTROL_REG |=
            (((uint32)(timer_PWM_ALIGN           << timer_UPDOWN_SHIFT))          |
             ((uint32)(timer_PWM_KILL_EVENT      << timer_PWM_SYNC_KILL_SHIFT)));
        #endif  /* (timer__PWM_PR == timer_PWM_MODE) */

        #if (timer__PWM_DT == timer_PWM_MODE)
            timer_CONTROL_REG |=
            ((uint32)(timer_PWM_DEAD_TIME_CYCLE  << timer_PRESCALER_SHIFT));
        #endif  /* (timer__PWM_DT == timer_PWM_MODE) */

        #if (timer__PWM == timer_PWM_MODE)
            timer_CONTROL_REG |=
            ((uint32)timer_PWM_PRESCALER         << timer_PRESCALER_SHIFT);
        #endif  /* (timer__PWM == timer_PWM_MODE) */
    #endif  /* (timer__PWM_SEL == timer_CONFIG) */

    #if (timer__TIMER == timer_CONFIG)
        timer_CONTROL_REG =
        (((uint32)(timer_TC_PRESCALER            << timer_PRESCALER_SHIFT))   |
         ((uint32)(timer_TC_COUNTER_MODE         << timer_UPDOWN_SHIFT))      |
         ((uint32)(timer_TC_RUN_MODE             << timer_ONESHOT_SHIFT))     |
         ((uint32)(timer_TC_COMP_CAP_MODE        << timer_MODE_SHIFT)));
    #endif  /* (timer__TIMER == timer_CONFIG) */

    /* Set values from customizer to CTRL1 */
    #if (timer__QUAD == timer_CONFIG)
        timer_TRIG_CONTROL1_REG  =
        (((uint32)(timer_QUAD_PHIA_SIGNAL_MODE   << timer_COUNT_SHIFT))       |
         ((uint32)(timer_QUAD_INDEX_SIGNAL_MODE  << timer_RELOAD_SHIFT))      |
         ((uint32)(timer_QUAD_STOP_SIGNAL_MODE   << timer_STOP_SHIFT))        |
         ((uint32)(timer_QUAD_PHIB_SIGNAL_MODE   << timer_START_SHIFT)));
    #endif  /* (timer__QUAD == timer_CONFIG) */

    #if (timer__PWM_SEL == timer_CONFIG)
        timer_TRIG_CONTROL1_REG  =
        (((uint32)(timer_PWM_SWITCH_SIGNAL_MODE  << timer_CAPTURE_SHIFT))     |
         ((uint32)(timer_PWM_COUNT_SIGNAL_MODE   << timer_COUNT_SHIFT))       |
         ((uint32)(timer_PWM_RELOAD_SIGNAL_MODE  << timer_RELOAD_SHIFT))      |
         ((uint32)(timer_PWM_STOP_SIGNAL_MODE    << timer_STOP_SHIFT))        |
         ((uint32)(timer_PWM_START_SIGNAL_MODE   << timer_START_SHIFT)));
    #endif  /* (timer__PWM_SEL == timer_CONFIG) */

    #if (timer__TIMER == timer_CONFIG)
        timer_TRIG_CONTROL1_REG  =
        (((uint32)(timer_TC_CAPTURE_SIGNAL_MODE  << timer_CAPTURE_SHIFT))     |
         ((uint32)(timer_TC_COUNT_SIGNAL_MODE    << timer_COUNT_SHIFT))       |
         ((uint32)(timer_TC_RELOAD_SIGNAL_MODE   << timer_RELOAD_SHIFT))      |
         ((uint32)(timer_TC_STOP_SIGNAL_MODE     << timer_STOP_SHIFT))        |
         ((uint32)(timer_TC_START_SIGNAL_MODE    << timer_START_SHIFT)));
    #endif  /* (timer__TIMER == timer_CONFIG) */

    /* Set values from customizer to INTR */
    #if (timer__QUAD == timer_CONFIG)
        timer_SetInterruptMode(timer_QUAD_INTERRUPT_MASK);
    #endif  /* (timer__QUAD == timer_CONFIG) */

    #if (timer__PWM_SEL == timer_CONFIG)
        timer_SetInterruptMode(timer_PWM_INTERRUPT_MASK);
    #endif  /* (timer__PWM_SEL == timer_CONFIG) */

    #if (timer__TIMER == timer_CONFIG)
        timer_SetInterruptMode(timer_TC_INTERRUPT_MASK);
    #endif  /* (timer__TIMER == timer_CONFIG) */

    /* Set other values from customizer */
    #if (timer__TIMER == timer_CONFIG)
        timer_WritePeriod(timer_TC_PERIOD_VALUE );
        #if (timer__COMPARE == timer_TC_COMP_CAP_MODE)
            timer_WriteCompare(timer_TC_COMPARE_VALUE);

            #if (1u == timer_TC_COMPARE_SWAP)
                timer_SetCompareSwap(1u);
                timer_WriteCompareBuf(timer_TC_COMPARE_BUF_VALUE);
            #endif  /* (1u == timer_TC_COMPARE_SWAP) */
        #endif  /* (timer__COMPARE == timer_TC_COMP_CAP_MODE) */
    #endif  /* (timer__TIMER == timer_CONFIG) */

    #if (timer__PWM_SEL == timer_CONFIG)
        timer_WritePeriod(timer_PWM_PERIOD_VALUE );
        timer_WriteCompare(timer_PWM_COMPARE_VALUE);

        #if (1u == timer_PWM_COMPARE_SWAP)
            timer_SetCompareSwap(1u);
            timer_WriteCompareBuf(timer_PWM_COMPARE_BUF_VALUE);
        #endif  /* (1u == timer_PWM_COMPARE_SWAP) */

        #if (1u == timer_PWM_PERIOD_SWAP)
            timer_SetPeriodSwap(1u);
            timer_WritePeriodBuf(timer_PWM_PERIOD_BUF_VALUE);
        #endif  /* (1u == timer_PWM_PERIOD_SWAP) */

        /* Set values from customizer to CTRL2 */
        #if (timer__PWM_PR == timer_PWM_MODE)
            timer_TRIG_CONTROL2_REG =
                    (timer_CC_MATCH_NO_CHANGE    |
                    timer_OVERLOW_NO_CHANGE      |
                    timer_UNDERFLOW_NO_CHANGE);
        #else
            #if (timer__LEFT == timer_PWM_ALIGN)
                timer_TRIG_CONTROL2_REG = timer_PWM_MODE_LEFT;
            #endif  /* ( timer_PWM_LEFT == timer_PWM_ALIGN) */

            #if (timer__RIGHT == timer_PWM_ALIGN)
                timer_TRIG_CONTROL2_REG = timer_PWM_MODE_RIGHT;
            #endif  /* ( timer_PWM_RIGHT == timer_PWM_ALIGN) */

            #if (timer__CENTER == timer_PWM_ALIGN)
                timer_TRIG_CONTROL2_REG = timer_PWM_MODE_CENTER;
            #endif  /* ( timer_PWM_CENTER == timer_PWM_ALIGN) */

            #if (timer__ASYMMETRIC == timer_PWM_ALIGN)
                timer_TRIG_CONTROL2_REG = timer_PWM_MODE_ASYM;
            #endif  /* (timer__ASYMMETRIC == timer_PWM_ALIGN) */
        #endif  /* (timer__PWM_PR == timer_PWM_MODE) */
    #endif  /* (timer__PWM_SEL == timer_CONFIG) */
}


/*******************************************************************************
* Function Name: timer_Enable
********************************************************************************
*
* Summary:
*  Enables the timer.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void timer_Enable(void)
{
    uint8 enableInterrupts;

    enableInterrupts = CyEnterCriticalSection();
    timer_BLOCK_CONTROL_REG |= timer_MASK;
    CyExitCriticalSection(enableInterrupts);

    /* Statr Timer or PWM if start input is absent */
    #if (timer__PWM_SEL == timer_CONFIG)
        #if (0u == timer_PWM_START_SIGNAL_PRESENT)
            timer_TriggerCommand(timer_MASK, timer_CMD_START);
        #endif /* (0u == timer_PWM_START_SIGNAL_PRESENT) */
    #endif /* (timer__PWM_SEL == timer_CONFIG) */

    #if (timer__TIMER == timer_CONFIG)
        #if (0u == timer_TC_START_SIGNAL_PRESENT)
            timer_TriggerCommand(timer_MASK, timer_CMD_START);
        #endif /* (0u == timer_TC_START_SIGNAL_PRESENT) */
    #endif /* (timer__TIMER == timer_CONFIG) */
}


/*******************************************************************************
* Function Name: timer_Start
********************************************************************************
*
* Summary:
*  Initialize the timer with default customizer
*  values when called the first time and enables the timer.
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
*  timer_initVar: global variable is used to indicate initial
*  configuration of this component.  The variable is initialized to zero and set
*  to 1 the first time timer_Start() is called. This allows
*  enable/disable component without re-initialization in all subsequent calls
*  to the timer_Start() routine.
*
*******************************************************************************/
void timer_Start(void)
{
    if (0u == timer_initVar)
    {
        timer_Init();
        timer_initVar = 1u;
    }

    timer_Enable();
}


/*******************************************************************************
* Function Name: timer_Stop
********************************************************************************
*
* Summary:
*  Disables the timer.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void timer_Stop(void)
{
    uint8 enableInterrupts;

    enableInterrupts = CyEnterCriticalSection();

    timer_BLOCK_CONTROL_REG &= (uint32)~timer_MASK;

    CyExitCriticalSection(enableInterrupts);
}


/*******************************************************************************
* Function Name: timer_SetMode
********************************************************************************
*
* Summary:
*  Sets the operation mode of the timer. This function is used when
*  configured as a generic timer and the actual mode of operation is
*  set at runtime. The mode must be set while the component is disabled.
*
* Parameters:
*  mode: Mode for the timer to operate in
*   Values:
*   - timer_MODE_TIMER_COMPARE - Timer / Counter with
*                                                 compare capability
*         - timer_MODE_TIMER_CAPTURE - Timer / Counter with
*                                                 capture capability
*         - timer_MODE_QUAD - Quadrature decoder
*         - timer_MODE_PWM - PWM
*         - timer_MODE_PWM_DT - PWM with dead time
*         - timer_MODE_PWM_PR - PWM with pseudo random capability
*
* Return:
*  None
*
*******************************************************************************/
void timer_SetMode(uint32 mode)
{
    uint8 enableInterrupts;

    enableInterrupts = CyEnterCriticalSection();

    timer_CONTROL_REG &= (uint32)~timer_MODE_MASK;
    timer_CONTROL_REG |= mode;

    CyExitCriticalSection(enableInterrupts);
}


/*******************************************************************************
* Function Name: timer_SetQDMode
********************************************************************************
*
* Summary:
*  Sets the the Quadrature Decoder to one of 3 supported modes.
*  Is functionality is only applicable to Quadrature Decoder operation.
*
* Parameters:
*  qdMode: Quadrature Decoder mode
*   Values:
*         - timer_MODE_X1 - Counts on phi 1 rising
*         - timer_MODE_X2 - Counts on both edges of phi1 (2x faster)
*         - timer_MODE_X4 - Counts on both edges of phi1 and phi2
*                                        (4x faster)
*
* Return:
*  None
*
*******************************************************************************/
void timer_SetQDMode(uint32 qdMode)
{
    uint8 enableInterrupts;

    enableInterrupts = CyEnterCriticalSection();

    timer_CONTROL_REG &= (uint32)~timer_QUAD_MODE_MASK;
    timer_CONTROL_REG |= qdMode;

    CyExitCriticalSection(enableInterrupts);
}


/*******************************************************************************
* Function Name: timer_SetPrescaler
********************************************************************************
*
* Summary:
*  Sets the prescaler value that is applied to the clock input.  Not applicable
*  to PWM with dead time mode or Quadrature Decoder mode.
*
* Parameters:
*  prescaler: Prescaler divider value
*   Values:
*         - timer_PRESCALE_DIVBY1    - Divide by 1 (no prescaling)
*         - timer_PRESCALE_DIVBY2    - Divide by 2
*         - timer_PRESCALE_DIVBY4    - Divide by 4
*         - timer_PRESCALE_DIVBY8    - Divide by 8
*         - timer_PRESCALE_DIVBY16   - Divide by 16
*         - timer_PRESCALE_DIVBY32   - Divide by 32
*         - timer_PRESCALE_DIVBY64   - Divide by 64
*         - timer_PRESCALE_DIVBY128  - Divide by 128
*
* Return:
*  None
*
*******************************************************************************/
void timer_SetPrescaler(uint32 prescaler)
{
    uint8 enableInterrupts;

    enableInterrupts = CyEnterCriticalSection();

    timer_CONTROL_REG &= (uint32)~timer_PRESCALER_MASK;
    timer_CONTROL_REG |= prescaler;

    CyExitCriticalSection(enableInterrupts);
}


/*******************************************************************************
* Function Name: timer_SetOneShot
********************************************************************************
*
* Summary:
*  Writes the register that controls whether the timer runs
*  continuously or stops when terminal count is reached.  By default the
*  timer operates in continuous mode.
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
void timer_SetOneShot(uint32 oneShotEnable)
{
    uint8 enableInterrupts;

    enableInterrupts = CyEnterCriticalSection();

    timer_CONTROL_REG &= (uint32)~timer_ONESHOT_MASK;
    timer_CONTROL_REG |= ((uint32)((oneShotEnable & timer_1BIT_MASK) <<
                                                               timer_ONESHOT_SHIFT));

    CyExitCriticalSection(enableInterrupts);
}


/*******************************************************************************
* Function Name: timer_SetPWMMode
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
void timer_SetPWMMode(uint32 modeMask)
{
    timer_TRIG_CONTROL2_REG = (modeMask & timer_6BIT_MASK);
}



/*******************************************************************************
* Function Name: timer_SetPWMSyncKill
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
void timer_SetPWMSyncKill(uint32 syncKillEnable)
{
    uint8 enableInterrupts;

    enableInterrupts = CyEnterCriticalSection();

    timer_CONTROL_REG &= (uint32)~timer_PWM_SYNC_KILL_MASK;
    timer_CONTROL_REG |= ((uint32)((syncKillEnable & timer_1BIT_MASK)  <<
                                               timer_PWM_SYNC_KILL_SHIFT));

    CyExitCriticalSection(enableInterrupts);
}


/*******************************************************************************
* Function Name: timer_SetPWMStopOnKill
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
void timer_SetPWMStopOnKill(uint32 stopOnKillEnable)
{
    uint8 enableInterrupts;

    enableInterrupts = CyEnterCriticalSection();

    timer_CONTROL_REG &= (uint32)~timer_PWM_STOP_KILL_MASK;
    timer_CONTROL_REG |= ((uint32)((stopOnKillEnable & timer_1BIT_MASK)  <<
                                                         timer_PWM_STOP_KILL_SHIFT));

    CyExitCriticalSection(enableInterrupts);
}


/*******************************************************************************
* Function Name: timer_SetPWMDeadTime
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
void timer_SetPWMDeadTime(uint32 deadTime)
{
    uint8 enableInterrupts;

    enableInterrupts = CyEnterCriticalSection();

    timer_CONTROL_REG &= (uint32)~timer_PRESCALER_MASK;
    timer_CONTROL_REG |= ((uint32)((deadTime & timer_8BIT_MASK) <<
                                                          timer_PRESCALER_SHIFT));

    CyExitCriticalSection(enableInterrupts);
}


/*******************************************************************************
* Function Name: timer_SetPWMInvert
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
*         - timer_INVERT_LINE   - Inverts the line output
*         - timer_INVERT_LINE_N - Inverts the line_n output
*
* Return:
*  None
*
*******************************************************************************/
void timer_SetPWMInvert(uint32 mask)
{
    uint8 enableInterrupts;

    enableInterrupts = CyEnterCriticalSection();

    timer_CONTROL_REG &= (uint32)~timer_INV_OUT_MASK;
    timer_CONTROL_REG |= mask;

    CyExitCriticalSection(enableInterrupts);
}



/*******************************************************************************
* Function Name: timer_WriteCounter
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
void timer_WriteCounter(uint32 count)
{
    timer_COUNTER_REG = (count & timer_16BIT_MASK);
}


/*******************************************************************************
* Function Name: timer_ReadCounter
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
uint32 timer_ReadCounter(void)
{
    return (timer_COUNTER_REG & timer_16BIT_MASK);
}


/*******************************************************************************
* Function Name: timer_SetCounterMode
********************************************************************************
*
* Summary:
*  Sets the counter mode.  Applicable to all modes except Quadrature Decoder
*  and PWM with pseudo random output.
*
* Parameters:
*  counterMode: Enumerated couner type values
*   Values:
*     - timer_COUNT_UP       - Counts up
*     - timer_COUNT_DOWN     - Counts down
*     - timer_COUNT_UPDOWN0  - Counts up and down. Terminal count
*                                         generated when counter reaches 0
*     - timer_COUNT_UPDOWN1  - Counts up and down. Terminal count
*                                         generated both when counter reaches 0
*                                         and period
*
* Return:
*  None
*
*******************************************************************************/
void timer_SetCounterMode(uint32 counterMode)
{
    uint8 enableInterrupts;

    enableInterrupts = CyEnterCriticalSection();

    timer_CONTROL_REG &= (uint32)~timer_UPDOWN_MASK;
    timer_CONTROL_REG |= counterMode;

    CyExitCriticalSection(enableInterrupts);
}


/*******************************************************************************
* Function Name: timer_WritePeriod
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
void timer_WritePeriod(uint32 period)
{
    timer_PERIOD_REG = (period & timer_16BIT_MASK);
}


/*******************************************************************************
* Function Name: timer_ReadPeriod
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
uint32 timer_ReadPeriod(void)
{
    return (timer_PERIOD_REG & timer_16BIT_MASK);
}


/*******************************************************************************
* Function Name: timer_SetCompareSwap
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
void timer_SetCompareSwap(uint32 swapEnable)
{
    uint8 enableInterrupts;

    enableInterrupts = CyEnterCriticalSection();

    timer_CONTROL_REG &= (uint32)~timer_RELOAD_CC_MASK;
    timer_CONTROL_REG |= (swapEnable & timer_1BIT_MASK);

    CyExitCriticalSection(enableInterrupts);
}


/*******************************************************************************
* Function Name: timer_WritePeriodBuf
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
void timer_WritePeriodBuf(uint32 periodBuf)
{
    timer_PERIOD_BUF_REG = (periodBuf & timer_16BIT_MASK);
}


/*******************************************************************************
* Function Name: timer_ReadPeriodBuf
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
uint32 timer_ReadPeriodBuf(void)
{
    return (timer_PERIOD_BUF_REG & timer_16BIT_MASK);
}


/*******************************************************************************
* Function Name: timer_SetPeriodSwap
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
void timer_SetPeriodSwap(uint32 swapEnable)
{
    uint8 enableInterrupts;

    enableInterrupts = CyEnterCriticalSection();

    timer_CONTROL_REG &= (uint32)~timer_RELOAD_PERIOD_MASK;
    timer_CONTROL_REG |= ((uint32)((swapEnable & timer_1BIT_MASK) <<
                                                            timer_RELOAD_PERIOD_SHIFT));

    CyExitCriticalSection(enableInterrupts);
}


/*******************************************************************************
* Function Name: timer_WriteCompare
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
void timer_WriteCompare(uint32 compare)
{
    timer_COMP_CAP_REG = (compare & timer_16BIT_MASK);
}


/*******************************************************************************
* Function Name: timer_ReadCompare
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
uint32 timer_ReadCompare(void)
{
    return (timer_COMP_CAP_REG & timer_16BIT_MASK);
}


/*******************************************************************************
* Function Name: timer_WriteCompareBuf
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
void timer_WriteCompareBuf(uint32 compareBuf)
{
   timer_COMP_CAP_BUF_REG = (compareBuf & timer_16BIT_MASK);
}


/*******************************************************************************
* Function Name: timer_ReadCompareBuf
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
uint32 timer_ReadCompareBuf(void)
{
    return (timer_COMP_CAP_BUF_REG & timer_16BIT_MASK);
}


/*******************************************************************************
* Function Name: timer_ReadCapture
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
uint32 timer_ReadCapture(void)
{
    return (timer_COMP_CAP_REG & timer_16BIT_MASK);
}


/*******************************************************************************
* Function Name: timer_ReadCaptureBuf
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
uint32 timer_ReadCaptureBuf(void)
{
    return (timer_COMP_CAP_BUF_REG & timer_16BIT_MASK);
}


/*******************************************************************************
* Function Name: timer_SetCaptureMode
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
*     - timer_TRIG_LEVEL     - Level
*     - timer_TRIG_RISING    - Rising edge
*     - timer_TRIG_FALLING   - Falling edge
*     - timer_TRIG_BOTH      - Both rising and falling edge
*
* Return:
*  None
*
*******************************************************************************/
void timer_SetCaptureMode(uint32 triggerMode)
{
    uint8 enableInterrupts;

    enableInterrupts = CyEnterCriticalSection();

    timer_TRIG_CONTROL1_REG &= (uint32)~timer_CAPTURE_MASK;
    timer_TRIG_CONTROL1_REG |= triggerMode;

    CyExitCriticalSection(enableInterrupts);
}


/*******************************************************************************
* Function Name: timer_SetReloadMode
********************************************************************************
*
* Summary:
*  Sets the reload trigger mode. For Quadrature Decoder mode this is the index
*  input.
*
* Parameters:
*  triggerMode: Enumerated trigger mode value
*   Values:
*     - timer_TRIG_LEVEL     - Level
*     - timer_TRIG_RISING    - Rising edge
*     - timer_TRIG_FALLING   - Falling edge
*     - timer_TRIG_BOTH      - Both rising and falling edge
*
* Return:
*  None
*
*******************************************************************************/
void timer_SetReloadMode(uint32 triggerMode)
{
    uint8 enableInterrupts;

    enableInterrupts = CyEnterCriticalSection();

    timer_TRIG_CONTROL1_REG &= (uint32)~timer_RELOAD_MASK;
    timer_TRIG_CONTROL1_REG |= ((uint32)(triggerMode << timer_RELOAD_SHIFT));

    CyExitCriticalSection(enableInterrupts);
}


/*******************************************************************************
* Function Name: timer_SetStartMode
********************************************************************************
*
* Summary:
*  Sets the start trigger mode. For Quadrature Decoder mode this is the
*  phiB input.
*
* Parameters:
*  triggerMode: Enumerated trigger mode value
*   Values:
*     - timer_TRIG_LEVEL     - Level
*     - timer_TRIG_RISING    - Rising edge
*     - timer_TRIG_FALLING   - Falling edge
*     - timer_TRIG_BOTH      - Both rising and falling edge
*
* Return:
*  None
*
*******************************************************************************/
void timer_SetStartMode(uint32 triggerMode)
{
    uint8 enableInterrupts;

    enableInterrupts = CyEnterCriticalSection();

    timer_TRIG_CONTROL1_REG &= (uint32)~timer_START_MASK;
    timer_TRIG_CONTROL1_REG |= ((uint32)(triggerMode << timer_START_SHIFT));

    CyExitCriticalSection(enableInterrupts);
}


/*******************************************************************************
* Function Name: timer_SetStopMode
********************************************************************************
*
* Summary:
*  Sets the stop trigger mode. For PWM mode this is the kill input.
*
* Parameters:
*  triggerMode: Enumerated trigger mode value
*   Values:
*     - timer_TRIG_LEVEL     - Level
*     - timer_TRIG_RISING    - Rising edge
*     - timer_TRIG_FALLING   - Falling edge
*     - timer_TRIG_BOTH      - Both rising and falling edge
*
* Return:
*  None
*
*******************************************************************************/
void timer_SetStopMode(uint32 triggerMode)
{
    uint8 enableInterrupts;

    enableInterrupts = CyEnterCriticalSection();

    timer_TRIG_CONTROL1_REG &= (uint32)~timer_STOP_MASK;
    timer_TRIG_CONTROL1_REG |= ((uint32)(triggerMode << timer_STOP_SHIFT));

    CyExitCriticalSection(enableInterrupts);
}


/*******************************************************************************
* Function Name: timer_SetCountMode
********************************************************************************
*
* Summary:
*  Sets the count trigger mode. For Quadrature Decoder mode this is the phiA
*  input.
*
* Parameters:
*  triggerMode: Enumerated trigger mode value
*   Values:
*     - timer_TRIG_LEVEL     - Level
*     - timer_TRIG_RISING    - Rising edge
*     - timer_TRIG_FALLING   - Falling edge
*     - timer_TRIG_BOTH      - Both rising and falling edge
*
* Return:
*  None
*
*******************************************************************************/
void timer_SetCountMode(uint32 triggerMode)
{
    uint8 enableInterrupts;

    enableInterrupts = CyEnterCriticalSection();

    timer_TRIG_CONTROL1_REG &= (uint32)~timer_COUNT_MASK;
    timer_TRIG_CONTROL1_REG |= ((uint32)(triggerMode << timer_COUNT_SHIFT));

    CyExitCriticalSection(enableInterrupts);
}


/*******************************************************************************
* Function Name: timer_TriggerCommand
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
*     - timer_CMD_CAPTURE    - Trigger Capture command
*     - timer_CMD_RELOAD     - Trigger Reload command
*     - timer_CMD_STOP       - Trigger Stop command
*     - timer_CMD_START      - Trigger Start command
*
* Return:
*  None
*
*******************************************************************************/
void timer_TriggerCommand(uint32 mask, uint32 command)
{
    uint8 enableInterrupts;

    enableInterrupts = CyEnterCriticalSection();

    timer_COMMAND_REG = ((uint32)(mask << command));

    CyExitCriticalSection(enableInterrupts);
}


/*******************************************************************************
* Function Name: timer_ReadStatus
********************************************************************************
*
* Summary:
*  Reads the status of the timer.
*
* Parameters:
*  None
*
* Return:
*  Status
*   Values:
*     - timer_STATUS_DOWN    - Set if counting down
*     - timer_STATUS_RUNNING - Set if counter is running
*
*******************************************************************************/
uint32 timer_ReadStatus(void)
{
    return ((timer_STATUS_REG >> timer_RUNNING_STATUS_SHIFT) |
            (timer_STATUS_REG & timer_STATUS_DOWN));
}


/*******************************************************************************
* Function Name: timer_SetInterruptMode
********************************************************************************
*
* Summary:
*  Sets the interrupt mask to control which interrupt
*  requests generate the interrupt signal.
*
* Parameters:
*   interruptMask: Mask of bits to be enabled
*   Values:
*     - timer_INTR_MASK_TC       - Terminal count mask
*     - timer_INTR_MASK_CC_MATCH - Compare count / capture mask
*
* Return:
*  None
*
*******************************************************************************/
void timer_SetInterruptMode(uint32 interruptMask)
{
    timer_INTERRUPT_MASK_REG =  interruptMask;
}


/*******************************************************************************
* Function Name: timer_GetInterruptSourceMasked
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
*     - timer_INTR_MASK_TC       - Terminal count mask
*     - timer_INTR_MASK_CC_MATCH - Compare count / capture mask
*
*******************************************************************************/
uint32 timer_GetInterruptSourceMasked(void)
{
    return (timer_INTERRUPT_MASKED_REG);
}


/*******************************************************************************
* Function Name: timer_GetInterruptSource
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
*     - timer_INTR_MASK_TC       - Terminal count mask
*     - timer_INTR_MASK_CC_MATCH - Compare count / capture mask
*
*******************************************************************************/
uint32 timer_GetInterruptSource(void)
{
    return (timer_INTERRUPT_REQ_REG);
}


/*******************************************************************************
* Function Name: timer_ClearInterrupt
********************************************************************************
*
* Summary:
*  Clears the interrupt request.
*
* Parameters:
*   interruptMask: Mask of interrupts to clear
*   Values:
*     - timer_INTR_MASK_TC       - Terminal count mask
*     - timer_INTR_MASK_CC_MATCH - Compare count / capture mask
*
* Return:
*  None
*
*******************************************************************************/
void timer_ClearInterrupt(uint32 interruptMask)
{
    timer_INTERRUPT_REQ_REG = interruptMask;
}


/*******************************************************************************
* Function Name: timer_SetInterrupt
********************************************************************************
*
* Summary:
*  Sets a software interrupt request.
*
* Parameters:
*   interruptMask: Mask of interrupts to set
*   Values:
*     - timer_INTR_MASK_TC       - Terminal count mask
*     - timer_INTR_MASK_CC_MATCH - Compare count / capture mask
*
* Return:
*  None
*
*******************************************************************************/
void timer_SetInterrupt(uint32 interruptMask)
{
    timer_INTERRUPT_SET_REG = interruptMask;
}


/* [] END OF FILE */
