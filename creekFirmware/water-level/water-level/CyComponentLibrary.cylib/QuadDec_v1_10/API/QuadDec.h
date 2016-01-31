/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*     This file provides constants and parameter values for the Quadrature
*      Decoder component.
*
*   Note:
*     None
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#ifndef CY_QUADRATURE_DECODER_`$INSTANCE_NAME`_H
#define CY_QUADRATURE_DECODER_`$INSTANCE_NAME`_H

#include "cytypes.h"
#include "cyfitter.h"
#include "`$INSTANCE_NAME`_Control_Reg.h"
`$writeIncludeCntHFile`

#if(CYDEV_CHIP_DIE_EXPECT == CYDEV_CHIP_DIE_LEOPARD)     
    #if((CYDEV_CHIP_REV_EXPECT <= CYDEV_CHIP_REV_LEOPARD_ES2) && (`$INSTANCE_NAME`_isr__ES2_PATCH))      
        #include <intrins.h>
        #define `$INSTANCE_NAME`_ISR_PATCH() _nop_(); _nop_(); _nop_(); _nop_(); _nop_(); _nop_(); _nop_(); _nop_();
    #endif
#endif


/***************************************
*   Conditional Compilation Parameters
***************************************/


/***************************************
*        Function Prototypes
***************************************/

void    `$INSTANCE_NAME`_Start(void);
void    `$INSTANCE_NAME`_Stop(void);
uint8   `$INSTANCE_NAME`_GetEvents(void);
void    `$INSTANCE_NAME`_SetInterruptMask(uint8 mask);
uint8   `$INSTANCE_NAME`_GetInterruptMask(void);
`$writeHFile`
CY_ISR_PROTO(`$INSTANCE_NAME`_ISR);


/***************************************
*           API Constants
***************************************/
`$writeInterruptMaskDefine`

/***************************************
*    Enumerated Types and Parameters
***************************************/

#define `$INSTANCE_NAME`_CounterSize                `$CounterSize`
#define `$INSTANCE_NAME`_GlitchFiltering            `$UsingGlitchFiltering`
#define `$INSTANCE_NAME`_IndexInput                 `$UsingIndexInput`
#define `$INSTANCE_NAME`_CounterResolution          `$CounterResolution`
#define `$INSTANCE_NAME`_ISR_NUMBER                 `$INSTANCE_NAME`_isr__INTC_NUMBER
#define `$INSTANCE_NAME`_ISR_PRIORITY               `$INSTANCE_NAME`_isr__INTC_PRIOR_NUM

/***************************************
*    Initial Parameter Constants
***************************************/


/***************************************
*             Registers
***************************************/

#define `$INSTANCE_NAME`_STATUS                     (* (reg8 *) `$INSTANCE_NAME`_bQuadDec_Stsreg__STATUS_REG)
#define `$INSTANCE_NAME`_STATUS_MASK                (* (reg8 *) `$INSTANCE_NAME`_bQuadDec_Stsreg__MASK_REG)
#define `$INSTANCE_NAME`_SR_AUX_CONTROL             (* (reg8 *) `$INSTANCE_NAME`_bQuadDec_Stsreg__STATUS_AUX_CTL_REG)


/***************************************
*       Software Registers
***************************************/
`$writeSoftwareCounterDefine`
/***************************************
*        Register Constants
***************************************/

#define `$INSTANCE_NAME`_COUNTER_OVERFLOW_SHIFT     0x00u
#define `$INSTANCE_NAME`_COUNTER_UNDERFLOW_SHIFT    0x01u
#define `$INSTANCE_NAME`_RESET_SHIFT                0x02u
#define `$INSTANCE_NAME`_INVALID_IN_SHIFT           0x03u
#define `$INSTANCE_NAME`_COUNTER_OVERFLOW           (0x01u << `$INSTANCE_NAME`_COUNTER_OVERFLOW_SHIFT)
#define `$INSTANCE_NAME`_COUNTER_UNDERFLOW          (0x01u << `$INSTANCE_NAME`_COUNTER_UNDERFLOW_SHIFT)
#define `$INSTANCE_NAME`_RESET                      (0x01u << `$INSTANCE_NAME`_RESET_SHIFT)
#define `$INSTANCE_NAME`_INVALID_IN                 (0x01u << `$INSTANCE_NAME`_INVALID_IN_SHIFT)

#define `$INSTANCE_NAME`_ENABLE_SHIFT               0x00u
#define `$INSTANCE_NAME`_ENABLE                     (0x01u << `$INSTANCE_NAME`_ENABLE_SHIFT)
#define `$INSTANCE_NAME`_INTERRUPTS_ENABLE_SHIFT    0x04u
#define `$INSTANCE_NAME`_INTERRUPTS_ENABLE          (0x01u << `$INSTANCE_NAME`_INTERRUPTS_ENABLE_SHIFT)
#define `$INSTANCE_NAME`_START_RESET_SHIFT          0x01u
#define `$INSTANCE_NAME`_START_RESET                (0x01u << `$INSTANCE_NAME`_START_RESET_SHIFT)
#define `$INSTANCE_NAME`_INIT_INT_MASK              0x0Fu
#define `$INSTANCE_NAME`_DISABLE                    0x00u

#endif /* CY_QUADRATURE_DECODER_`$INSTANCE_NAME`_H */


/* [] END OF FILE */
