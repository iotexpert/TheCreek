/*******************************************************************************
* File Name: `$INSTANCE_NAME`_PVT.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This header file contains internal definitions for the FILT component.
*  It must be included after `$INSTANCE_NAME`.h.
*
* Note:
* 
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#if !defined(`$INSTANCE_NAME`_PVT_H) /* FILT Header File */
#define `$INSTANCE_NAME`_PVT_H

#include "`$INSTANCE_NAME`.H"

/*******************************************************************************
* FILT component internal function prototypes.
*******************************************************************************/

extern void   `$INSTANCE_NAME`_SetInterruptMode(uint8 events);
extern void   `$INSTANCE_NAME`_SetDMAMode(uint8 events);

/*******************************************************************************
* FILT component internal variables.
*******************************************************************************/

extern const uint8 CYCODE `$INSTANCE_NAME`_control[]; 
extern const uint8 CYCODE `$INSTANCE_NAME`_data_a[];
extern const uint8 CYCODE `$INSTANCE_NAME`_data_b[];
extern const uint8 CYCODE `$INSTANCE_NAME`_cfsm[];
extern const uint8 CYCODE `$INSTANCE_NAME`_acu[];

/*******************************************************************************
* FILT component internal constants.
*******************************************************************************/

/* Parameters */
#define `$INSTANCE_NAME`_INIT_INTERRUPT_MODE    `$initialIrqMode_API_GEN`u
#define `$INSTANCE_NAME`_INIT_DMA_MODE          `$initialDmaMode_API_GEN`u
#define `$INSTANCE_NAME`_INIT_COHER             `$initialCoherency_API_GEN`u
#define `$INSTANCE_NAME`_INIT_DALIGN            `$initialDataAlign_API_GEN`u

/* RAM memory map offsets */
#define `$INSTANCE_NAME`_DA_RAM         (void XDATA *) (`$INSTANCE_NAME`_DFB__DPA_SRAM_DATA)
#define `$INSTANCE_NAME`_DB_RAM         (void XDATA *) (`$INSTANCE_NAME`_DFB__DPB_SRAM_DATA)
#define `$INSTANCE_NAME`_CSA_RAM        (void XDATA *) (`$INSTANCE_NAME`_DFB__CSA_SRAM_DATA)
#define `$INSTANCE_NAME`_CSB_RAM        (void XDATA *) (`$INSTANCE_NAME`_DFB__CSB_SRAM_DATA)
#define `$INSTANCE_NAME`_CFSM_RAM       (void XDATA *) (`$INSTANCE_NAME`_DFB__FSM_SRAM_DATA)
#define `$INSTANCE_NAME`_ACU_RAM        (void XDATA *) (`$INSTANCE_NAME`_DFB__ACU_SRAM_DATA)

// RAM register space sizes in bytes.
#define `$INSTANCE_NAME`_DA_RAM_SIZE    0x200u
#define `$INSTANCE_NAME`_DB_RAM_SIZE    0x200u
#define `$INSTANCE_NAME`_CSA_RAM_SIZE   0x100u
#define `$INSTANCE_NAME`_CSB_RAM_SIZE   0x100u
#define `$INSTANCE_NAME`_CFSM_RAM_SIZE  0x100u
#define `$INSTANCE_NAME`_ACU_RAM_SIZE   0x040u

/*******************************************************************************
* FILT component internal register contents.
*******************************************************************************/

#define `$INSTANCE_NAME`_PM_ACT_MSK     `$INSTANCE_NAME`_DFB__PM_ACT_MSK 
#define `$INSTANCE_NAME`_PM_STBY_MSK    `$INSTANCE_NAME`_DFB__PM_STBY_MSK 

#define `$INSTANCE_NAME`_RUN_MASK           0x01u
#define `$INSTANCE_NAME`_EVENT_MASK         0x1Fu
#define `$INSTANCE_NAME`_SR_EVENT_SHIFT     0x03u
#define `$INSTANCE_NAME`_DMA_CTRL_MASK      0x0Fu
#define `$INSTANCE_NAME`_RAM_DIR_BUS        0x3Fu
#define `$INSTANCE_NAME`_RAM_DIR_DFB        0x00u

/*******************************************************************************
* FILT component internal DFB registers.
*******************************************************************************/

