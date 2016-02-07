/*******************************************************************************
* File Name: TMP05_INT.c
* Version 1.10
*
* Description:
*  This file provides Interrupt Service Routine (ISR) for the TMP05
*  component.
*
* Note:
*  None
*
********************************************************************************
* Copyright 2012-2013, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "TMP05_PVT.h"


/*******************************************************************************
*  Place your includes, defines and code here
********************************************************************************/
/* `#START TMP05_INT_HEADER` */

/* `#END` */


/*******************************************************************************
* Function Name: TMP05_ISR_Interrupt
********************************************************************************
*
* Summary:
*   The default Interrupt Service Routine for TMP05_ISR.
*
*   Add custom code between the coments to keep the next version of this file
*   from over writting your code.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global Variables:
*  TMP05_busyFlag - used for reflect sensor masuring.
*  TMP05_contMode - used for reflect modes of operation used:
*     - TMP05_MODE_CONTINUOUS.
*     - TMP05_MODE_ONESHOT.
*
*******************************************************************************/
CY_ISR(TMP05_EOC_ISR_Int)
{
    uint8 enableInterrupts;
    uint8 indexVar;

    for(indexVar = 0u; indexVar < TMP05_CUSTOM_NUM_SENSORS; indexVar++)
    {
        /* Store high time of sensor PWM */
        TMP05_hi[indexVar] = CY_GET_REG16(TMP05_HI_CNT_PTR);

         /* Store low time of sensor PWM */
        TMP05_lo[indexVar] = CY_GET_REG16(TMP05_LO_CNT_PTR);
    }
        /* Change shared regs, need to be safety */
        enableInterrupts = CyEnterCriticalSection();

       TMP05_CONTROL_REG ^= TMP05_CTRL_EOC;

        /* shared regs config are done */
        CyExitCriticalSection(enableInterrupts);
    
    /* In continuous mode, trigger next measurement, otherwise clear busy flag */
    if (TMP05_contMode == TMP05_MODE_CONTINUOUS)
    {
        /* Change shared regs, need to be safety */
        enableInterrupts = CyEnterCriticalSection();

       TMP05_CONTROL_REG ^= TMP05_CTRL_TRIG;

        /* shared regs config are done */
        CyExitCriticalSection(enableInterrupts);
    }
    else
    {
        TMP05_busyFlag = 0u;
    }

}


/* [] END OF FILE */
