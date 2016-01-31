/*******************************************************************************
* File Name: ledhb.h  
* Version 1.70
*
* Description:
*  This file containts Control Register function prototypes and register defines
*
* Note:
*
********************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

#if !defined(CY_PINS_ledhb_H) /* Pins ledhb_H */
#define CY_PINS_ledhb_H

#include "cytypes.h"
#include "cyfitter.h"
#include "cypins.h"
#include "ledhb_aliases.h"

/* Check to see if required defines such as CY_PSOC5A are available */
/* They are defined starting with cy_boot v3.0 */
#if !defined (CY_PSOC5A)
    #error Component cy_pins_v1_70 requires cy_boot v3.0 or later
#endif /* (CY_PSOC5A) */

/* APIs are not generated for P15[7:6] */
#if !(CY_PSOC5A &&\
	 ledhb__PORT == 15 && (ledhb__MASK & 0xC0))

/***************************************
*        Function Prototypes             
***************************************/    

void    ledhb_Write(uint8 value) ;
void    ledhb_SetDriveMode(uint8 mode) ;
uint8   ledhb_ReadDataReg(void) ;
uint8   ledhb_Read(void) ;
uint8   ledhb_ClearInterrupt(void) ;

/***************************************
*           API Constants        
***************************************/

/* Drive Modes */
#define ledhb_DM_ALG_HIZ         PIN_DM_ALG_HIZ
#define ledhb_DM_DIG_HIZ         PIN_DM_DIG_HIZ
#define ledhb_DM_RES_UP          PIN_DM_RES_UP
#define ledhb_DM_RES_DWN         PIN_DM_RES_DWN
#define ledhb_DM_OD_LO           PIN_DM_OD_LO
#define ledhb_DM_OD_HI           PIN_DM_OD_HI
#define ledhb_DM_STRONG          PIN_DM_STRONG
#define ledhb_DM_RES_UPDWN       PIN_DM_RES_UPDWN

/* Digital Port Constants */
#define ledhb_MASK               ledhb__MASK
#define ledhb_SHIFT              ledhb__SHIFT
#define ledhb_WIDTH              1u

/***************************************
*             Registers        
***************************************/

/* Main Port Registers */
/* Pin State */
#define ledhb_PS                     (* (reg8 *) ledhb__PS)
/* Data Register */
#define ledhb_DR                     (* (reg8 *) ledhb__DR)
/* Port Number */
#define ledhb_PRT_NUM                (* (reg8 *) ledhb__PRT) 
/* Connect to Analog Globals */                                                  
#define ledhb_AG                     (* (reg8 *) ledhb__AG)                       
/* Analog MUX bux enable */
#define ledhb_AMUX                   (* (reg8 *) ledhb__AMUX) 
/* Bidirectional Enable */                                                        
#define ledhb_BIE                    (* (reg8 *) ledhb__BIE)
/* Bit-mask for Aliased Register Access */
#define ledhb_BIT_MASK               (* (reg8 *) ledhb__BIT_MASK)
/* Bypass Enable */
#define ledhb_BYP                    (* (reg8 *) ledhb__BYP)
/* Port wide control signals */                                                   
#define ledhb_CTL                    (* (reg8 *) ledhb__CTL)
/* Drive Modes */
#define ledhb_DM0                    (* (reg8 *) ledhb__DM0) 
#define ledhb_DM1                    (* (reg8 *) ledhb__DM1)
#define ledhb_DM2                    (* (reg8 *) ledhb__DM2) 
/* Input Buffer Disable Override */
#define ledhb_INP_DIS                (* (reg8 *) ledhb__INP_DIS)
/* LCD Common or Segment Drive */
#define ledhb_LCD_COM_SEG            (* (reg8 *) ledhb__LCD_COM_SEG)
/* Enable Segment LCD */
#define ledhb_LCD_EN                 (* (reg8 *) ledhb__LCD_EN)
/* Slew Rate Control */
#define ledhb_SLW                    (* (reg8 *) ledhb__SLW)

/* DSI Port Registers */
/* Global DSI Select Register */
#define ledhb_PRTDSI__CAPS_SEL       (* (reg8 *) ledhb__PRTDSI__CAPS_SEL) 
/* Double Sync Enable */
#define ledhb_PRTDSI__DBL_SYNC_IN    (* (reg8 *) ledhb__PRTDSI__DBL_SYNC_IN) 
/* Output Enable Select Drive Strength */
#define ledhb_PRTDSI__OE_SEL0        (* (reg8 *) ledhb__PRTDSI__OE_SEL0) 
#define ledhb_PRTDSI__OE_SEL1        (* (reg8 *) ledhb__PRTDSI__OE_SEL1) 
/* Port Pin Output Select Registers */
#define ledhb_PRTDSI__OUT_SEL0       (* (reg8 *) ledhb__PRTDSI__OUT_SEL0) 
#define ledhb_PRTDSI__OUT_SEL1       (* (reg8 *) ledhb__PRTDSI__OUT_SEL1) 
/* Sync Output Enable Registers */
#define ledhb_PRTDSI__SYNC_OUT       (* (reg8 *) ledhb__PRTDSI__SYNC_OUT) 


#if defined(ledhb__INTSTAT)  /* Interrupt Registers */

    #define ledhb_INTSTAT                (* (reg8 *) ledhb__INTSTAT)
    #define ledhb_SNAP                   (* (reg8 *) ledhb__SNAP)

#endif /* Interrupt Registers */

#endif /* End Pins ledhb_H */

#endif
/* [] END OF FILE */
