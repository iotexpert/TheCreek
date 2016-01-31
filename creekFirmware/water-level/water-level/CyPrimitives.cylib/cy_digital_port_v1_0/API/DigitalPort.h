/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file containts Control Register function prototypes and register defines
*
* Note:
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

#if !defined(CY_DIGITAL_PORT_`$INSTANCE_NAME`_H) /* Digital Port `$INSTANCE_NAME`_H */
#define CY_DIGITAL_PORT_`$INSTANCE_NAME`_H

#include "cytypes.h"
#include "cyfitter.h"


/***************************************
*        Function Prototypes             
***************************************/    

void    `$INSTANCE_NAME`_Write(uint8 prtValue);
void    `$INSTANCE_NAME`_WriteDM(uint8 mode, uint8 mask);
uint8   `$INSTANCE_NAME`_ReadDR(void);
uint8   `$INSTANCE_NAME`_Read(void);
uint8   `$INSTANCE_NAME`_ClearInterrupt(void);
uint8   `$INSTANCE_NAME`_GetLastInterrupt(void);

/* 
 * Macros below provide access to write the DR and read the PS 
 * using the same nomenclature as above
 */
#define `$INSTANCE_NAME`_WriteDR(prtValue)  `$INSTANCE_NAME`_Write(prtValue) 
#define `$INSTANCE_NAME`_ReadPS()           `$INSTANCE_NAME`_Read()


`$DriveModeMacros_API_GEN`

/***************************************
*           API Constants        
***************************************/

/* Drive Mode Constants for WriteDM() */
#define `$INSTANCE_NAME`_BIT_SET            0xFFu
#define `$INSTANCE_NAME`_BIT_CLEAR          0x00u
#define `$INSTANCE_NAME`_MODE_MASK          0x07u
#define `$INSTANCE_NAME`_MODE_BIT_0         0x01u
#define `$INSTANCE_NAME`_MODE_BIT_1         0x02u
#define `$INSTANCE_NAME`_MODE_BIT_2         0x04u
#define `$INSTANCE_NAME`_PC_DM_MASK         0x0Eu
#define `$INSTANCE_NAME`_PC_DM_SHIFT        0x01u

/* Drive Modes */
#define `$INSTANCE_NAME`_HI_Z               0x01u
#define `$INSTANCE_NAME`_RES_PULL_UP        0x02u
#define `$INSTANCE_NAME`_RES_PULL_DOWN      0x03u
#define `$INSTANCE_NAME`_OPEN_DRAIN_LO      0x04u
#define `$INSTANCE_NAME`_OPEN_DRAIN_HI      0x05u
#define `$INSTANCE_NAME`_CMOS_OUT           0x06u
#define `$INSTANCE_NAME`_RES_PULL_UPDOWN    0x07u

/* Digital Port Constants */
#define `$INSTANCE_NAME`_MASK               `$INSTANCE_NAME`__MASK
#define `$INSTANCE_NAME`_SHIFT              `$INSTANCE_NAME`__SHIFT
#define `$INSTANCE_NAME`_WIDTH              `$Width`u

/***************************************
*             Registers        
***************************************/