#define `$INSTANCE_NAME`_CR             * (reg8 *) `$INSTANCE_NAME`_DFB__CR 
/* Defined in `$INSTANCE_NAME`.h */ 
/* #define `$INSTANCE_NAME`_SR          * (reg8 *) `$INSTANCE_NAME`_DFB__SR */
#define `$INSTANCE_NAME`_INT_CTRL       * (reg8 *) `$INSTANCE_NAME`_DFB__INT_CTRL 
#define `$INSTANCE_NAME`_DMA_CTRL       * (reg8 *) `$INSTANCE_NAME`_DFB__DMA_CTRL
#define `$INSTANCE_NAME`_RAM_DIR        * (reg8 *) `$INSTANCE_NAME`_DFB__RAM_DIR 

#define `$INSTANCE_NAME`_DALIGN         * (reg8 *) `$INSTANCE_NAME`_DFB__DALIGN 
#define `$INSTANCE_NAME`_DSI_CTRL       * (reg8 *) `$INSTANCE_NAME`_DFB__DSI_CTRL 
#define `$INSTANCE_NAME`_HOLDA          * (reg8 *) `$INSTANCE_NAME`_DFB__HOLDA 
#define `$INSTANCE_NAME`_HOLDAH         * (reg8 *) `$INSTANCE_NAME`_DFB__HOLDAH
#define `$INSTANCE_NAME`_HOLDAM         * (reg8 *) `$INSTANCE_NAME`_DFB__HOLDAM
#define `$INSTANCE_NAME`_HOLDB          * (reg8 *) `$INSTANCE_NAME`_DFB__HOLDB 
#define `$INSTANCE_NAME`_HOLDBH         * (reg8 *) `$INSTANCE_NAME`_DFB__HOLDBH
#define `$INSTANCE_NAME`_HOLDBM         * (reg8 *) `$INSTANCE_NAME`_DFB__HOLDBM
#define `$INSTANCE_NAME`_PM_ACT_CFG     * (reg8 *) `$INSTANCE_NAME`_DFB__PM_ACT_CFG 
#define `$INSTANCE_NAME`_PM_STBY_CFG    * (reg8 *) `$INSTANCE_NAME`_DFB__PM_STBY_CFG 
#define `$INSTANCE_NAME`_RAM_EN         * (reg8 *) `$INSTANCE_NAME`_DFB__RAM_EN 
#define `$INSTANCE_NAME`_STAGEA         * (reg8 *) `$INSTANCE_NAME`_DFB__STAGEA 
#define `$INSTANCE_NAME`_STAGEAH        * (reg8 *) `$INSTANCE_NAME`_DFB__STAGEAH
#define `$INSTANCE_NAME`_STAGEAM        * (reg8 *) `$INSTANCE_NAME`_DFB__STAGEAM
#define `$INSTANCE_NAME`_STAGEB         * (reg8 *) `$INSTANCE_NAME`_DFB__STAGEB 
#define `$INSTANCE_NAME`_STAGEBH        * (reg8 *) `$INSTANCE_NAME`_DFB__STAGEBH
#define `$INSTANCE_NAME`_STAGEBM        * (reg8 *) `$INSTANCE_NAME`_DFB__STAGEBM
#define `$INSTANCE_NAME`_COHER          * (reg8 *) `$INSTANCE_NAME`_DFB__COHER
#define `$INSTANCE_NAME`_DALIGN         * (reg8 *) `$INSTANCE_NAME`_DFB__DALIGN

#define `$INSTANCE_NAME`_HOLDA16        (* (reg16 *) `$INSTANCE_NAME`_DFB__HOLDAM)
#define `$INSTANCE_NAME`_HOLDB16        (* (reg16 *) `$INSTANCE_NAME`_DFB__HOLDBM)
#define `$INSTANCE_NAME`_STAGEA16       (* (reg16 *) `$INSTANCE_NAME`_DFB__STAGEAM)
#define `$INSTANCE_NAME`_STAGEB16       (* (reg16 *) `$INSTANCE_NAME`_DFB__STAGEBM)

#define `$INSTANCE_NAME`_HOLDA24        (* (reg32 *) `$INSTANCE_NAME`_DFB__HOLDA)
#define `$INSTANCE_NAME`_HOLDB24        (* (reg32 *) `$INSTANCE_NAME`_DFB__HOLDB)
#define `$INSTANCE_NAME`_STAGEA24       (* (reg32 *) `$INSTANCE_NAME`_DFB__STAGEA)
#define `$INSTANCE_NAME`_STAGEB24       (* (reg32 *) `$INSTANCE_NAME`_DFB__STAGEB)

#endif /* End FILT_PVT Header File */

/* [] END OF FILE */