/* Main Port Registers */
/* Pin State */
#define `$INSTANCE_NAME`_PS                     (* (reg8 *) `$INSTANCE_NAME`__PS)
/* Data Register */
#define `$INSTANCE_NAME`_DR                     (* (reg8 *) `$INSTANCE_NAME`__DR)
/* Port Number */
#define `$INSTANCE_NAME`_PRT_NUM                (* (reg8 *) `$INSTANCE_NAME`__PRT) 
/* Connect to Analog Globals */                                                  
#define `$INSTANCE_NAME`_AG                     (* (reg8 *) `$INSTANCE_NAME`__AG)                       
/* Analog MUX bux enable */
#define `$INSTANCE_NAME`_AMUX                   (* (reg8 *) `$INSTANCE_NAME`__AMUX) 
/* Bidirectional Enable */                                                        
#define `$INSTANCE_NAME`_BIE                    (* (reg8 *) `$INSTANCE_NAME`__BIE)
/* Bit-mask for Aliased Register Access */
#define `$INSTANCE_NAME`_BIT_MASK               (* (reg8 *) `$INSTANCE_NAME`__BIT_MASK)
/* Bypass Enable */
#define `$INSTANCE_NAME`_BYP                    (* (reg8 *) `$INSTANCE_NAME`__BYP)
/* Port wide control signals */                                                   
#define `$INSTANCE_NAME`_CTL                    (* (reg8 *) `$INSTANCE_NAME`__CTL)
/* Drive Modes */
#define `$INSTANCE_NAME`_DM0                    (* (reg8 *) `$INSTANCE_NAME`__DM0) 
#define `$INSTANCE_NAME`_DM1                    (* (reg8 *) `$INSTANCE_NAME`__DM1)
#define `$INSTANCE_NAME`_DM2                    (* (reg8 *) `$INSTANCE_NAME`__DM2) 
/* Input Buffer Disable Override */
#define `$INSTANCE_NAME`_INP_DIS                (* (reg8 *) `$INSTANCE_NAME`__INP_DIS)
/* LCD Common or Segment Drive */
#define `$INSTANCE_NAME`_LCD_COM_SEG            (* (reg8 *) `$INSTANCE_NAME`__LCD_COM_SEG)
/* Enable Segment LCD */
#define `$INSTANCE_NAME`_LCD_EN                 (* (reg8 *) `$INSTANCE_NAME`__LCD_EN)
/* Slew Rate Control */
#define `$INSTANCE_NAME`_SLW                    (* (reg8 *) `$INSTANCE_NAME`__SLW)

/* DSI Port Registers */
/* Global DSI Select Register */
#define `$INSTANCE_NAME`_PRTDSI__CAPS_SEL       (* (reg8 *) `$INSTANCE_NAME`__PRTDSI__CAPS_SEL) 
/* Double Sync Enable */
#define `$INSTANCE_NAME`_PRTDSI__DBL_SYNC_IN    (* (reg8 *) `$INSTANCE_NAME`__PRTDSI__DBL_SYNC_IN) 
/* Output Enable Select Drive Strength */
#define `$INSTANCE_NAME`_PRTDSI__OE_SEL0        (* (reg8 *) `$INSTANCE_NAME`__PRTDSI__OE_SEL0) 
#define `$INSTANCE_NAME`_PRTDSI__OE_SEL1        (* (reg8 *) `$INSTANCE_NAME`__PRTDSI__OE_SEL1) 
/* Port Pin Output Select Registers */
#define `$INSTANCE_NAME`_PRTDSI__OUT_SEL0       (* (reg8 *) `$INSTANCE_NAME`__PRTDSI__OUT_SEL0) 
#define `$INSTANCE_NAME`_PRTDSI__OUT_SEL1       (* (reg8 *) `$INSTANCE_NAME`__PRTDSI__OUT_SEL1) 
/* Sync Output Enable Registers */
#define `$INSTANCE_NAME`_PRTDSI__SYNC_OUT       (* (reg8 *) `$INSTANCE_NAME`__PRTDSI__SYNC_OUT) 


#if defined(`$INSTANCE_NAME`__INTSTAT)  /* Interrupt Registers */

    #define `$INSTANCE_NAME`_INTSTAT                (* (reg8 *) `$INSTANCE_NAME`__INTSTAT)
    #define `$INSTANCE_NAME`_SNAP                   (* (reg8 *) `$INSTANCE_NAME`__SNAP)

#endif /* Interrupt Registers */

`$PinConfigurationRegisters_API_GEN`

#endif /* End Digital Port `$INSTANCE_NAME`_H */


/* [] END OF FILE */
